//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Engine.Persistence;

internal class CategoryFileRepository : ICategoryRepository
{
    private readonly LowDbAsync<HabitStore> _db;

    public CategoryFileRepository(LowDbAsync<HabitStore> db)
    {
        _db = db;
    }

    public async Task<Category[]> GetCategoriesFor(string userId)
    {
        var store = await _db.Get();
        var forUser = store.Entities.FirstOrDefault(x => x.User.UserId == userId);

        return forUser?.Categories.Where(x => x.State == EntityState.Active).ToArray() ?? [];
    }

    public async Task<Optional<Category>> GetCategoryById(string userId, string categoryId)
    {
        var store = await _db.Get();
        var forUser = store.Entities.FirstOrDefault(x => x.User.UserId == userId);

        return forUser?.Categories.FirstOrDefault(x =>x.CategoryId == categoryId);
    }

    public async Task<Category> CreateCategory(Category category)
    {
        await _db.Update(x =>
        {
            var forUser = x.Entities.FirstOrDefault(x => x.User.UserId == category.UserId);
            if (forUser is not null)
            {
                var catId = forUser.GetNextCatId().ToString();
                category.SetId(catId);

                if (forUser.Categories.Any(x => x.CategoryId == catId))
                {
                    throw new InvalidOperationException($"Category with id {catId} already exists.");
                }

                forUser.Categories.Add(category);
            }
            else
            {
                throw new InvalidOperationException($"User with id {category.UserId} cannot be found. Make sure to use an existing user account.");
            }
        });

        return category;
    }

    public async Task<Category> UpdateCategory(Category updatedCategory)
    {
        await _db.Update(x =>
        {
            var forUser = x.Entities.FirstOrDefault(x => x.User.UserId == updatedCategory.UserId);

            var index = forUser?.Categories.FindIndex(x => x.CategoryId == updatedCategory.CategoryId) ?? -1;
            if (forUser is null || index < 0)
            {
                throw new InvalidOperationException($"Category with id {updatedCategory.CategoryId} not found.");
            }

            forUser.Categories[index] = updatedCategory;
        });

        return updatedCategory;
    }
}
