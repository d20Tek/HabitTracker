//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentAssertions;
using HabitTracker.Engine.UnitTests.Fakes;

namespace HabitTracker.Engine.UnitTests;

[TestClass]
public class UserServiceTests
{
    [TestMethod]
    public async Task GetUsers_ReturnsUserList()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetUsers();

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(3);
        result.Value.Any(x => x.UserId == "test-user-2");
    }

    [TestMethod]
    public async Task GetUser_WithExistingId_ReturnsUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetUser("test-user-2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be("test-user-2");
    }

    [TestMethod]
    public async Task GetUser_WithMissingId_ReturnsError()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetUser("test-user-foo");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "User.NotFound").Should().BeTrue();
    }

    [TestMethod]
    public async Task GetUser_WithEmptyId_ReturnsError()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetUser("");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "UserId.Empty").Should().BeTrue();
    }

    [TestMethod]
    public async Task GetCurrentUser_WithValidId_ReturnsUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetCurrentUser();

        // assert
        result.IsEmpty().Should().BeFalse();
        result.UserId.Should().Be("test-user-1");
    }

    [TestMethod]
    public async Task GetCurrentUser_WithoutId_ReturnsEmptyUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.GetCurrentUser();

        // assert
        result.IsEmpty().Should().BeTrue();
    }

    [TestMethod]
    public async Task SetCurrentUserId_WithValidId_ReturnsEmptyUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.SetCurrentUserId("test-user-2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be("test-user-2");
    }

    [TestMethod]
    public async Task SetCurrentUserId_WithMissingId_ReturnsError()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.SetCurrentUserId("test-user-foo");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "User.NotFound").Should().BeTrue();
    }

    [TestMethod]
    public async Task SetCurrentUserId_WithEmptyId_ReturnsError()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.SetCurrentUserId("");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "UserId.Empty").Should().BeTrue();
    }

    [TestMethod]
    public async Task AddUser_WithValidInput_AddsAndReturnsUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.AddUser("test-user-new", "New", "Foo");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be("test-user-new");
        var addedUser = await service.GetUser("test-user-new");
        addedUser.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "New", "Foo", "User.UserId.Empty")]
    [DataRow("test-user-new", "", "Foo", "User.GivenName.Empty")]
    [DataRow("test-user-new", "New", "", "User.FamilyName.Empty")]
    [DataRow("test-user-2", "New2", "Foo2", "General.Exception")]
    public async Task AddUser_WithFailedValidation_ReturnsError(string id, string given, string family, string expectedErrorCode)
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.AddUser(id, given, family);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task EditUser_WithValidInput_UpdatesAndReturnsUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.EditUser("test-user-1", "New2", "Foo2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.GivenName.Should().Be("New2");
        result.Value.FamilyName.Should().Be("Foo2");
    }

    [TestMethod]
    [DataRow("", "New", "Foo", "User.UserId.Empty")]
    [DataRow("test-user-new", "", "Foo", "User.GivenName.Empty")]
    [DataRow("test-user-new", "New", "", "User.FamilyName.Empty")]
    [DataRow("test-user-2", "New", "Foo", "User.NotFound")]
    public async Task EditUser_WithFailedValidation_ReturnsError(string id, string given, string family, string expectedErrorCode)
    {
        // arrange
        var repo = UserRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new UserService(repo);

        // act
        var result = await service.EditUser(id, given, family);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task DeleteUser_WithValidId_DeletesAndReturnsUser()
    {
        // arrange
        var repo = UserRepositoryFactory.CreateMemoryRepo();
        IUserService service = new UserService(repo);

        // act
        var result = await service.DeleteUser("test-user-1");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be("test-user-1");
        var removedUser = await service.GetUser("test-user-1");
        removedUser.IsFailure.Should().BeTrue();
        removedUser.Errors.Any(x => x.Code == "User.NotFound").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "UserId.Empty")]
    [DataRow("test-user-missing", "User.NotFound")]
    public async Task DeleteUser_WithFailedValidation_ReturnsError(string id, string expectedErrorCode)
    {
        // arrange
        var repo = UserRepositoryFactory.CreateEmptyMemoryRepo();
        IUserService service = new UserService(repo);

        // act
        var result = await service.DeleteUser(id);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }
}