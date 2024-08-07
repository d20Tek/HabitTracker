//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace HabitTracker.Engine.UnitTests;

[TestClass]
public class ServiceOperationTests
{
    [TestMethod]
    public void Run_WithSuccessfulOperation_ReturnsResult()
    {
        // arrange

        // act
        var result = ServiceOperation.Run<string>(
            () => "success");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("success");
    }

    [TestMethod]
    public void Run_WithException_ReturnsError()
    {
        // arrange

        // act
        var result = ServiceOperation.Run<string>(
            () => throw new InvalidOperationException());

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "General.Exception").Should().BeTrue();
    }

    [TestMethod]
    public void Run_ValidationWithSuccessfulOperation_ReturnsResult()
    {
        // arrange

        // act
        var result = ServiceOperation.Run<string>(
            validate: () => new ValidationsResult(),
            onSuccess: () => "success");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("success");
    }

    [TestMethod]
    public void Run_WithValidationFailures_ReturnsError()
    {
        // arrange

        // act
        var result = ServiceOperation.Run<string>(
            validate: () =>
            {
                var result = new ValidationsResult();
                result.AddValidationError("Validation.Error", "Expected error condition.");
                return result;
            },
            onSuccess: [ExcludeFromCodeCoverage]() => throw new NotImplementedException());

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "Validation.Error").Should().BeTrue();
    }

    [TestMethod]
    public void Run_ValidationWithException_ReturnsError()
    {
        // arrange

        // act
        var result = ServiceOperation.Run<string>(
            validate: () => new ValidationsResult(),
            onSuccess: () => throw new InvalidOperationException());

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "General.Exception").Should().BeTrue();
    }

    [TestMethod]
    public async Task RunAsync_WithException_ReturnsError()
    {
        // arrange

        // act
        var result = await ServiceOperation.RunAsync<string>(
            () => throw new InvalidOperationException());

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Any(x => x.Code == "General.Exception").Should().BeTrue();
    }
}
