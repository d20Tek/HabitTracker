//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Engine;

public static class DependencyInjection
{
    public static IServiceCollection AddHabitsEngine(this IServiceCollection services)
    {
        services.AddDatabases();

        services.AddSingleton<IUserRepository, UserFileRepository>();
        services.AddSingleton<ICategoryRepository, CategoryFileRepository>();
        services.AddSingleton<IHabitRepository, HabitFileRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IHabitService, HabitService>();

        return services;
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services)
    {
        var appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HabitTracker";

        services.AddLowDbAsync<HabitStore>(b =>
            b.UseFileDatabase("user-habits.json")
             .WithFolder(appFolder));

        return services;
    }
}
