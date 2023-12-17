namespace LinqSharp.EFCore;

/// <summary>
/// Similar to EntityState.
/// </summary>
public enum AutoState
{
    //
    // Summary:
    //     The entity is being tracked by the context and exists in the database. It has
    //     been marked for deletion from the database.
    Deleted = 2,
    //
    // Summary:
    //     The entity is being tracked by the context and exists in the database. Some or
    //     all of its property values have been modified.
    Modified = 3,
    //
    // Summary:
    //     The entity is being tracked by the context but does not yet exist in the database.
    Added = 4,
}
