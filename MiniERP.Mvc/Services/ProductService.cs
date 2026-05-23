using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IProductService
{
    Task<Result<PagedResult<ProductDto>>> ListProducts(ProductQuery request);
    Task<Result<ProductDto>> CreateProduct(ProductCreateRequest request);
    Task<Result<ProductDto>> GetProduct(int id);
    Task<Result<List<ProductDto>>> GetProductsLowStock();
    Task<Result> UpdateProduct(int id, ProductUpdateRequest request);
    Task<Result> DeleteProduct(int id);

    // ----------- Stock Movement ---------------
    Task<Result> IncreaseStock(int productId, ProductStockQuantityRequest request);
    Task<Result> DecreaseStock(int productId, ProductStockQuantityRequest request);
    Task<Result> AdjustStock(int productId, ProductStockAdjustRequest request);
}

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<PagedResult<ProductDto>>> ListProducts(ProductQuery request)
    {
        var query = _context.Products.AsQueryable();

        // Search name
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name));
        }

        // Filter category id
        if (request.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == request.CategoryId.Value);
        }

        // Total count
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .Paginate(request.Page, request.PageSize)
            .Select(x => new ProductDto
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
        var result = new PagedResult<ProductDto>()
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Items = items
        };

        return Result<PagedResult<ProductDto>>.Success(result);
    }

    public async Task<Result<ProductDto>> CreateProduct(ProductCreateRequest request)
    {
        var exists = await _context.Products.AnyAsync(x => x.Name == request.Name);

        if (exists) return Result<ProductDto>.Failure("Name already exists", ErrorCode.BadRequest);

        var product = request.ToEntity();
        // Add Product
        _context.Add(product);
        await _context.SaveChangesAsync();
        
        return Result<ProductDto>.Success(product.ToProductDto());
    }

    public async Task<Result<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        return product is null
            ? Result<ProductDto>.Failure("Product not found", ErrorCode.NotFound)
            : Result<ProductDto>.Success(product.ToProductDto());
    }

    public async Task<Result<List<ProductDto>>> GetProductsLowStock()
    {
        var products = await _context.Products
            .AsNoTracking().Where(x => x.Stock <= x.MinimumStock)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Sku = x.Sku,
                Stock = x.Stock,
                MinimumStock = x.MinimumStock,
                Price = x.Price,
            })
            .ToListAsync();

        return Result<List<ProductDto>>.Success(products);
    }

    public async Task<Result> UpdateProduct(int id, ProductUpdateRequest request)
    {
        var result = await FindProductById(id);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var nameExists = await _context.Products
                .AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (nameExists) return Result.Failure("Name already exists", ErrorCode.BadRequest);
        }

        // Update Product
        request.ApplyUpdate(product);
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

    public async Task<Result> IncreaseStock(int productId, ProductStockQuantityRequest request)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (request.Quantity <= 0) return Result.Failure("Increase stock greater than zero", ErrorCode.BadRequest);

        // Increase Stock
        product.Stock += request.Quantity;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = request.Quantity,
            MovementType = StockMovementType.In
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DecreaseStock(int productId, ProductStockQuantityRequest request)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Check Stock
        if (product.Stock < request.Quantity) return Result.Failure("Stock not Enough", ErrorCode.BadRequest);

        // Decrease Stock
        product.Stock -= request.Quantity;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = -request.Quantity,
            MovementType = StockMovementType.Out
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AdjustStock(int productId, ProductStockAdjustRequest request)
    {
        var result = await FindProductById(productId);
        var product = result.Data;

        if (product is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        var diff = request.NewStock - product.Stock;

        // Update Stock
        product.Stock = request.NewStock;

        // Add Stock Movement
        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = diff,
            MovementType = StockMovementType.Adjust,
            Remark = request.Remark
        });

        await _context.SaveChangesAsync();
        return Result.Success();
    }
}