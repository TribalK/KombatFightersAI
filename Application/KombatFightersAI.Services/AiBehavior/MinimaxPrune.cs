using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Services.AiBehavior
{
    /// <summary>
    /// Minimax Algorithm with Alpha-Beta Pruning for each character's decision tree.
    /// </summary>
    public class MinimaxPrune
    {
        private readonly ISimulationGameLogic _simulator;
        private List<List<string>> viableCardCombosKeys;

        public MinimaxPrune(ISimulationGameLogic simulator) 
        {
            _simulator = simulator;
            viableCardCombosKeys = new List<List<string>>();
        }

        public async Task<List<List<string>>> BuildPlayerOptimalActionCardList(GameState gameState, string playerKey)
        {
            HeuristicMovePruning(gameState, playerKey, 1);

            return viableCardCombosKeys;
        }

        // TODO
        private void HeuristicMovePruning(GameState gameState, string playerKey, int depth)
        {
            // Validate players
            var selected = gameState.Players[playerKey];
            var opponent = gameState.Players.First(p => p.Key != playerKey).Value;

            if (selected.CardsUsed.Count == Globals.CardsUsedPerTurn)
            {
                viableCardCombosKeys.Add(selected.CardsUsed);
                return;
            }

            foreach (var slctMove in selected.GetAvailableMoves())
            {
                double totalEvaluationOutcome = 0.0;
                foreach (var opptMove in opponent.GetAvailableMoves())
                {
                    GameState gameStateClone = gameState.DeepCopy();

                    _simulator.SimulateSingleTurn(gameStateClone, slctMove, opptMove);

                    var selectedClone = gameStateClone.Players[playerKey];
                    var opponentClone = gameStateClone.Players.First(p => p.Key != playerKey).Value;

                    totalEvaluationOutcome += EvaluationLogicHelper.EvaluatePlayerStateAdvantage(selected, opponent, selectedClone, opponentClone, gameStateClone.RandomPriorityOccurrence, depth);
                }

                // Make recursive call here?
                Console.WriteLine($"Move: {slctMove.Name} Score: {totalEvaluationOutcome}");
            }

        }

    }
}   
