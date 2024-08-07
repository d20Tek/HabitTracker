//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence;

namespace HabitTracker.Engine;

internal class CategoryService : ICategoryService
{
    private static Task<Result<Category>> _notFoundError(string categoryId) =>
        Task.FromResult<Result<Category>>(
            Error.NotFound("Category.NotFound", $"The category with id=[{categoryId}] was not found"));

    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Category[]>> GetUserCategories(string userId)
    {
        return await ServiceOperation.RunAsync<Category[]>(
            validate: () => CategoryInputValidator.ValidateUserId(userId),
            onSuccess: async () => await _repository.GetCategoriesFor(userId));
    }

    public async Task<Result<Category>> AddCategory(string userId, string categoryName)
    {
        return await ServiceOperation.RunAsync<Category>(
            validate: () => CategoryInputValidator.ValidateCreate(userId, categoryName),
            onSuccess: async () =>
            {
                var newCategory = new Category(string.Empty, userId, categoryName, EntityState.Active);
                return await _repository.CreateCategory(newCategory);
            });
    }

    public async Task<Result<Category>> RenameCategory(string userId, string categoryId, string newName)
    {
        return await ServiceOperation.RunAsync<Category>(
            validate: () => CategoryInputValidator.Validate(userId, categoryId, newName),
            onSuccess: async () =>
            {
                var category = await _repository.GetCategoryById(userId, categoryId);

                return await category.IfPresentOrElse<Task<Result<Category>>>(
                    async (c) =>
                    {
                        c.ChangeName(newName);
                        return await _repository.UpdateCategory(c);
                    },
                    () => _notFoundError(categoryId));
            });
    }

    public async Task<Result<Category>> DisableCategory(string userId, string categoryId)
    {
        return await ServiceOperation.RunAsync<Category>(
            validate: () => CategoryInputValidator.Validate(userId, categoryId),
            onSuccess: async () =>
            {
                var category = await _repository.GetCategoryById(userId, categoryId);

                return await category.IfPresentOrElse<Task<Result<Category>>>(
                    async (c) =>
                    {
                        if (c.State == EntityState.Inactive)
                        {
                            return Error.Invalid("Category.AlreadyDisabled", "This category is already disabled.");
                        }

                        c.Disable();
                        return await _repository.UpdateCategory(c);
                    },
                    () => _notFoundError(categoryId));
            });
    }

    public async Task<Result<Category>> EnableCategory(string userId, string categoryId)
    {
        return await ServiceOperation.RunAsync<Category>(
            validate: () => CategoryInputValidator.Validate(userId, categoryId),
            onSuccess: async () =>
            {
                var category = await _repository.GetCategoryById(userId, categoryId);

                return await category.IfPresentOrElse<Task<Result<Category>>>(
                    async (c) =>
                    {
                        if (c.State == EntityState.Active)
                        {
                            return Error.Invalid("Category.AlreadyEnabled", "This category is already enabled.");
                        }

                        c.Enable();
                        return await _repository.UpdateCategory(c);
                    },
                    () => _notFoundError(categoryId));
            });
    }
}
