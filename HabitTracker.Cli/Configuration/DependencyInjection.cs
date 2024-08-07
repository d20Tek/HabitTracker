//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Users;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Cli.Configuration;

internal static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<EnsureCurrentUserExistsCommand>();

        return services.AddHabitsEngine();
    }
}
