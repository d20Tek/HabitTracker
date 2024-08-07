//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace HabitTracker.Engine;

public static class ServiceOperation
{
    public static Result<T> Run<T>(Func<Result<T>> operation)
    {
        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<T>> RunAsync<T>(Func<Task<Result<T>>> operation)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T> Run<T>(Func<ValidationsResult> validate, Func<Result<T>> onSuccess)
        where T : class
    {
        try
        {
            var validations = validate();
            if (validations.IsValid is false)
            {
                return validations.ToResult<T>();
            }

            return onSuccess();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<T>> RunAsync<T>(Func<ValidationsResult> validate, Func<Task<Result<T>>> onSuccess)
        where T : class
    {
        try
        {
            var validations = validate();
            if (validations.IsValid is false)
            {
                return validations.ToResult<T>();
            }

            return await onSuccess();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
