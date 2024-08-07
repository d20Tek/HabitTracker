//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace HabitTracker.Engine;

internal static class HabitInputValidator
{
    public static ValidationsResult ValidateUserId(string userId)
    {
        var result = new ValidationsResult();
        if (string.IsNullOrEmpty(userId))
        {
            result.AddValidationError("Habit.UserId.Empty", "The specified user id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string habitId)
    {
        var result = ValidateUserId(userId);

        if (string.IsNullOrEmpty(habitId))
        {
            result.AddValidationError("Habit.HabitId.Empty", "The specified habit id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string name, string categoryId, int targetAttempts)
    {
        var result = ValidateUserId(userId);

        if (string.IsNullOrEmpty(name))
        {
            result.AddValidationError("Habit.Name.Empty", "The specified habit name is invalid.");
        }

        if (targetAttempts <= 0 || targetAttempts > 100)
        {
            result.AddValidationError("Habit.TargetAttempts.OutOfRange", "The number of target attempts per day must be between 1-100.");
        }

        if (string.IsNullOrEmpty(categoryId))
        {
            result.AddValidationError("Habit.CategoryId.Empty", "The specified category id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string habitId, string name, string categoryId, int targetAttempts)
    {
        var result = Validate(userId, name, categoryId, targetAttempts);

        if (string.IsNullOrEmpty(habitId))
        {
            result.AddValidationError("Habit.HabitId.Empty", "The specified habit id is invalid.");
        }

        return result;
    }

    public static ValidationsResult Validate(string userId, string habitId, DateTimeOffset date, int changeCount)
    {
        var result = Validate(userId, habitId);

        if (date > DateTimeOffset.Now)
        {
            result.AddValidationError("Habit.Date.FutureInvalid", "The optional date parameter cannot be in the future.");
        }

        if (changeCount < 1 || changeCount > 100)
        {
            result.AddValidationError("Habit.ChangeAmount.OutOfRange", "The optional mark/unmark change amount is out of range (1-100).");
        }

        return result;
    }
}
