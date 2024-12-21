using Newtonsoft.Json;

namespace KombatFightersAI.Domain.DTO
{
    public class Character
    {
        [JsonProperty("character_id")]
        public required int Id { get; set; }
        [JsonProperty("character_name")]
        public required string CharacterName { get; set; }
        [JsonProperty("attack_cards")]
        public required List<Card> MoveList { get; set; }

        public Character DeepCopy()
        {
            return new Character
            {
                Id = Id,
                CharacterName = CharacterName,
                MoveList = MoveList.Select(card => card.DeepCopy()).ToList()
            };
        }
    }
}
