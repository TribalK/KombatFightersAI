using Newtonsoft.Json;

namespace KombatFightersAI.Domain.DTO
{
    public class Card
    {
        [JsonProperty("card_name")]
        public required string Name { get; set; }
        [JsonProperty("card_damage")]
        public int Damage { get; set; }
        [JsonProperty("card_energy")]
        public int Energy { get; set; }
        [JsonProperty("card_range")]
        public required int[][] Range { get; set; }
        [JsonProperty("card_keyboard_letter")]
        public required string KeyboardCard { get; set; }
        public CardAction ActionType 
        { 
            get
            {
                if (Damage == 0 && Energy == 0)
                    return CardAction.Move;
                else if (Damage > 0)
                    return CardAction.Attack;
                else if (Energy < 0)
                    return CardAction.Regenerate;
                else if (Damage < 0)
                    return CardAction.Guard;
                else 
                    return CardAction.Undefined;
            }
        }

        public Card DeepCopy()
        {
            return new Card
            {
                Name = Name,
                Damage = Damage,
                Energy = Energy,
                Range = Range.Select(rangeRow => (int[])rangeRow.Clone()).ToArray(),
                KeyboardCard = KeyboardCard
            };
        }
    }
}
