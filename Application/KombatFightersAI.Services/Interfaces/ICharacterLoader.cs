using KombatFightersAI.Domain.DTO;

namespace KombatFightersAI.Services.Interfaces
{
    public interface ICharacterLoader
    {
        public List<Character> SearchCharactersByIds(int p1CharId, int p2CharId);
    }
}
