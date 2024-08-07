//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb.Adapters;
using D20Tek.LowDb;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence.DTOs;
using HabitTracker.Engine.Persistence;

namespace HabitTracker.Engine.UnitTests.Fakes;

internal static class CategoryRepositoryFactory
{
    public static ICategoryRepository CreateMemoryRepo()
    {
        var testStore = new HabitStore
        {
            Version = "1.0",
            Entities =
            [
                new UserHabits(
                    new User("test-user-1", "Foo", "Tester"), 
                    3,
                    [
                        new ("1", "test-user-1", "Cat1", EntityState.Active),
                        new ("2", "test-user-1", "Cat2", EntityState.Active),
                        new ("3", "test-user-1", "Cat3", EntityState.Active),
                    ],
                    0,
                    [])
            ]
        };

        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>(), testStore);
        var repo = new CategoryFileRepository(db);

        return repo;
    }

    public static ICategoryRepository CreateMemoryRepoWithDisabledCategory()
    {
        var testStore = new HabitStore
        {
            Version = "1.0",
            Entities =
            [
                new UserHabits(
                    new User("test-user-1", "Foo", "Tester"),
                    3,
                    [
                        new ("1", "test-user-1", "Cat1", EntityState.Active),
                        new ("2", "test-user-1", "Cat2", EntityState.Inactive),
                        new ("3", "test-user-1", "Cat3", EntityState.Active),
                    ],
                    0,
                    [])
            ]
        };

        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>(), testStore);
        var repo = new CategoryFileRepository(db);

        return repo;
    }

    public static ICategoryRepository CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var repo = new CategoryFileRepository(db);

        return repo;
    }
}
