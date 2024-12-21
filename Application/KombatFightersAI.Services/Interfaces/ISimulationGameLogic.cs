using KombatFightersAI.Domain.Commands;
using KombatFightersAI.Domain.DTO;

namespace KombatFightersAI.Services.Interfaces
{
    public interface ISimulationGameLogic
    {
        /// <summary>
        /// Determine which of the two cards used have higher priority.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="p1Card"></param>
        /// <param name="p2Card"></param>
        /// <returns>The unique player identifier.</returns>
        public void SimulateSingleTurn(GameState gameState, Card p1Card, Card p2Card);
        public string DeterminePriorityOrder(GameState gameState, Card p1Card, Card p2Card);
    }
}
