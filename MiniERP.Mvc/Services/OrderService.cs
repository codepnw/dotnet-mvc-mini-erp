using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IOrderService
{
    Task<Result> CreateOrder(OrderCreateDto dto);
    Task<Result<PagedResult<OrderViewModel>>> ListOrders(OrderQuery req);
    Task<Result<OrderDetailsViewModel>> GetOrderDetails(int orderId);
    Task<Result> CancelOrder(int orderId);    
}

public class OrderService(AppDbContext context) : IOrderService
{
    private readonly AppDbContext _context = context;

    public async Task<Result> CreateOrder(OrderCreateDto dto)
    {
        var productIds = dto.Items.Select(x => x.ProductId).ToList();
        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync();

        var order = new Order
        {
            // TODO: change later
            UserId = 1,
            // TODO: change later
            OrderNumber = "mock-order-number-0000",
            Status = OrderStatus.Completed,
            CreatedAt = DateTime.UtcNow,
        };

        var totalPrice = 0m;

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0) return Result.Failure("Quantity must be positive", ErrorCode.BadRequest);

            // Find Product
            var product = products.FirstOrDefault(x => x.Id == item.ProductId);

            if (product is null)
                return Result.Failure(
                    $"Product {item.ProductId} not found",
                    ErrorCode.NotFound
                );

            // Check Product Stock
            if (product.Stock < item.Quantity)
                return Result.Failure(
                    $"Product {item.ProductId} not enough stock",
                    ErrorCode.BadRequest
                );

            // Calculate Total Price
            var subTotal = product.Price * item.Quantity;
            totalPrice += subTotal;

            // Decrease Product Stock
            product.Stock -= item.Quantity;

            // Insert Stock Movement
            _context.StockMovements.Add(new StockMovement
            {
                ProductId = product.Id,
                Quantity = -item.Quantity,
                MovementType = StockMovementType.Out,
                CreatedAt = DateTime.UtcNow
            });

            // Insert Order Items
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                SubTotal = subTotal,
            });
        }

        // Update Total Amount
        order.TotalAmount = totalPrice;
        // Insert Order
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<PagedResult<OrderViewModel>>> ListOrders(OrderQuery req)
    {
        var query = _context.Orders.AsNoTracking().AsQueryable();

        // Filter Status
        if (req.Status.HasValue)
        {
            query = query.Where(x => x.Status == req.Status.Value);
        }

        // Filter From Date
        if (req.FromDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= req.FromDate.Value.Date);
        }

        // Filter To Date
        if (req.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= req.ToDate.Value.Date);
        }

        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Paginate(req.Page, req.PageSize)
            .Select(x => new OrderViewModel
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
            })
            .ToListAsync();

        var result = new PagedResult<OrderViewModel>()
        {
            Items = items,
            TotalCount = totalCount,
            Page = req.Page,
            PageSize = req.PageSize,
        };

        return Result<PagedResult<OrderViewModel>>.Success(result);
    }

    public async Task<Result<OrderDetailsViewModel>> GetOrderDetails(int orderId)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Where(x => x.Id == orderId)
            .Select(x => new OrderDetailsViewModel
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                Items = x.Items.Select(i => new OrderItemViewModel
                    {
                        ProductName = i.Product!.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.SubTotal
                    }
                ).ToList()
            })
            .FirstOrDefaultAsync();
        
        if (order is null)
            return Result<OrderDetailsViewModel>.Failure("Order not found", ErrorCode.NotFound);

        var response = new OrderDetailsViewModel
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Items = order.Items
        };

        return Result<OrderDetailsViewModel>.Success(response);
    }

    public async Task<Result> CancelOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order is null)
            return Result.Failure("Order not found", ErrorCode.NotFound);
        // Check Status Cancelled
        if (order.Status == OrderStatus.Cancelled)
            return Result.Failure("Order already cancelled", ErrorCode.BadRequest);
        // Check Status Completed
        if (order.Status != OrderStatus.Completed)
            return Result.Failure("Only completed orders can be cancelled", ErrorCode.BadRequest);

        foreach (var item in order.Items)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

            if (product is null)
                return Result.Failure($"Product {item.ProductId} not found", ErrorCode.NotFound);

            // Increase Product Stock
            product.Stock += item.Quantity;

            // Insert Stock Movement
            _context.StockMovements.Add(new StockMovement
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                MovementType = StockMovementType.In,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Update Order Status
        order.Status = OrderStatus.Cancelled;

        await _context.SaveChangesAsync();
        return Result.Success();
    }
}