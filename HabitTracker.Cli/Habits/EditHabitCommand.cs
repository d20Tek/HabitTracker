//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class EditHabitCommand : AsyncCommand<EditHabitCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<HABIT-ID>")]
        [Description("The id of the habit to edit.")]
        public string HabitId { get; set; } = "";

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

    public EditHabitCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var getResult = await _habitService.GetHabit(user.UserId, request.HabitId);

        return await getResult.IfOrElse(
            habit => PerformHabitEdit(request, habit),
            errors => Task.FromResult(errors.Render(_console)));
    }

    private async Task<int> PerformHabitEdit(Request request, Habit prevHabit)
    { 
        if (string.IsNullOrEmpty(request.Name))
        {
            request.Name = _console.Ask<string>($"Change habit's name [[{prevHabit.Name}]]:");
        }

        if (string.IsNullOrEmpty(request.CatId))
        {
            request.CatId = _console.Ask<string>($"Change its category id [[{prevHabit.CategoryId}]]:");
        }

        if (request.TargetAttempts is null)
        {
            request.TargetAttempts = _console.Ask<int>(
                $"Change daily target attempts (1-100) [[{prevHabit.TargetAttempts}]]:");
        }

        var user = await _userService.GetCurrentUser();
        var result = await _habitService.EditHabit(
            user.UserId,
            request.HabitId,
            request.Name,
            request.CatId,
            request.TargetAttempts ?? 1);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The habit [{x.Name}] was updated."),
            errors => errors.Render(_console));
    }
}
