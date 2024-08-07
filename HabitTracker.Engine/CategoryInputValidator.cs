//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace HabitTracker.Engine;

internal static class CategoryInputValidator
{
    public static ValidationsResult ValidateUserId(string userId)
    {
        var result = new ValidationsResult();

        if (string.IsNullOrEmpty(userId))
        {
            result.AddValidationError("Category.UserId.Empty", "The specified user id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string categoryId)
    {
        var result = ValidateUserId(userId);

        if (string.IsNullOrEmpty(categoryId))
        {
            result.AddValidationError("Category.Id.Empty", "The specified category id is invalid.");
        }

        return result;
    }

    public static ValidationsResult ValidateCreate(string userId, string name)
    {
        var result = ValidateUserId(userId);

        if (string.IsNullOrEmpty(name))
        {
            result.AddValidationError("Category.Name.Empty", "The specified category name is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string categoryId, string catName)
    {
        var result = ValidateUserId(userId);

        if (string.IsNullOrEmpty(categoryId))
        {
            result.AddValidationError("Category.Id.Empty", "The specified category id is invalid.");
        }

        if (string.IsNullOrEmpty(catName))
        {
            result.AddValidationError("Category.Name.Empty", "The specified category name is invalid.");
        }

        return result;
    }
}
