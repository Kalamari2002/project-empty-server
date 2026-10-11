public enum GrabActionsEnums
{
    // When player misses a grab ray cast
    NOTHING,
    // When player hits a grab ray cast and the enemy is NOT attacking
    GRAB,
    // When player hits a grab ray cast and the enemy IS attacking
    COUNTER,
    // When player hits a grab ray cast on a pickable object. Today that's just weapons, but i might abstract it one layer up to generic objects
    PICKABLE
}
