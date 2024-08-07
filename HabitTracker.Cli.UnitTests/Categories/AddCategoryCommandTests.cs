//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Categories;

[TestClass]
public class AddCategoryCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new AddCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("add-category", ["-n", "FooCat"]);
        var request = new AddCategoryCommand.Request { Name = "FooCat" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "FooCat");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("CatAdded");
        var command = new AddCategoryCommand(testConsole, catService, userService);
        var context = CommandContextFactory.Create("add-category", []);
        var request = new AddCategoryCommand.Request();

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().ContainAll("Success", "CatAdded");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateCategoryServiceWithErrors();

        var command = new AddCategoryCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("add-category", ["-n", "FooCat"]);
        var request = new AddCategoryCommand.Request { Name = "FooCat" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
