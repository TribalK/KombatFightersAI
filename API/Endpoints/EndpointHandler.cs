using KombatFightersAI.Domain.Commands;
using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.AiBehavior;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Core.Endpoints
{
    public class EndpointHandler
    {
        private IJsonConfigDeserializer _configDeserializer;
        private ICharacterLoader _characterLoader;
        private MinimaxPrune _minimaxPrune;

        public EndpointHandler(IJsonConfigDeserializer configDeserializer, ICharacterLoader characterLoader, MinimaxPrune minimaxPrune)
        {
            _configDeserializer = configDeserializer;
            _characterLoader = characterLoader;
            _minimaxPrune = minimaxPrune;
        }

        public string GetAllCharacterDataHandler()
        {
            return _configDeserializer.LoadDataFromJsonFiles("Characters.json");
        }

        public List<PlayerState> GetSelectedCharacterDataHandler(int p1CharId, int p2CharId)
        {
            List<Character> charactersFound = _characterLoader.SearchCharactersByIds(p1CharId, p2CharId);

            return new List<PlayerState>()
            {
                // Mock out Player 1 initial state
                new PlayerState
                {
                    Character = charactersFound[0],
                    Health = Globals.MaxHealth,
                    Energy = Globals.MaxEnergy,
                    StagePosition = new Coordinates {
                        X = 1,
                        Y = 0
                    },
                    CardsUsed = new List<string>()
                },
                // Mock out Player 2
                new PlayerState
                {
                    Character = charactersFound[1],
                    Health = Globals.MaxHealth,
                    Energy = Globals.MaxEnergy,
                    StagePosition = new Coordinates {
                        X = 1,
                        Y = Globals.MaxArenaLength-1
                    },
                    CardsUsed = new List<string>()
                }
            };
        }

        public async Task RunSimulationAsyncHandler(RunSimulationRequest request)
        {
            var selectedCharacters = _characterLoader.SearchCharactersByIds(request.PlayerOneState.CharacterId, request.PlayerTwoState.CharacterId);

            GameState gameState = new GameState()
            {
                PlayerKeys = [request.PlayerOneState.PlayerKey, request.PlayerTwoState.PlayerKey],
                Players = new Dictionary<string, PlayerState>()
            };

            gameState.Players.Add(request.PlayerOneState.PlayerKey, new PlayerState
            {
                Character = selectedCharacters[0],
                Health = request.PlayerOneState.Health,
                Energy = request.PlayerOneState.Energy,
                StagePosition = new Coordinates
                {
                    X = request.PlayerOneState.StagePosition.X,
                    Y = request.PlayerOneState.StagePosition.Y
                },
                CardsUsed = request.PlayerOneState.CardsUsed != null
                    ? new List<string>(request.PlayerOneState.CardsUsed.Distinct()) 
                    : new List<string>()
            });

            gameState.Players.Add(request.PlayerTwoState.PlayerKey, new PlayerState
            {
                Character = selectedCharacters[1],
                Health = request.PlayerTwoState.Health,
                Energy = request.PlayerTwoState.Energy,
                StagePosition = new Coordinates
                {
                    X = request.PlayerTwoState.StagePosition.X,
                    Y = request.PlayerTwoState.StagePosition.Y
                },
                CardsUsed = request.PlayerTwoState.CardsUsed != null 
                    ? new List<string>(request.PlayerTwoState.CardsUsed.Distinct()) 
                    : new List<string>()
            });

            if (gameState.Players[request.PlayerOneState.PlayerKey].CardsUsed.Count != Globals.CardsUsedPerTurn)
            {
                await _minimaxPrune.BuildPlayerOptimalActionCardList(gameState, request.PlayerOneState.PlayerKey);
            }
        }
    }
}
