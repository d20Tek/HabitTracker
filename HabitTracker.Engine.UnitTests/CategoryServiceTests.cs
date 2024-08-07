//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentAssertions;
using HabitTracker.Engine.Domain;
using HabitTracker.Engine.UnitTests.Fakes;

namespace HabitTracker.Engine.UnitTests;

[TestClass]
public class CategoryServiceTests
{
    [TestMethod]
    public async Task GetUserCategories_ReturnsList()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.GetUserCategories("test-user-1");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(3);
        result.Value.Any(x => x.CategoryId == "2");
    }

    [TestMethod]
    public async Task GetUserCategories_WithDisabledCategory_ReturnsFilteredList()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepoWithDisabledCategory();
        var service = new CategoryService(repo);

        // act
        var result = await service.GetUserCategories("test-user-1");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(2);
        result.Value.Any(x => x.CategoryId == "2").Should().BeFalse();
        result.Value.Any(x => x.CategoryId == "1").Should().BeTrue();
    }

    [TestMethod]
    public async Task GetUserCategories_WithDifferentUserId_ReturnsEmptyList()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.GetUserCategories("test-user-diff");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(0);
    }

    [TestMethod]
    public async Task GetUserCategories_WithInvalidUserId_ReturnsError()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.GetUserCategories("");

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == "Category.UserId.Empty").Should().BeTrue();
    }

    [TestMethod]
    public async Task AddCategory_WithValidInput_AddsAndReturnsCategory()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.AddCategory("test-user-1", "NewCat");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CategoryId.Should().Be("4");
        result.Value.UserId.Should().Be("test-user-1");

        var addedCat = await service.GetUserCategories("test-user-1");
        addedCat.IsSuccess.Should().BeTrue();
        addedCat.Value.Any(x => x.CategoryId == "4").Should().BeTrue();
        addedCat.Value.Any(x => x.UserId == "test-user-1").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "New", "Category.UserId.Empty")]
    [DataRow("test-user-new", "", "Category.Name.Empty")]
    [DataRow("test-user-new", "NewCat", "General.Exception")]
    public async Task AddCategory_WithFailedValidation_ReturnsError(string userId, string name, string expectedErrorCode)
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.AddCategory(userId, name);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task RenameCategory_WithValidInput_UpdatesAndReturnsCategory()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.RenameCategory("test-user-1", "2", "Foo2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Foo2");
    }

    [TestMethod]
    [DataRow("", "1", "Foo", "Category.UserId.Empty")]
    [DataRow("test-user-new", "", "Foo", "Category.Id.Empty")]
    [DataRow("test-user-2", "2", "", "Category.Name.Empty")]
    [DataRow("test-user-2", "2", "Foo", "Category.NotFound")]
    public async Task RenameCategory_WithFailedValidation_ReturnsError(string userId, string catId, string name, string expectedErrorCode)
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.RenameCategory(userId, catId, name);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task DisableCategory_WithEnabledCat_UpdatesAndReturnsCategory()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.DisableCategory("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Cat2");
        result.Value.State.Should().Be(EntityState.Inactive);
    }

    [TestMethod]
    public async Task DisableCategory_WithDisabledCat_ReturnsError()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepoWithDisabledCategory();
        var service = new CategoryService(repo);

        // act
        var result = await service.DisableCategory("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Category.AlreadyDisabled").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", "Category.UserId.Empty")]
    [DataRow("test-user-new", "", "Category.Id.Empty")]
    [DataRow("test-user-new", "99", "Category.NotFound")]
    public async Task DisableCategory_WithFailedValidation_ReturnsError(string userId, string catId, string expectedErrorCode)
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.DisableCategory(userId, catId);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }

    [TestMethod]
    public async Task EnableCategory_WithDisabledCat_UpdatesAndReturnsCategory()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepoWithDisabledCategory();
        var service = new CategoryService(repo);

        // act
        var result = await service.EnableCategory("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Cat2");
        result.Value.State.Should().Be(EntityState.Active);
    }

    [TestMethod]
    public async Task EnableCategory_WithEbabledCat_ReturnsError()
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.EnableCategory("test-user-1", "2");

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Category.AlreadyEnabled").Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "4", "Category.UserId.Empty")]
    [DataRow("test-user-new", "", "Category.Id.Empty")]
    [DataRow("test-user-new", "99", "Category.NotFound")]
    public async Task EnableCategory_WithFailedValidation_ReturnsError(string userId, string catId, string expectedErrorCode)
    {
        // arrange
        var repo = CategoryRepositoryFactory.CreateEmptyMemoryRepo();
        var service = new CategoryService(repo);

        // act
        var result = await service.EnableCategory(userId, catId);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Any(x => x.Code == expectedErrorCode).Should().BeTrue();
    }
}
