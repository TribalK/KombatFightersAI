
using KombatFightersAI.Domain.Commands;

namespace KombatFightersAI.Domain.DTO
{
    public class GameState
    {
        public required string[] PlayerKeys { get; init; }
        public required Dictionary<string, PlayerState> Players { get; set; }
        public bool RandomPriorityOccurrence { get; set; }

        public GameState DeepCopy()
        {
            return new GameState
            {
                PlayerKeys = (string[])PlayerKeys.Clone(),
                Players = Players.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value.DeepCopy()
                )
            };
        }
    }
}
