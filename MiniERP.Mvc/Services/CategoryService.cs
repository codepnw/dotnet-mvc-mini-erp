using Microsoft.EntityFrameworkCore;
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

public interface ICategoryService
{
    Task<Result> Create(CategoryCreateRequest request);
    Task<Result<PagedResult<CategoryDto>>> ListCategories(CategoryQuery request);
    Task<Result<CategoryDto>> GetCategory(int id);
    Task<Result> UpdateCategory(int id, CategoryUpdateRequest request);
    Task<Result> DeleteCategory(int id);
}

public class CategoryService(AppDbContext context) : ICategoryService
{
    private readonly AppDbContext _context = context;

    public async Task<Result> Create(CategoryCreateRequest request)
    {
        var exists = await _context.Categories.AnyAsync(x => x.Title == request.Title);

        if (exists) return Result.Failure("Category already exists", ErrorCode.BadRequest);

        _context.Categories.Add(request.ToEntity());
        await _context.SaveChangesAsync();
        
        return  Result.Success();
    }

    public async Task<Result<PagedResult<CategoryDto>>> ListCategories(CategoryQuery req)
    {
        var query = _context.Categories.AsNoTracking().AsQueryable();

        // Search Title
        if (!string.IsNullOrEmpty(req.Title))
        {
            query = query.Where(x => x.Title.Contains(req.Title));
        }

        // Count Items
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .Paginate(req.Page, req.PageSize)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            })
            .ToListAsync();

        var result = new PagedResult<CategoryDto>()
        {
            Items = items,
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<CategoryDto>>.Success(result);
    }

    public async Task<Result<CategoryDto>> GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return category is null
            ? Result<CategoryDto>.Failure("Category not found", ErrorCode.NotFound)
            : Result<CategoryDto>.Success(category.ToCategoryDto());
    }

    public async Task<Result> UpdateCategory(int id, CategoryUpdateRequest request)
    {
        var result = await FindCategoryById(id);
        var category = result.Data;

        if (category is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (!string.IsNullOrEmpty(request.Title))
        {
            var titleExists = await _context.Categories
                .AnyAsync(x => x.Title == request.Title && x.Id != id);

            if (titleExists) return Result.Failure("Title already exists", ErrorCode.BadRequest);
        }

        // Update Category
        request.ApplyUpdate(category);
        await _context.SaveChangesAsync();
        
        return  Result.Success();
    }

    public async Task<Result> DeleteCategory(int id)
    {
        var result = await FindCategoryById(id);
        var category = result.Data;

        if (category is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Soft Delete Category
        category.IsDeleted = true;
        await _context.SaveChangesAsync();
        
        return  Result.Success();
    }

    private async Task<Result<Category>> FindCategoryById(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        return category is null
            ? Result<Category>.Failure("Category not found", ErrorCode.NotFound)
            : Result<Category>.Success(category);
    }
}