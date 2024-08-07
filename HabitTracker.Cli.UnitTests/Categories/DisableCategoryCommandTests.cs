//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Categories;

[TestClass]
public class DisableCategoryCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new DisableCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("disable-category", ["3"]);
        var request = new DisableCategoryCommand.Request { CategoryId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "3", "disabled");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithDisabledCategory_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepoWithDisabledCategory();

        var command = new DisableCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("disable-category", ["2"]);
        var request = new DisableCategoryCommand.Request { CategoryId = "2" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "already disabled");
    }
}
