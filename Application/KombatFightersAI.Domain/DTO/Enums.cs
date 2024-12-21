namespace KombatFightersAI.Domain.DTO
{
    public enum CardAction
    {
        Undefined = 0,
        Move = 1,
        Attack = 2,
        Regenerate = 3,
        Guard = 4,
    }

    public enum AiBehavior
    {
        // No targeted behavior
        Optimize = 0,

        // Movement-based behavior
        Pursuit = 1,
        Fencing = 2,
        Control = 3,

        // Attack-type behavior
        Multistrike = 4,
        Parry = 5,

        // Misc behavior
        Manipulation = 6,
    }
}
