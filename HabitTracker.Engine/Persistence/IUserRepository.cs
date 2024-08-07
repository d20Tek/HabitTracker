//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine.Persistence;

public interface IUserRepository
{
    Task<Optional<User>> GetCurrentUser();

    Task SetCurrentUser(User user);

    Task<User[]> GetAllUsers();

    Task<Optional<User>> GetUserById(string userId);

    Task<User> CreateUser(User user);

    Task<User> UpdateUser(User user);

    Task<User> DeleteUser(string userId);
}
