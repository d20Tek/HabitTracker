//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Users;

internal static class CommandConfigurator
{
    public static IConfigurator ConfigureUserCommands(this IConfigurator config)
    {
        config.AddCommand<AddUserCommand>("add-user")
              .WithAlias("au")
              .WithDescription("Adds a new user accounts for this app.")
              .WithExample(["add-user", "--id", "test-user", "--given-name", "Tester", "--family-name", "User"])
              .WithExample(["au", "-i", "test-user", "-g", "Tester", "-f", "User"]);

        config.AddCommand<EditUserCommand>("edit-user")
              .WithAlias("eu")
              .WithDescription("Edits the user account information.")
              .WithExample(["edit-user", "test-user", "--given-name", "Tester", "--family-name", "User"])
              .WithExample(["eu", "test-user", "-g", "Tester", "-f", "User"]);

        config.AddCommand<DeleteUserCommand>("delete-user")
              .WithAlias("du")
              .WithDescription("Deletes the specified user account information.")
              .WithExample(["delete-user", "test-user"])
              .WithExample(["eu", "test-user"]);

        config.AddCommand<ListUsersCommand>("list-users")
              .WithAlias("lu")
              .WithDescription("Displays the list of all user accounts in this app.")
              .WithExample(["list-users"])
              .WithExample(["lu"]);

        config.AddCommand<SetCurrentUserCommand>("set-current-user")
              .WithAlias("scu")
              .WithAlias("set-user")
              .WithAlias("su")
              .WithDescription("Sets the specified user as the current one performing operations.")
              .WithExample(["set-current-user", "test-user"])
              .WithExample(["scu", "test-user"])
              .WithExample(["set-user", "test-user"]);

        return config;
    }

}
