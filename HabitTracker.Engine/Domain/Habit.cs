//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Text.Json.Serialization;

namespace HabitTracker.Engine.Domain;

public class Habit
{
    public string HabitId { get; private set; }

    public string UserId { get; private set; }

    public string Name { get; private set; }

    public string CategoryId { get; private set; }

    public int TargetAttempts { get; private set; }

    public EntityState State { get; private set; }

    public Dictionary<DateTimeOffset, int> DailyCompletions { get; private set; }

    public Habit(
        string habitId,
        string userId,
        string name,
        Category category,
        int targetAttempts,
        EntityState state = EntityState.Active,
        Dictionary<DateTimeOffset, int>? dailyCompletions = null)
        : this(habitId, userId, name, category.CategoryId, targetAttempts, state, dailyCompletions)
    {
    }

    [JsonConstructor]
    private Habit(
        string habitId,
        string userId,
        string name,
        string categoryId,
        int targetAttempts,
        EntityState state,
        Dictionary<DateTimeOffset, int>? dailyCompletions)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));
        ArgumentNullException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentNullException.ThrowIfNullOrEmpty(categoryId, nameof(categoryId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(targetAttempts, nameof(targetAttempts));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(targetAttempts, 100, nameof(targetAttempts));

        HabitId = habitId;
        UserId = userId;
        Name = name;
        CategoryId = categoryId;
        TargetAttempts = targetAttempts;
        State = state;
        DailyCompletions = dailyCompletions ?? [];
    }

    internal void SetId(string id)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(id, nameof(id));
        HabitId = id;
    }

    internal void ChangeName(string name)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name, nameof(name));
        Name = name;
    }

    internal void ChangeCategory(Category category)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(category.CategoryId, nameof(category.CategoryId));
        CategoryId = category.CategoryId;
    }

    internal void ChangeTargetAttempts(int targetAttempts)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(targetAttempts, nameof(targetAttempts));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(targetAttempts, 100, nameof(targetAttempts));

        TargetAttempts = targetAttempts;
    }

    internal void Enable() => State = EntityState.Active;

    internal void Disable() => State = EntityState.Inactive;

    internal void MarkCompleted(DateTimeOffset date, int increment)
    {
        if (DailyCompletions.ContainsKey(date.Date) is false)
        {
            DailyCompletions[date.Date] = increment;
        }
        else
        {
            DailyCompletions[date.Date] += increment;
        }
    }

    internal void UnmarkCompleted(DateTimeOffset date, int decrement)
    {
        if (DailyCompletions.ContainsKey(date.Date) is false || DailyCompletions[date.Date] <= 0)
        {
            DailyCompletions[date.Date] = 0;

        }
        else
        {
            if (decrement > DailyCompletions[date.Date])
            {
                DailyCompletions[date.Date] = 0;
            }
            else
            {
                DailyCompletions[date.Date] -= decrement;
            }
        }
    }

    public int GetCompletionCount(DateTimeOffset date)
    {
        if (DailyCompletions.ContainsKey(date.Date))
        {
            return DailyCompletions[date.Date];
        }

        return 0;
    }

    public bool IsCompleted(DateTimeOffset date)
    {
        return DailyCompletions.ContainsKey(date.Date) && DailyCompletions[date.Date] >= TargetAttempts;
    }
}
