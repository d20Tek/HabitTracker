//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli;
using HabitTracker.Cli.Common;
using HabitTracker.Cli.Configuration;
using Microsoft.Extensions.DependencyInjection;

// configure services here...
var services = new ServiceCollection()
                    .AddServices();

var registrar = new DependencyInjectionTypeRegistrar(services);

// Create the CommandApp with specified command type and type registrar.
var app = new CommandApp<InteractiveLoopCommand>(registrar);
services.AddSingleton<CommandApp<InteractiveLoopCommand>>(sp => app);

// Configure any commands in the application.
app.Configure(config => config.ConfigureCommands());

return await app.RunAsync(args);
