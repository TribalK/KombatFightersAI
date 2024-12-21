namespace KombatFightersAI.Domain.DTO
{
    public class Coordinates
    {
        public int X {  get; set; }
        public int Y {  get; set; }

        public Coordinates DeepCopy()
        {
            return new Coordinates
            {
                X = X,
                Y = Y
            };
        }
    }
}
