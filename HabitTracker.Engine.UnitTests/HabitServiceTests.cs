//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentAssertions;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.Persistence;
using HabitTracker.Engine.UnitTests.Fakes;

namespace HabitTracker.Engine.UnitTests;

[TestClass]
public class HabitServiceTests
{
    private readonly ICategoryRepository _catRepo = CategoryRepositoryFactory.CreateMemoryRepo();

    [TestMethod]
    public async Task GetUserHabits_ReturnsList()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.GetUserHabits("test-user-1");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(5);
        result.Value.Any(x => x.HabitId == "3");
    }

    [TestMethod]
    public async Task GetUserHabits_WithDisabledHabit_ReturnsFilteredList()
    {
        // arrange
        var repo = HabitRepositoryFactory.CreateMemoryRepoWithDisabledHabits();
        var service = new HabitService(repo, _catRepo);

        // act
        var result = await service.GetUserHabits("test-user-1");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(3);
        result.Value.Any(x => x.HabitId == "3").Should().BeFalse();
        result.Value.Any(x => x.HabitId == "5").Should().BeTrue();
    }

    [TestMethod]
    public async Task GetUserHabits_WithDifferentUserId_ReturnsEmptyList()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.GetUserHabits("test-user-diff");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(0);
    }

    [TestMethod]
    public async Task GetHabit_WithExistingId_ReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        // act
        var result = await service.GetHabit("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.HabitId.Should().Be("2");
        result.Value.UserId.Should().Be("test-user-1");
        result.Value.DailyCompletions.Should().BeEmpty();
        result.Value.IsCompleted(date).Should().BeFalse();
        result.Value.GetCompletionCount(date).Should().Be(0);
    }

    [TestMethod]
    public async Task GetHabit_WithMissingId_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.GetHabit("test-user-1", "99");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "Habit.NotFound").Should().BeTrue();
    }

    [TestMethod]
    public async Task GetHabit_WithEmptyId_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.GetHabit("test-user-1", "");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "Habit.HabitId.Empty").Should().BeTrue();
    }

    [TestMethod]
    public async Task AddHabit_WithValidInput_AddsAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.AddHabit("test-user-1", "New Habit", "2", 3);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.HabitId.Should().Be("6");
        result.Value.UserId.Should().Be("test-user-1");
        result.Value.Name.Should().Be("New Habit");

        var addedHabit = await service.GetHabit("test-user-1", "6");
        addedHabit.IsSuccess.Should().BeTrue();
        addedHabit.Value.TargetAttempts.Should().Be(3);
    }

    [TestMethod]
    [DataRow("", "New habit", "2", 1, "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", "2", 2, "Habit.Name.Empty")]
    [DataRow("test-user-new", "New habit", "", 3, "Habit.CategoryId.Empty")]
    [DataRow("test-user-new", "New habit", "99", 4, "Habit.CategoryId.NotFound")]
    [DataRow("test-user-new", "New habit", "2", 0, "Habit.TargetAttempts.OutOfRange")]
    [DataRow("test-user-new", "New habit", "2", 110, "Habit.TargetAttempts.OutOfRange")]
    public async Task AddHabit_WithFailedValidation_ReturnsError(string userId, string name, string catId, int attempts, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.AddHabit(userId, name, catId, attempts);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task EditHabit_WithValidInput_AddsAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.EditHabit("test-user-1", "2", "Updated Habit", "2", 5);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.HabitId.Should().Be("2");
        result.Value.UserId.Should().Be("test-user-1");
        result.Value.Name.Should().Be("Updated Habit");

        var addedHabit = await service.GetHabit("test-user-1", "2");
        addedHabit.IsSuccess.Should().BeTrue();
        addedHabit.Value.TargetAttempts.Should().Be(5);
        addedHabit.Value.CategoryId.Should().Be("2");
    }

    [TestMethod]
    [DataRow("", "3", "changed habit", "2", 1, "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", "changed habit", "2", 1, "Habit.HabitId.Empty")]
    [DataRow("test-user-1", "999", "changed habit", "2", 1, "Habit.NotFound")]
    [DataRow("test-user-new", "3", "", "2", 2, "Habit.Name.Empty")]
    [DataRow("test-user-new", "3", "changed habit", "", 3, "Habit.CategoryId.Empty")]
    [DataRow("test-user-new", "3", "changed habit", "99", 4, "Habit.CategoryId.NotFound")]
    [DataRow("test-user-new", "3", "changed habit", "2", 0, "Habit.TargetAttempts.OutOfRange")]
    [DataRow("test-user-new", "3", "changed habit", "2", 110, "Habit.TargetAttempts.OutOfRange")]
    public async Task EditHabit_WithFailedValidation_ReturnsError(string userId, string habitId, string name, string catId, int attempts, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.EditHabit(userId, habitId, name, catId, attempts);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task DisableHabit_OnEnabledHabit_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.DisableHabit("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Habit2");
        result.Value.State.Should().Be(EntityState.Inactive);
    }

    [TestMethod]
    public async Task DisableHabit_OnDisabledHabit_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepoWithDisabledHabits();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.DisableHabit("test-user-1", "4");

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Habit.AlreadyDisabled").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", "Habit.HabitId.Empty")]
    [DataRow("test-user-new", "99", "Habit.NotFound")]
    public async Task DisableHabit_WithFailedValidations_ReturnsError(string userId, string habitId, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.DisableHabit(userId, habitId);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task EnableHabit_OnDisabledHabit_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepoWithDisabledHabits();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.EnableHabit("test-user-1", "3");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Habit3");
        result.Value.State.Should().Be(EntityState.Active);
    }

    [TestMethod]
    public async Task EnableHabit_OnEnableHabit_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.EnableHabit("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Habit.AlreadyEnabled").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", "Habit.HabitId.Empty")]
    [DataRow("test-user-new", "99", "Habit.NotFound")]
    public async Task EnableCategory_WithFailedValidation_ReturnsError(string userId, string habitId, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.EnableHabit(userId, habitId);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task MarkHabit_TodayWithSingleIncrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        // act
        var result = await service.MarkHabit("test-user-1", "3", date);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.GetCompletionCount(date).Should().Be(1);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task MarkHabit_TodayWithMultipleIncrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        // act
        var result = await service.MarkHabit("test-user-1", "3", date, 10);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.GetCompletionCount(date).Should().Be(10);
        result.Value.IsCompleted(date).Should().BeTrue();
    }

    [TestMethod]
    public async Task MarkHabit_TodayWithMultipleIncrements_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        // act
        _ = await service.MarkHabit("test-user-1", "3", date);
        var result = await service.MarkHabit("test-user-1", "3", date);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.GetCompletionCount(date).Should().Be(2);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task MarkHabit_InPastWithSingleIncrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now.AddDays(-7);

        // act
        var result = await service.MarkHabit("test-user-1", "3", date);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.DailyCompletions.First().Value.Should().Be(1);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task MarkHabit_InFutureWithSingleIncrement_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now.AddDays(1);

        // act
        var result = await service.MarkHabit("test-user-1", "3", date, 5);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Habit.Date.FutureInvalid").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", 1, "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", 1, "Habit.HabitId.Empty")]
    [DataRow("test-user-new", "99", 5, "Habit.NotFound")]
    [DataRow("test-user-new", "1", 0, "Habit.ChangeAmount.OutOfRange")]
    [DataRow("test-user-new", "1", 101, "Habit.ChangeAmount.OutOfRange")]
    public async Task MarkHabit_WithFailedValidation_ReturnsError(string userId, string habitId, int increment, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.MarkHabit(userId, habitId, DateTimeOffset.Now, increment);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task UnmarkHabit_TodayWithSingleDecrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        _ = await service.MarkHabit("test-user-1", "3", date, 2);

        // act
        var result = await service.UnmarkHabit("test-user-1", "3", date);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.DailyCompletions.First().Value.Should().Be(1);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task UnmarkHabit_TodayWithMultipleDecrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        _ = await service.MarkHabit("test-user-1", "3", date, 5);

        // act
        var result = await service.UnmarkHabit("test-user-1", "3", date, 3);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.DailyCompletions.First().Value.Should().Be(2);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task UnmarkHabit_WithHighDecrementAmount_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now;

        _ = await service.MarkHabit("test-user-1", "3", date, 5);

        // act
        var result = await service.UnmarkHabit("test-user-1", "3", date, 50);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.GetCompletionCount(date).Should().Be(0);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task UnmarkHabit_InPastWithSingleDecrement_UpdatesAndReturnsHabit()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now.AddDays(-7);

        // act
        var result = await service.UnmarkHabit("test-user-1", "3", date);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DailyCompletions.Count.Should().Be(1);
        result.Value.DailyCompletions.First().Value.Should().Be(0);
        result.Value.IsCompleted(date).Should().BeFalse();
    }

    [TestMethod]
    public async Task UnmarkHabit_InFutureWithSingleDecrement_ReturnsError()
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);
        var date = DateTimeOffset.Now.AddDays(1);

        // act
        var result = await service.UnmarkHabit("test-user-1", "3", date, 5);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Habit.Date.FutureInvalid").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", 1, "Habit.UserId.Empty")]
    [DataRow("test-user-new", "", 1, "Habit.HabitId.Empty")]
    [DataRow("test-user-new", "99", 5, "Habit.NotFound")]
    [DataRow("test-user-new", "1", 0, "Habit.ChangeAmount.OutOfRange")]
    [DataRow("test-user-new", "1", 101, "Habit.ChangeAmount.OutOfRange")]
    public async Task UnmarkHabit_WithFailedValidation_ReturnsError(string userId, string habitId, int increment, string expectedErrorCode)
    {
        // arrange
        var habitRepo = HabitRepositoryFactory.CreateMemoryRepo();
        var service = new HabitService(habitRepo, _catRepo);

        // act
        var result = await service.UnmarkHabit(userId, habitId, DateTimeOffset.Now, increment);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }
}
