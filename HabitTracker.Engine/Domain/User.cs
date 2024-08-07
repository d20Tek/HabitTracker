//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Engine.Domain;

public class User
{
    public string UserId { get; private set; }

    public string GivenName { get; private set; }

    public string FamilyName { get; private set; }

    public User(string userId, string givenName, string familyName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));
        ArgumentNullException.ThrowIfNullOrEmpty(givenName, nameof(givenName));
        ArgumentNullException.ThrowIfNullOrEmpty(familyName, nameof(familyName));

        UserId = userId;
        GivenName = givenName;
        FamilyName = familyName;
    }

    private User()
    {
        UserId = "";
        FamilyName = ""; 
        GivenName = "";
    }

    public static User Empty => new();

    public bool IsEmpty() => string.IsNullOrEmpty(UserId);

    internal void UpdateName(string givenName, string familyName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(givenName, nameof(givenName));
        ArgumentNullException.ThrowIfNullOrEmpty(familyName, nameof(familyName));

        GivenName = givenName;
        FamilyName = familyName;
    }
}
