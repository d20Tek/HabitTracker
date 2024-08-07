//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class AddHabitCommand : AsyncCommand<AddHabitCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The new habit's display name.")]
        public string Name { get; set; } = "";

        [CommandOption("-c|--cat-id")]
        [Description("The new habit's category id.")]
        public string CatId { get; set; } = "";

        [CommandOption("-a|--target-attempts")]
        [Description("The new habit's target attempts per day (1-100).")]
        public int? TargetAttempts { get; set; }
    }

    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public AddHabitCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            request.Name = _console.Ask<string>("Enter the new habit's name:");
        }

        if (string.IsNullOrEmpty(request.CatId))
        {
            request.CatId = _console.Ask<string>("Enter its category id:");
        }

        if (request.TargetAttempts is null)
        {
            request.TargetAttempts = _console.Ask<int>("Enter target attempts per day (1-100):");
        }

        var user = await _userService.GetCurrentUser();
        var result = await _habitService.AddHabit(
            user.UserId,
            request.Name,
            request.CatId,
            request.TargetAttempts ?? 1);

        return result.IfOrElse(
            x => _console.RenderSuccess($"New habit [{x.Name}] was created with id {x.HabitId}."),
            errors => errors.Render(_console));
    }
}
