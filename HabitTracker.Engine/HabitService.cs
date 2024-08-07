//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence;

namespace HabitTracker.Engine;

internal class HabitService : IHabitService
{
    private static Error _notFoundError(string habitId) =>
        Error.NotFound("Habit.NotFound", $"The specified habit [{habitId}] could not be found in the data store.");

    private readonly IHabitRepository _habitRepo;
    private readonly ICategoryRepository _categoryRepo;

    public HabitService(IHabitRepository habitRepo, ICategoryRepository categoryRepo)
    {
        _habitRepo = habitRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<Habit[]>> GetUserHabits(string userId)
    {
        return await ServiceOperation.RunAsync<Habit[]>(
            validate: () => HabitInputValidator.ValidateUserId(userId),
            onSuccess: async () => await _habitRepo.GetHabitsFor(userId));
    }

    public async Task<Result<Habit>> GetHabit(string userId, string habitId)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId),
            onSuccess: async () =>
            {
                var habit = await _habitRepo.GetHabitById(userId, habitId);
                return habit.IfPresentOrElse<Result<Habit>>(
                    h => h,
                    () => _notFoundError(habitId));
            });
    }

    public async Task<Result<Habit>> AddHabit(string userId, string name, string categoryId, int targetAttempts)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, name, categoryId, targetAttempts),
            onSuccess: async () =>
            {
                var category = await _categoryRepo.GetCategoryById(userId, categoryId);
                if (category.HasValue is false)
                {
                    return Error.Validation("Habit.CategoryId.NotFound", "The specified category id was not found in our system.");
                }

                var newHabit = new Habit(string.Empty, userId, name, category.Value, targetAttempts);
                return await _habitRepo.CreateHabit(newHabit);
            });
    }

    public async Task<Result<Habit>> EditHabit(string userId, string habitId, string name, string categoryId, int targetAttempts)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId, name, categoryId, targetAttempts),
            onSuccess: async () =>
            {
                var category = await _categoryRepo.GetCategoryById(userId, categoryId);
                if (category.HasValue is false)
                {
                    return Error.Validation("Habit.CategoryId.NotFound", "The specified category id was not found in our system.");
                }

                var habit = await _habitRepo.GetHabitById(userId, habitId);

                return await habit.IfPresentOrElse<Task<Result<Habit>>>(
                    async (h) =>
                    {
                        h.ChangeName(name);
                        h.ChangeCategory(category.Value);
                        h.ChangeTargetAttempts(targetAttempts);

                        return await _habitRepo.UpdateHabit(h);
                    },
                    () => Task.FromResult<Result<Habit>>(_notFoundError(habitId)));
            });
    }

    public async Task<Result<Habit>> DisableHabit(string userId, string habitId)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId),
            onSuccess: async () =>
            {
                var habit = await _habitRepo.GetHabitById(userId, habitId);

                return await habit.IfPresentOrElse<Task<Result<Habit>>>(
                    async (h) =>
                    {
                        if (h.State == EntityState.Inactive)
                        {
                            return Error.Invalid("Habit.AlreadyDisabled", "This habit is already disabled.");
                        }

                        h.Disable();
                        return await _habitRepo.UpdateHabit(h);
                    },
                    () => Task.FromResult<Result<Habit>>(_notFoundError(habitId)));
            });
    }

    public async Task<Result<Habit>> EnableHabit(string userId, string habitId)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId),
            onSuccess: async () =>
            {
                var habit = await _habitRepo.GetHabitById(userId, habitId);

                return await habit.IfPresentOrElse<Task<Result<Habit>>>(
                    async (h) =>
                    {
                        if (h.State == EntityState.Active)
                        {
                            return Error.Invalid("Habit.AlreadyEnabled", "This habit is already enabled.");
                        }

                        h.Enable();
                        return await _habitRepo.UpdateHabit(h);
                    },
                    () => Task.FromResult<Result<Habit>>(_notFoundError(habitId)));
            });
    }

    public async Task<Result<Habit>> MarkHabit(string userId, string habitId, DateTimeOffset date, int increment = 1)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId, date, increment),
            async () =>
            {
                var habit = await _habitRepo.GetHabitById(userId, habitId);

                return await habit.IfPresentOrElse<Task<Result<Habit>>>(
                    async (h) =>
                    {
                        h.MarkCompleted(date, increment);
                        return await _habitRepo.UpdateHabit(h);
                    },
                    () => Task.FromResult<Result<Habit>>(_notFoundError(habitId)));
            });
    }

    public async Task<Result<Habit>> UnmarkHabit(string userId, string habitId, DateTimeOffset date, int decrement = 1)
    {
        return await ServiceOperation.RunAsync<Habit>(
            validate: () => HabitInputValidator.Validate(userId, habitId, date, decrement),
            onSuccess: async () =>
            {
                var habit = await _habitRepo.GetHabitById(userId, habitId);

                return await habit.IfPresentOrElse<Task<Result<Habit>>>(
                    async (h) =>
                    {
                        h.UnmarkCompleted(date, decrement);
                        return await _habitRepo.UpdateHabit(h);
                    },
                    () => Task.FromResult<Result<Habit>>(_notFoundError(habitId)));
            });
    }
}
