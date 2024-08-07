//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Engine.Domain;
using System.Text.Json.Serialization;

namespace HabitTracker.Engine.Persistence.DTOs;

internal class UserHabits
{
    public User User { get; internal set; }

    public int LastCatId { get; set; } = 0;

    public List<Category> Categories { get; init; }

    public int LastHabitId { get; set; } = 0;

    public List<Habit> Habits { get; init; }

    [JsonConstructor]
    public UserHabits(User user, int lastCatId, List<Category> categories, int lastHabitId, List<Habit> habits)
    {
        User = user;
        LastCatId = lastCatId;
        Categories = categories;
        LastHabitId = lastHabitId;
        Habits = habits;
    }

    public UserHabits(User user)
        : this(user, 0, [], 0, [])
    {
    }

    internal int GetNextCatId() => ++LastCatId;

    internal int GetNextHabitId() => ++LastHabitId;
}
