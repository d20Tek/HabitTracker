//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Categories;

[TestClass]
public class RanameCategoryCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new RenameCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("rename-category", ["2", "-n", "FooUpdated"]);
        var request = new RenameCategoryCommand.Request { CategoryId = "2", Name = "FooUpdated" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "FooUpdated", "renamed");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("CatUpdated");
        var command = new RenameCategoryCommand(testConsole, catService, userService);
        var context = CommandContextFactory.Create("rename-category", []);
        var request = new RenameCategoryCommand.Request { CategoryId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().ContainAll("Success", "CatUpdated", "renamed");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithMissingCatId_WritesErrorMessage()
    {
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new RenameCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("rename-category", ["-n", "CatUpdated"]);
        var request = new RenameCategoryCommand.Request { CategoryId = "bogus-id", Name = "CatUpdated" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "bogus-id", "not found");
    }
}
