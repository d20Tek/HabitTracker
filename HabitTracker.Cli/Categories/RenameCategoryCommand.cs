//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Categories;

internal class RenameCategoryCommand : AsyncCommand<RenameCategoryCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<CATEGORY-ID>")]
        [Description("The id of the category to rename.")]
        public string CategoryId { get; set; } = "";

        [CommandOption("-n|--name")]
        [Description("The category's updated display name.")]
        public string Name { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public RenameCategoryCommand(IAnsiConsole console, ICategoryService categoryService, IUserService userService)
    {
        _console = console;
        _categoryService = categoryService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            request.Name = _console.Ask<string>("Enter the category's new name:");
        }

        var user = await _userService.GetCurrentUser();
        var result = await _categoryService.RenameCategory(user.UserId, request.CategoryId, request.Name);

        return result.IfOrElse(
            x => _console.RenderSuccess($"Category with id=[{x.CategoryId}] was renamed to [{x.Name}]."),
            errors => errors.Render(_console));
    }
}
