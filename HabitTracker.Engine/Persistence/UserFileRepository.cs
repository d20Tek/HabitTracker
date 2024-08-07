//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Engine.Persistence;

internal class UserFileRepository : IUserRepository
{
    private readonly LowDbAsync<HabitStore> _db;

    public UserFileRepository(LowDbAsync<HabitStore> db)
    {
        _db = db;
    }

    public async Task<Optional<User>> GetCurrentUser()
    {
        var store = await _db.Get();
        return await GetUserById(store.CurrentUserId);
    }

    public async Task SetCurrentUser(User user)
    {
        await _db.Update(x =>
        {
            if (x.Entities.Any(x => x.User.UserId == user.UserId) is false)
            {
                throw new InvalidOperationException($"User with id ({user.UserId}) does not exist.");
            }

            x.CurrentUserId = user.UserId;
        });
    }

    public async Task<User[]> GetAllUsers()
    {
        var store = await _db.Get();
        return store.Entities.Select(x => x.User).ToArray();
    }

    public async Task<Optional<User>> GetUserById(string userId)
    {
        var store = await _db.Get();
        return store.Entities.FirstOrDefault(x => x.User.UserId == userId)?.User;
    }

    public async Task<User> CreateUser(User user)
    {
        await _db.Update(x =>
        {
            if (x.Entities.Any(x => x.User.UserId == user.UserId))
            {
                throw new InvalidOperationException($"User with id ({user.UserId}) already exists.");
            }

            x.Entities.Add(new UserHabits(user));
        });

        return user;
    }

    public async Task<User> UpdateUser(User updatedUser)
    {
        await _db.Update(x =>
        {
            var index = x.Entities.FindIndex(x => x.User.UserId == updatedUser.UserId);
            if (index < 0)
            {
                throw new InvalidOperationException($"User with id ({updatedUser.UserId}) not found.");
            }

            x.Entities[index].User = updatedUser;

        });

        return updatedUser;
    }

    public async Task<User> DeleteUser(string userId)
    {
        User? user = null;
        await _db.Update(x =>
        {
            user = x.Entities.FirstOrDefault(x => x.User.UserId == userId)?.User;
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({userId}) not found.");
            }

            x.Entities.RemoveAll(x => x.User.UserId == userId);
        });

        return user!;
    }
}
