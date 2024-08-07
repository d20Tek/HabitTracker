//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Categories;

internal static class CommandConfigurator
{
    public static IConfigurator ConfigureCategoryCommands(this IConfigurator config)
    {
        config.AddCommand<AddCategoryCommand>("add-category")
              .WithAlias("ac")
              .WithDescription("Adds a habit category for the current user.")
              .WithExample(["add-category", "--name", "Category 1"])
              .WithExample(["ac", "-n", "Category 1"]);

        config.AddCommand<DisableCategoryCommand>("disable-category")
              .WithAlias("dc")
              .WithDescription("Disables the specified category so it no longer can be used.")
              .WithExample(["disable-category", "cat-id"])
              .WithExample(["dc", "cat-id"]);

        config.AddCommand<EnableCategoryCommand>("enable-category")
              .WithAlias("ec")
              .WithDescription("Enables the specified category so it can be used again.")
              .WithExample(["enable-category", "cat-id"])
              .WithExample(["ec", "cat-id"]);

        config.AddCommand<ListCategoriesCommand>("list-categories")
              .WithAlias("lc")
              .WithDescription("Displays the list of habit cateegories for the current user.")
              .WithExample(["list-categories"])
              .WithExample(["lc"]);

        config.AddCommand<RenameCategoryCommand>("rename-category")
              .WithAlias("rc")
              .WithDescription("Changes the category to the new name.")
              .WithExample(["rename-category", "cat-id", "--name", "Category Updated"])
              .WithExample(["rc", "cat-id", "-n", "Category Updated"]);

        return config;
    }
}
