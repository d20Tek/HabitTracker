//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using D20Tek.LowDb.Adapters;
using D20Tek.Minimal.Result;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;

namespace HabitTracker.Cli.UnitTests.Fakes;

internal static class CategoryServiceFactory
{
    public static ICategoryService CreateMemoryRepo()
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

        return new CategoryService(repo);
    }

    public static ICategoryService CreateMemoryRepoWithDisabledCategory()
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

        return new CategoryService(repo);
    }

    public static ICategoryService CreateEmptyMemoryRepo()
    {
        var db = new LowDbAsync<HabitStore>(new MemoryStorageAdapterAsync<HabitStore>());
        var repo = new CategoryFileRepository(db);

        return new CategoryService(repo);
    }

    public static ICategoryService CreateCategoryServiceWithErrors()
    {
        var catService = new Mock<ICategoryService>();

        catService.Setup(x => x.AddCategory(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        catService.Setup(x => x.RenameCategory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        catService.Setup(x => x.GetUserCategories(It.IsAny<string>()))
                   .ReturnsAsync(Error.Invalid("Test.Code", "test error message."));

        return catService.Object;
    }
}
