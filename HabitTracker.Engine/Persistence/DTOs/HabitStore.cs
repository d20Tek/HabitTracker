//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Engine.Persistence.DTOs;

internal class HabitStore : IEntityStore<UserHabits>
{
    public string Version { get; set; } = "1.0";

    public string CurrentUserId { get; set; } = string.Empty;

    public List<UserHabits> Entities { get; init; } = [];
}
