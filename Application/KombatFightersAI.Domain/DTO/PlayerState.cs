
namespace KombatFightersAI.Domain.DTO
{
    public class PlayerState
    {
        public required Character Character { get; set; }
        public int Health { get; set; }
        public int Energy { get; set; }
        public int GuardDefense { get; set; }
        public required Coordinates StagePosition { get; set; }
        public required List<string> CardsUsed { get; set; }

        public PlayerState DeepCopy()
        {
            return new PlayerState
            {
                Character = Character.DeepCopy(),
                Health = Health,
                Energy = Energy,
                GuardDefense = GuardDefense,
                StagePosition = StagePosition.DeepCopy(),
                CardsUsed = new List<string>(CardsUsed)
            };
        }

        public void TakeAttackDamage(int damage)
        {
            Health = Math.Max(Health - damage, 0);
        }

        public void RegenerateEnergy(int energyGained)
        {
            Energy = Math.Min(Energy + Math.Abs(energyGained), Globals.MaxEnergy);
        }

        public void SpendEnergy(int energySpent)
        {
            // Ensure energy spent does not increase
            Energy = Math.Max(Energy - Math.Abs(energySpent), 0); 
        }

        public void SetGuardDefense(int defense)
        {
            GuardDefense = Math.Min(Math.Abs(defense), Globals.MaxHealth);
        }

        public void SetStagePosition(int posChangeX, int posChangeY)
        {
            StagePosition.X += posChangeX;
            StagePosition.Y += posChangeY;
        }

        public List<Card> GetAvailableMoves()
        {
            return Character.MoveList
                .Where(card => Energy - card.Energy >= 0 && !(CardsUsed.Contains(card.KeyboardCard)))
                .ToList();
        }
    }
}
