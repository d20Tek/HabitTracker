//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Engine.Persistence.DTOs;

public interface IEntityStore<TEntity>
    where TEntity : class
{
    public string Version { get; }

    public List<TEntity> Entities { get; }
}
