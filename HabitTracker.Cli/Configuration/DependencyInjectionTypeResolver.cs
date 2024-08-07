//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Cli;

public class DependencyInjectionTypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public DependencyInjectionTypeResolver(IServiceProvider provider)
    {
        _provider = provider;
    }

    public object? Resolve(Type? type) => _provider.GetRequiredService(type!);
}
