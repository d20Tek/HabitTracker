//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Categories;

internal class EnableCategoryCommand : AsyncCommand<EnableCategoryCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<CATEGORY-ID>")]
        [Description("The id of the category to enable.")]
        public string CategoryId { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public EnableCategoryCommand(IAnsiConsole console, ICategoryService categoryService, IUserService userService)
    {
        _console = console;
        _categoryService = categoryService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _categoryService.EnableCategory(user.UserId, request.CategoryId);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The category with id=[{x.CategoryId}] was enabled."),
            errors => errors.Render(_console));
    }
}
