//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.LowDb.Adapters;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Cli.UnitTests.Fakes;

internal static class HabitServiceFactory
{
    public static IHabitService CreateMemoryRepo()
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
        var habitRepo = new HabitFileRepository(db);
        var catRepo = new CategoryFileRepository(db);

        return new HabitService(habitRepo, catRepo);
    }

    public static IHabitService CreateMemoryRepoWithDisabledHabits()
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
        var habitRepo = new HabitFileRepository(db);
        var catRepo = new CategoryFileRepository(db);

        return new HabitService(habitRepo, catRepo);
    }

    public static IHabitService CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var habitRepo = new HabitFileRepository(db);
        var catRepo = new CategoryFileRepository(db);

        return new HabitService(habitRepo, catRepo);
    }

    public static IHabitService CreateHabitServiceWithErrors()
    {
        var habitService = new Mock<IHabitService>();

        habitService.Setup(x => x.GetHabit(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(Result<Habit>.Success(
                       new Habit("test-id", "test-user-id", "Habit name", new Category("2", "test-user-id", "Cat2", EntityState.Active), 1)));

        habitService.Setup(x => x.AddHabit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        habitService.Setup(x => x.EditHabit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        habitService.Setup(x => x.GetUserHabits(It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        return habitService.Object;
    }
}
