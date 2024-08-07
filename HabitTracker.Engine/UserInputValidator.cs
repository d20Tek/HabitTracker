//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace HabitTracker.Engine;

internal static class UserInputValidator
{
    public static ValidationsResult ValidateUserId(string userId)
    {
        var result = new ValidationsResult();
        if (string.IsNullOrEmpty(userId))
        {
            result.AddValidationError("UserId.Empty", "The specified user id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string givenName, string familyName)
    {
        var result = new ValidationsResult();
        if (string.IsNullOrEmpty(userId))
        {
            result.AddValidationError("User.UserId.Empty", "The specified user id is invalid.");
        }

        if (string.IsNullOrEmpty(givenName))
        {
            result.AddValidationError("User.GivenName.Empty", "The specified user given name is invalid.");
        }

        if (string.IsNullOrEmpty(familyName))
        {
            result.AddValidationError("User.FamilyName.Empty", "The specified user family name is invalid.");
        }

        return result;
    }
}
