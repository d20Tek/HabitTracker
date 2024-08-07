//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Categories;

internal class ListCategoriesCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public ListCategoriesCommand(IAnsiConsole console, ICategoryService categoryService, IUserService userService)
    {
        _console = console;
        _categoryService = categoryService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _categoryService.GetUserCategories(user.UserId);

        return result.IfOrElse(RenderCategories, errors => errors.Render(_console));
    }

    private int RenderCategories(Category[] categories)
    {
        _console.MarkupLine("List of Habit Categories");

        var table = new Table()
            .Border(TableBorder.Rounded);

        table.AddColumns(
            new TableColumn("Id").Centered().Width(5),
            new TableColumn("Name").Width(50));

        if (categories.Any() is false)
        {
            table.AddRow("", "No categories exist... please add some.");
        }
        else
        {
            foreach (var category in categories)
            {
                table.AddRow(category.CategoryId, category.Name);
            }
        }

        _console.Write(table);
        return Globals.S_OK;
    }
}
