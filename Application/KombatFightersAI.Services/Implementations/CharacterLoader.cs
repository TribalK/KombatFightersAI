using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Services.Implementations
{
    public class CharacterLoader: ICharacterLoader
    {
        private IJsonConfigDeserializer _configDeserializer;

        public CharacterLoader(IJsonConfigDeserializer jsonConfigDeserializer)
        {
            _configDeserializer = jsonConfigDeserializer;
        }
        
        public List<Character> SearchCharactersByIds(int p1CharId, int p2CharId)
        {
            var selectedCharacters = new List<Character>();

            // Parse data from JSON configuration files
            var allCharacterData = _configDeserializer.LoadDataFromJsonFiles("Characters.json");
            var standardCardData = _configDeserializer.LoadDataFromJsonFiles("StandardCards.json");

            var characterDataList = _configDeserializer.DeserializeJsonData<IEnumerable<Character>>(allCharacterData);

            // Filter list based on the selected characters
            var p1Character = characterDataList?.Where(c => c.Id == p1CharId).FirstOrDefault();
            var p2Character = characterDataList?.Where(c => c.Id == p2CharId).FirstOrDefault();

            if (p1Character == null || p2Character == null)
            {
                throw new MissingMemberException("Unable to matching find character data for one or more character ids.");
            }

            // Add Standard card data for each character
            var standardCardList = _configDeserializer.DeserializeJsonData<IEnumerable<Card>>(standardCardData) ?? Enumerable.Empty<Card>();

            p1Character.MoveList.AddRange(standardCardList);
            p2Character.MoveList.AddRange(standardCardList);

            selectedCharacters.Add(p1Character);
            selectedCharacters.Add(p2Character);

            return selectedCharacters;
        }
    }
}
