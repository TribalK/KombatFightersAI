using KombatFightersAI.Domain.DTO;

namespace KombatFightersAI.Domain.Commands
{

    public class RunSimulationRequest
    {
        public int? NumberOfRounds { get; init; }
        public required PlayerStateRequest PlayerOneState { get; set; }
        public required PlayerStateRequest PlayerTwoState { get; set; }
    }

    public class PlayerStateRequest
    {
        public required string PlayerKey { get; set; }
        public required int CharacterId { get; set; }
        public int Health { get; set; }
        public int Energy { get; set; }
        public required Coordinates StagePosition { get; set; }
        public required List<string>? CardsUsed { get; set; }
    }
}
