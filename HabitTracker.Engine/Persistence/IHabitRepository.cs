//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine.Persistence;

public interface IHabitRepository
{
    Task<Habit[]> GetHabitsFor(string userId);

    Task<Optional<Habit>> GetHabitById(string userId, string habitId);

    Task<Habit> CreateHabit(Habit habit);

    Task<Habit> UpdateHabit(Habit habit);
}
