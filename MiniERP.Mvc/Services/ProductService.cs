using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IProductService
{
    Task<Result<PagedResult<ProductViewModel>>> ListProducts(ProductQuery req);
    Task<Result<ProductViewModel>> CreateProduct(ProductCreateDto dto);
    Task<Result<ProductViewModel>> GetProduct(int id);
    Task<Result<List<ProductViewModel>>> GetProductsLowStock();
    Task<Result> UpdateProduct(int id, ProductUpdateDto dto);
    Task<Result> DeleteProduct(int id);

    // ----------- Stock Movement ---------------
    Task<Result> IncreaseStock(int productId, ProductStockQuantityDto dto);
    Task<Result> DecreaseStock(int productId, ProductStockQuantityDto dto);
    Task<Result> AdjustStock(int productId, ProductStockAdjustDto dto);
}

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<PagedResult<ProductViewModel>>> ListProducts(ProductQuery req)
    {
        var query = _context.Products.AsQueryable();

        // Search name
        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            query = query.Where(x => x.Name.Contains(req.Name));
        }

        // Filter category id
        if (req.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == req.CategoryId.Value);
        }

        // Total count
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .Paginate(req.Page, req.PageSize)
            .Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Sku = x.Sku,
                Price = x.Price,
                Stock = x.Stock,
                MinimumStock = x.MinimumStock,
                CategoryTitle = x.Category != null ? x.Category.Title : "N/A"
            })
            .ToListAsync();

        // Response Result
        var result = new PagedResult<ProductViewModel>()
        {
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = totalCount,
            Items = items
        };

        return Result<PagedResult<ProductViewModel>>.Success(result);
    }

    public async Task<Result<ProductViewModel>> CreateProduct(ProductCreateDto dto)
    {
        var exists = await _context.Products.AnyAsync(x => x.Name == dto.Name);

        if (exists) return Result<ProductViewModel>.Failure("Name already exists", ErrorCode.BadRequest);

        var product = dto.ToEntity();
        // Add Product
        _context.Add(product);

        await _context.SaveChangesAsync();
        return Result<ProductViewModel>.Success(product.ToViewModel());
    }

    public async Task<Result<ProductViewModel>> GetProduct(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        return product is null
            ? Result<ProductViewModel>.Failure("Product not found", ErrorCode.NotFound)
            : Result<ProductViewModel>.Success(product.ToViewModel());
    }

    public async Task<Result<List<ProductViewModel>>> GetProductsLowStock()
    {
        var products = await _context.Products
            .AsNoTracking().Where(x => x.Stock <= x.MinimumStock)
            .Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Sku = x.Sku,
                Stock = x.Stock,
                MinimumStock = x.MinimumStock,
                Price = x.Price,
            })
            .ToListAsync();

        return Result<List<ProductViewModel>>.Success(products);
    }

    public async Task<Result> UpdateProduct(int id, ProductUpdateDto dto)
    {
        var result = await FindProductById(id);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            var nameExists = await _context.Products
                .AnyAsync(x => x.Name == dto.Name && x.Id != id);

            if (nameExists) return Result.Failure("Name already exists", ErrorCode.BadRequest);
        }

        // Update Product
        dto.ApplyUpdate(product);

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProduct(int id)
    {
        var result = await FindProductById(id);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Soft Delete Product
        product.IsDeleted = true;

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<Result<Product>> FindProductById(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        return product is null
            ? Result<Product>.Failure("Product not found", ErrorCode.NotFound)
            : Result<Product>.Success(product);
    }

    // ------------------------- Stock Movement ---------------------------

    public async Task<Result> IncreaseStock(int productId, ProductStockQuantityDto dto)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (dto.Quantity <= 0) return Result.Failure("Increase stock greater than zero", ErrorCode.BadRequest);

        // Increase Stock
        product.Stock += dto.Quantity;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = dto.Quantity,
            MovementType = StockMovementType.In
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DecreaseStock(int productId, ProductStockQuantityDto dto)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Check Stock
        if (product.Stock < dto.Quantity) return Result.Failure("Stock not Enough", ErrorCode.BadRequest);

        // Decrease Stock
        product.Stock -= dto.Quantity;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = -dto.Quantity,
            MovementType = StockMovementType.Out
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AdjustStock(int productId, ProductStockAdjustDto dto)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        var diff = dto.NewStock - product.Stock;

        // Update Stock
        product.Stock = dto.NewStock;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = diff,
            MovementType = StockMovementType.Adjust,
            Remark = dto.Remark
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }
}