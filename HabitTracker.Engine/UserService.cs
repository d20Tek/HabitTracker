//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence;

namespace HabitTracker.Engine;

internal class UserService : IUserService
{
    private static Error _notFoundError(string userId) =>
        Error.NotFound("User.NotFound", $"The specified user [{userId}] could not be found in the data store.");

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetCurrentUser()
    {
        var user = await _userRepository.GetCurrentUser();
        return user.IfPresentOrElse<User>(u => u, () => User.Empty);
    }

    public async Task<Result<User>> SetCurrentUserId(string userId)
    {
        return await ServiceOperation.RunAsync<User>(
            validate: () => UserInputValidator.ValidateUserId(userId),
            onSuccess: async () => 
            {
                var user = await _userRepository.GetUserById(userId);
                return await user.IfPresentOrElse<Task<Result<User>>>(
                    async (u) =>
                    {
                        await _userRepository.SetCurrentUser(u);
                        return u;
                    },
                    () => Task.FromResult<Result<User>>(_notFoundError(userId)));
            });
    }

    public async Task<Result<User[]>> GetUsers()
    {
        return await ServiceOperation.RunAsync<User[]>(async () =>
        {
            return await _userRepository.GetAllUsers();
        });
    }

    public async Task<Result<User>> GetUser(string userId)
    {
        return await ServiceOperation.RunAsync<User>(
            validate: () => UserInputValidator.ValidateUserId(userId),
            onSuccess: async () =>
            {
                var user = await _userRepository.GetUserById(userId);
                return user.IfPresentOrElse<Result<User>>(
                    u => u,
                    () => _notFoundError(userId));
            });
    }

    public async Task<Result<User>> AddUser(string userId, string givenName, string familyName)
    {
        return await ServiceOperation.RunAsync<User>(
            validate: () => UserInputValidator.Validate(userId, givenName, familyName),
            onSuccess: async () =>
            {
                var newUser = new User(userId, givenName, familyName);

                var user = await _userRepository.CreateUser(newUser);
                var current = await _userRepository.GetCurrentUser();

                if ((await _userRepository.GetCurrentUser()).HasValue is false)
                {
                    await _userRepository.SetCurrentUser(user);
                }

                return user;
            });
    }

    public async Task<Result<User>> EditUser(string userId, string givenName, string familyName)
    {
        return await ServiceOperation.RunAsync<User>(
            validate: () => UserInputValidator.Validate(userId, givenName, familyName),
            onSuccess: async () =>
            {
                var user = await _userRepository.GetUserById(userId);

                return await user.IfPresentOrElse<Task<Result<User>>>(
                    async(u) =>
                    {
                        u.UpdateName(givenName, familyName);
                        return await _userRepository.UpdateUser(u);
                    },
                    () => Task.FromResult<Result<User>>(_notFoundError(userId)));
            });
    }

    public async Task<Result<User>> DeleteUser(string userId)
    {
        return await ServiceOperation.RunAsync<User>(
            validate: () => UserInputValidator.ValidateUserId(userId),
            onSuccess: async () =>
            {
                var user = await _userRepository.GetUserById(userId);

                return await user.IfPresentOrElse<Task<Result<User>>>(
                    async (u) => await _userRepository.DeleteUser(u.UserId),
                    () => Task.FromResult<Result<User>>(_notFoundError(userId)));
            });
    }
}
