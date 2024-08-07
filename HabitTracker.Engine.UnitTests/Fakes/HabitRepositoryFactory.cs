//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb.Adapters;
using D20Tek.LowDb;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence.DTOs;
using HabitTracker.Engine.Persistence;

namespace HabitTracker.Engine.UnitTests.Fakes;

internal static class HabitRepositoryFactory
{
    public static IHabitRepository CreateMemoryRepo()
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
                    5,
                    [
                        new Habit("1", "test-user-1", "Habit1", new Category("2", "test-user-1", "Cat2", EntityState.Active), 1),
                        new Habit("2", "test-user-1", "Habit2", new Category("1", "test-user-1", "Cat1", EntityState.Active), 1),
                        new Habit("3", "test-user-1", "Habit3", new Category("2", "test-user-1", "Cat2", EntityState.Active), 3),
                        new Habit("4", "test-user-1", "Habit4", new Category("1", "test-user-1", "Cat1", EntityState.Active), 1),
                        new Habit("5", "test-user-1", "Habit5", new Category("2", "test-user-1", "Cat2", EntityState.Active), 1)
                    ])
            ]
        };

        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>(), testStore);
        var repo = new HabitFileRepository(db);

        return repo;
    }

    public static IHabitRepository CreateMemoryRepoWithDisabledHabits()
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
                    5,
                    [
                        new Habit("1", "test-user-1", "Habit1", new Category("2", "test-user-1", "Cat2", EntityState.Active), 1),
                        new Habit("2", "test-user-1", "Habit2", new Category("1", "test-user-1", "Cat1", EntityState.Active), 1),
                        new Habit("3", "test-user-1", "Habit3", new Category("2", "test-user-1", "Cat2", EntityState.Active), 1, EntityState.Inactive),
                        new Habit("4", "test-user-1", "Habit4", new Category("1", "test-user-1", "Cat1", EntityState.Active), 1, EntityState.Inactive),
                        new Habit("5", "test-user-1", "Habit5", new Category("2", "test-user-1", "Cat2", EntityState.Active), 1)
                    ])
            ]
        };

        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>(), testStore);
        var repo = new HabitFileRepository(db);

        return repo;
    }

    public static IHabitRepository CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var repo = new HabitFileRepository(db);

        return repo;
    }
}
