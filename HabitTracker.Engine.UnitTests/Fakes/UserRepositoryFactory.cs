//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.LowDb.Adapters;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Engine.UnitTests.Fakes;

internal static class UserRepositoryFactory
{
    public static IUserRepository CreateMemoryRepo()
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

        return repo;
    }

    public static IUserRepository CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var repo = new UserFileRepository(db);

        return repo;
    }
}
