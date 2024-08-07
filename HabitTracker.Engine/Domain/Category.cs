//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Engine.Domain;

public class Category
{
    public string CategoryId { get; private set; }

    public string UserId { get; private set; }

    public string Name { get; private set; }

    public EntityState State { get; private set; }

    public Category(string categoryId, string userId, string name, EntityState state)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));
        ArgumentNullException.ThrowIfNullOrEmpty(name, nameof(name));

        CategoryId = categoryId;
        UserId = userId;
        Name = name;
        State = state;
    }

    internal void SetId(string id)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(id, nameof(id));
        CategoryId = id;
    }

    internal void Enable() => State = EntityState.Active;

    internal void Disable() => State = EntityState.Inactive;

    internal void ChangeName(string newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));
        Name = newName;
    }
}
