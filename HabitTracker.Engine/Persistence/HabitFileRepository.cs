//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Engine.Persistence;

internal class HabitFileRepository : IHabitRepository
{
    private readonly LowDbAsync<HabitStore> _db;

    public HabitFileRepository(LowDbAsync<HabitStore> db)
    {
        _db = db;
    }

    public async Task<Habit[]> GetHabitsFor(string userId)
    {
        var store = await _db.Get();
        var forUser = store.Entities.FirstOrDefault(x => x.User.UserId == userId);

        return forUser?.Habits.Where(x => x.State == EntityState.Active).ToArray() ?? [];
    }

    public async Task<Optional<Habit>> GetHabitById(string userId, string habitId)
    {
        var store = await _db.Get();
        var forUser = store.Entities.FirstOrDefault(x => x.User.UserId == userId);

        return forUser?.Habits.FirstOrDefault(x => x.HabitId == habitId);
    }

    public async Task<Habit> CreateHabit(Habit habit)
    {
        await _db.Update(x =>
        {
            var forUser = x.Entities.FirstOrDefault(x => x.User.UserId == habit.UserId);
            if (forUser is not null)
            {
                var habitId = forUser.GetNextHabitId().ToString();
                habit.SetId(habitId);

                if (forUser.Habits.Any(x => x.HabitId == habitId))
                {
                    throw new InvalidOperationException($"Habit with id {habitId} already exists.");
                }

                forUser.Habits.Add(habit);
            }
            else
            {
                throw new InvalidOperationException($"User with id {habit.UserId} cannot be found. Make sure to use an existing user account.");
            }
        });

        return habit;
    }

    public async Task<Habit> UpdateHabit(Habit updatedHabit)
    {
        await _db.Update(x =>
        {
            var forUser = x.Entities.FirstOrDefault(x => x.User.UserId == updatedHabit.UserId);

            var index = forUser?.Habits.FindIndex(x => x.HabitId == updatedHabit.HabitId) ?? -1;
            if (forUser is null || index < 0)
            {
                throw new InvalidOperationException($"Habit with id {updatedHabit.HabitId} not found.");
            }

            forUser.Habits[index] = updatedHabit;
        });

        return updatedHabit;
    }
}
