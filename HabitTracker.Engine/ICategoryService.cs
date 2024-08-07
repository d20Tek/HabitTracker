//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine;

public interface ICategoryService
{
    Task<Result<Category[]>> GetUserCategories(string userId);

    Task<Result<Category>> AddCategory(string userId, string categoryName);

    Task<Result<Category>> RenameCategory(string userId, string categoryId, string newName);

    Task<Result<Category>> EnableCategory(string userId, string categoryId);

    Task<Result<Category>> DisableCategory(string userId, string categoryId);
}
