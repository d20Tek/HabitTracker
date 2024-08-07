//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;

namespace HabitTracker.Engine;

public interface IUserService
{
    Task<User> GetCurrentUser();

    Task<Result<User>> SetCurrentUserId(string userId);

    Task<Result<User[]>> GetUsers();

    Task<Result<User>> GetUser(string userId);

    Task<Result<User>> AddUser(string userId, string givenName, string familyName);

    Task<Result<User>> EditUser(string userId, string givenName, string familyName);

    Task<Result<User>> DeleteUser(string userId);
}
