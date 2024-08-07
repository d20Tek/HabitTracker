//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Categories;

internal class DisableCategoryCommand : AsyncCommand<DisableCategoryCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<CATEGORY-ID>")]
        [Description("The id of the category to disable.")]
        public string CategoryId { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public DisableCategoryCommand(IAnsiConsole console, ICategoryService categoryService, IUserService userService)
    {
        _console = console;
        _categoryService = categoryService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _categoryService.DisableCategory(user.UserId, request.CategoryId);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The category with id=[{x.CategoryId}] was disabled."),
            errors => errors.Render(_console));
    }
}
