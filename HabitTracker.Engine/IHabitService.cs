//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine;

public interface IHabitService
{
    Task<Result<Habit[]>> GetUserHabits(string userId);

    Task<Result<Habit>> GetHabit(string userId, string habitId);

    Task<Result<Habit>> AddHabit(string userId, string name, string categoryId, int targetAttempts);

    Task<Result<Habit>> EditHabit(string userId, string habitId, string name, string categoryId, int targetAttempts);

    Task<Result<Habit>> EnableHabit(string userId, string habitId);

    Task<Result<Habit>> DisableHabit(string userId, string habitId);

    Task<Result<Habit>> MarkHabit(string userId, string habitId, DateTimeOffset date, int increment = 1);

    Task<Result<Habit>> UnmarkHabit(string userId, string habitId, DateTimeOffset date, int decrement = 1);
}
