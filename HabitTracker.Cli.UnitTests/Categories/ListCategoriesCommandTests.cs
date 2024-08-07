//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Categories;

[TestClass]
public class ListCategoriesCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithCategoriesForUser_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateMemoryRepo();

        var command = new ListCategoriesCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("list-categories", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        var text = AnsiConsole.ExportText();
        text.Should().Contain("Name");
        text.Should().Contain("Cat1");
        text.Should().Contain("Cat2");
        text.Should().Contain("Cat3");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithNoCategories_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateEmptyMemoryRepo();

        var command = new ListCategoriesCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("list-categories", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Name", "No categories exist");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var catService = CategoryServiceFactory.CreateCategoryServiceWithErrors();

        var command = new ListCategoriesCommand(AnsiConsole.Console, catService, userService);
        var context = CommandContextFactory.Create("list-categories", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
