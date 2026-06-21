using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Constants;
using MiniERP.Mvc.Common.CurrentUser;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Services;

public interface ICategoryService
{
    Task<Result> Create(CategoryCreateRequest request);
    Task<Result<PagedResult<CategoryDto>>> ListCategories(CategoryQuery request);
    Task<Result<CategoryDto>> GetCategory(int id);
    Task<Result> UpdateCategory(int id, CategoryUpdateRequest request);
    Task<Result> DeleteCategory(int id);
}

public class CategoryService(AppDbContext context, ICurrentUser currentUser) : ICategoryService
{
    private readonly AppDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result> Create(CategoryCreateRequest request)
    {
        if (!_currentUser.IsAdmin) return Result.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);
        
        var exists = await _context.Categories.AnyAsync(x => x.Title == request.Title);

        if (exists) return Result.Failure("Category already exists", ErrorCode.BadRequest);

        _context.Categories.Add(request.ToEntity());
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PagedResult<CategoryDto>>> ListCategories(CategoryQuery request)
    {
        var query = _context.Categories.AsNoTracking().AsQueryable();

        // Global Search: Id, Title, Description
        if (!string.IsNullOrEmpty(request.Search))
        {
            var isId = int.TryParse(request.Search, out var id);

            query = query.Where(x =>
                x.Title.Contains(request.Search) ||
                x.Description!.Contains(request.Search) ||
                (isId && x.Id == id));
        }

        // Count Items
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .Paginate(request.Page, request.PageSize)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description!,
            })
            .ToListAsync();

        var result = new PagedResult<CategoryDto>()
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
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
            : Result<CategoryDto>.Success(category.ToDto());
    }

    public async Task<Result> UpdateCategory(int id, CategoryUpdateRequest request)
    {
        if (!_currentUser.IsAdmin) return Result.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);

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

        return Result.Success();
    }

    public async Task<Result> DeleteCategory(int id)
    {
        if (!_currentUser.IsAdmin) return Result.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);

        var result = await FindCategoryById(id);
        var category = result.Data;

        if (category is null) return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        // Soft Delete Category
        category.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result<Category>> FindCategoryById(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        return category is null
            ? Result<Category>.Failure("Category not found", ErrorCode.NotFound)
            : Result<Category>.Success(category);
    }
}