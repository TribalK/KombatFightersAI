using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Test.EvaluationLogicTests
{
    public class Tests
    {
        private ICharacterLoader _characterLoader;

        [SetUp]
        public void Setup(ICharacterLoader characterLoader)
        {
            _characterLoader = characterLoader;
        }

        [TestCase(0,0)]
        public void Test1(int p1CharId, int p2CharId)
        {
            var selectedCharacters = _characterLoader.SearchCharactersByIds(p1CharId, p2CharId);
            /*
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
            });*/

            Assert.Pass();
        }
    }
}