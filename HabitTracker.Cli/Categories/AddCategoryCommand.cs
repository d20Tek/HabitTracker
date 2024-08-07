//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Categories;

internal class AddCategoryCommand : AsyncCommand<AddCategoryCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The new category's display name.")]
        public string Name { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public AddCategoryCommand(IAnsiConsole console, ICategoryService categoryService, IUserService userService)
    {
        _console = console;
        _categoryService = categoryService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            request.Name = _console.Ask<string>("Enter the new category's name:");
        }

        var user = await _userService.GetCurrentUser();
        var result = await _categoryService.AddCategory(user.UserId, request.Name);

        return result.IfOrElse(
            x => _console.RenderSuccess($"New category [{x.Name}] was created with id {x.CategoryId}."),
            errors => errors.Render(_console));
    }
}
