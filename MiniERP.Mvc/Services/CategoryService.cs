using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface ICategoryService
{
    Task<Result> Create(CategoryCreateDto dto);
    Task<Result<PagedResult<CategoryViewModel>>> ListCategories(CategoryQuery req);
    Task<Result<CategoryViewModel>> GetCategory(int id);
    Task<Result> UpdateCategory(int id, CategoryUpdateDto dto);
    Task<Result> DeleteCategory(int id);
}

public class CategoryService(AppDbContext context) : ICategoryService
{
    private readonly AppDbContext _context = context;

    public async Task<Result> Create(CategoryCreateDto dto)
    {
        var exists = await _context.Categories.AnyAsync(x => x.Title == dto.Title);

        if (exists) return Result.Failure("Category already exists", ErrorCode.BadRequest);

        _context.Categories.Add(dto.ToEntity());
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Insert category failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    public async Task<Result<PagedResult<CategoryViewModel>>> ListCategories(CategoryQuery req)
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
            .Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            })
            .ToListAsync();

        var result = new PagedResult<CategoryViewModel>()
        {
            Items = items,
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<CategoryViewModel>>.Success(result);
    }

    public async Task<Result<CategoryViewModel>> GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return category is null
            ? Result<CategoryViewModel>.Failure("Category not found", ErrorCode.NotFound)
            : Result<CategoryViewModel>.Success(category.ToViewModel());
    }

    public async Task<Result> UpdateCategory(int id, CategoryUpdateDto dto)
    {
        var result = await FindCategoryById(id);
        var category = result.Data;

        if (category is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (!string.IsNullOrEmpty(dto.Title))
        {
            var titleExists = await _context.Categories
                .AnyAsync(x => x.Title == dto.Title && x.Id != id);

            if (titleExists) return Result.Failure("Title already exists", ErrorCode.BadRequest);
        }

        // Update Category
        dto.ApplyUpdate(category);

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Update category failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    public async Task<Result> DeleteCategory(int id)
    {
        var result = await FindCategoryById(id);
        var category = result.Data;

        if (category is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Soft Delete Category
        category.IsDeleted = true;

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Delete category failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    private async Task<Result<Category>> FindCategoryById(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        return category is null
            ? Result<Category>.Failure("Category not found", ErrorCode.NotFound)
            : Result<Category>.Success(category);
    }
}