//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine.Persistence;

public interface ICategoryRepository
{
    Task<Category[]> GetCategoriesFor(string userId);

    Task<Optional<Category>> GetCategoryById(string userId, string categoryId);

    Task<Category> CreateCategory(Category category);

    Task<Category> UpdateCategory(Category category);
}
