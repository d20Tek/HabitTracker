//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.LowDb;
using FluentAssertions;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.Persistence.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Engine.UnitTests;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void AddHabitsEngine_AddsServices()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddHabitsEngine();

        // assert
        services.Any(x => x.ServiceType == typeof(IUserRepository)).Should().BeTrue();
        services.Any(x => x.ServiceType == typeof(ICategoryService)).Should().BeTrue();
        services.Any(x => x.ServiceType == typeof(LowDbAsync<HabitStore>)).Should().BeTrue();
        services.Any(x => x.ServiceType == typeof(IHabitService)).Should().BeTrue();
    }
}
