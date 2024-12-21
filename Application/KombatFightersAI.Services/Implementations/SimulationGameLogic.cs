using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Services.Implementations
{
    public class SimulationGameLogic: ISimulationGameLogic
    {
        /// <summary>
        /// Simulate a single turn between two players
        /// </summary>
        /// <param name="gameState"></param>
        public void SimulateSingleTurn(GameState gameState, Card playerOneAction, Card playerTwoAction)
        {
            // Determine priority order between two cards and execute, return updated stats
            var firstPriorityPlayerKey = DeterminePriorityOrder(gameState, playerOneAction, playerTwoAction);

            if (firstPriorityPlayerKey == gameState.PlayerKeys[0])
            {
                // Player One goes first
                UpdateGameStateWithCardAction(gameState, playerOneAction, gameState.PlayerKeys[0]);

                UpdateGameStateWithCardAction(gameState, playerTwoAction, gameState.PlayerKeys[1]);
            }
            else
            {
                // Player Two goes first
                UpdateGameStateWithCardAction(gameState, playerTwoAction, gameState.PlayerKeys[1]);

                UpdateGameStateWithCardAction(gameState, playerOneAction, gameState.PlayerKeys[0]);
            }

            ResetGuardStats(gameState);
        }

        /// <summary>
        /// Determine which of the two cards used have higher priority.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerOneAction"></param>
        /// <param name="playerTwoAction"></param>
        /// <returns>The unique player identifier.</returns>
        public string DeterminePriorityOrder(GameState gameState, Card playerOneAction, Card playerTwoAction)
        {
            // PRIORITY ORDER HIERARCHY RULES:
            // 1. Attack cards have less priority than other card actions, which are equal
            // 2. If both cards are attack, then higher damage has higher priority
            // 3. If both attacks deals the same damage, the character with the most health has higher priority
            string identifier = DeterminePriorityByActionType(gameState, playerOneAction, playerTwoAction);

            if (string.IsNullOrEmpty(identifier)) {
                // All tiebreakers occurred - Randomize priority randomly
                identifier = RandomizePlayerPriority(gameState);
            }

            return identifier;
        }

        private string DeterminePriorityByActionType(GameState gameState, Card playerOneAction, Card playerTwoAction)
        {
            if (playerOneAction.ActionType != CardAction.Attack && playerTwoAction.ActionType == CardAction.Attack)
                return gameState.PlayerKeys[0];

            else if (playerOneAction.ActionType == CardAction.Attack && playerTwoAction.ActionType != CardAction.Attack)
                return gameState.PlayerKeys[1];

            else return DeterminePriorityByDamage(gameState, playerOneAction, playerTwoAction);
        }
                    
        private string DeterminePriorityByDamage(GameState gameState, Card playerOneAction, Card playerTwoAction)
        {
            if (playerOneAction.Damage > playerTwoAction.Damage)
                return gameState.PlayerKeys[0];

            else if (playerTwoAction.Damage > playerOneAction.Damage)
                return gameState.PlayerKeys[1];

            else return DeterminePriorityByHealth(gameState);
        }

        private string DeterminePriorityByHealth(GameState gameState)
        {
            var largestHealth = gameState.Players.Values.Max(p => p.Health);
            var playerWithMoreHealth = gameState.Players.Where(p => p.Value.Health == largestHealth).ToList();

            return playerWithMoreHealth.Count > 1 ? string.Empty : playerWithMoreHealth[0].Key;
        }

        private string RandomizePlayerPriority(GameState gameState)
        {
            gameState.RandomPriorityOccurrence = true;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            return string.Join("", rnd.Next(0, 2) == 0 ? gameState.PlayerKeys[0] : gameState.PlayerKeys[1]);
        }

        private void UpdateGameStateWithCardAction(GameState gameState, Card action, string playerKey)
        {
            var currentPlayer = gameState.Players[playerKey];
            var opponentPlayer = gameState.Players.First(p => p.Key != playerKey).Value;

            if (currentPlayer.Health == 0 || opponentPlayer.Health == 0)
                return;

            switch (action.ActionType)
            {
                case CardAction.Move:
                    MoveStagePosition(currentPlayer, action.Range);
                    break;
                case CardAction.Attack:
                    AttackOpponentPlayer(currentPlayer, opponentPlayer, action);
                    break;
                case CardAction.Regenerate:
                    currentPlayer.RegenerateEnergy(action.Energy);
                    break;
                case CardAction.Guard:
                    currentPlayer.SetGuardDefense(action.Damage);
                    break;
            }

            currentPlayer.SpendEnergy(action.Energy);
        }

        /// <summary>
        /// Move player to position specified
        /// </summary>
        /// <param name="player"></param>
        /// <param name="movementRange"></param>
        private void MoveStagePosition(PlayerState player, int[][] movementRange)
        {
            Coordinates positionChange = GetActiveHitboxDifferentialsFromRange(movementRange).First();

            if (IsInStageBounds(player.StagePosition.X + positionChange.X, player.StagePosition.Y + positionChange.Y))
            {
                player.SetStagePosition(positionChange.X, positionChange.Y);
            }
        }

        private void AttackOpponentPlayer(PlayerState attacker, PlayerState opponent, Card action)
        {
            IEnumerable<Coordinates> positionChangeList = GetActiveHitboxDifferentialsFromRange(action.Range);

            foreach (Coordinates attack in positionChangeList)
            {
                int attackCoordX = attacker.StagePosition.X + attack.X;
                int attackCoordY = attacker.StagePosition.Y + attack.Y;

                if (attackCoordX == opponent.StagePosition.X && attackCoordY == opponent.StagePosition.Y)
                {
                    // Calculate remaining damage if guard was applied
                    var damageDealtAfterGuard = (opponent.GuardDefense < Globals.MaxHealth) ? Math.Max(action.Damage - opponent.GuardDefense, 0) : 0;
                    
                    opponent.TakeAttackDamage(damageDealtAfterGuard);
                    break;
                }
            }
        }

        /// <summary>
        /// Get differentials of a card's active hitbox
        /// </summary>
        /// <param name="cardRange"></param>
        /// <returns></returns>
        private List<Coordinates> GetActiveHitboxDifferentialsFromRange(int[][] cardRange)
        {
            // Differentials are viewed from the perspective of (1,1):
            /*
             * [0, 0, 0]
             * [1, 0, 1]
             * [0, 0, 0]
             * 
             * This is positions (1,0) and (1,2) respectively,
             * so this list returns differentials (0,-1) and (0,1).
             */
            
            var differentials = new List<Coordinates>();

            for (int i = 0; i < cardRange.Length; i++)
            {
                for (int j = 0; j < cardRange[i].Length; j++)
                {
                    if (cardRange[i][j] == 1)
                        differentials.Add(new Coordinates() { X = i-1, Y = j-1 });
                }
            }

            return differentials;
        }

        private bool IsInStageBounds(int positionX, int positionY) 
        {
            return (positionX >= 0 && positionY >= 0 && positionX < Globals.MaxArenaLength && positionY < Globals.MaxArenaHeight);
        }

        private void ResetGuardStats(GameState gameState)
        {
            foreach(var player in gameState.Players.Values.Where(p => p.GuardDefense > 0))
            {
                player.SetGuardDefense(0);
            }
        }

        private void EndOfRound(GameState gameState)
        {
            foreach (var player in gameState.Players.Values)
            {
                player.RegenerateEnergy(Globals.EOREnergyGain);
            }
        }
    }
}
