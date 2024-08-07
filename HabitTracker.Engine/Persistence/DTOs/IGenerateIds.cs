//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Engine.Persistence.DTOs;

public interface IGenerateIds
{
    public int LastId { get; }

    public int GetNextId();
}
