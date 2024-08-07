//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Categories;

[TestClass]
public class EnableCategoryCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepoWithDisabledCategory();

        var command = new EnableCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("enable-category", ["2"]);
        var request = new EnableCategoryCommand.Request { CategoryId = "2" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "2", "enabled");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithDisabledCategory_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new EnableCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("enable-category", ["1"]);
        var request = new EnableCategoryCommand.Request { CategoryId = "1" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "already enabled");
    }
}
