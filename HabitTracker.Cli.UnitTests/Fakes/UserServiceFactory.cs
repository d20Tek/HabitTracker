//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.LowDb.Adapters;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Cli.UnitTests.Fakes;

internal static class UserServiceFactory
{
    public static IUserService CreateMemoryRepo()
    {
        var testStore = new HabitStore
        {
            Version = "1.0",
            CurrentUserId = "test-user-1",
            Entities =
            [
                new UserHabits(new User("test-user-1", "Foo", "Tester")),
                new UserHabits(new User("test-user-2", "Bar", "Tester")),
                new UserHabits(new User("test-user-3", "Waz", "Tester")),
            ]
        };

        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>(), testStore);
        var repo = new UserFileRepository(db);

        return new UserService(repo);
    }

    public static IUserService CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var repo = new UserFileRepository(db);

        return new UserService(repo);
    }

    public static IUserService CreateUserServiceWithErrors()
    {
        var userService = new Mock<IUserService>();

        userService.Setup(x => x.GetUser(It.IsAny<string>()))
                   .ReturnsAsync(Result<User>.Success(new User("test-id", "test", "test-last")));

        userService.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        userService.Setup(x => x.EditUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        userService.Setup(x => x.GetUsers())
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        return userService.Object;
    }
}
