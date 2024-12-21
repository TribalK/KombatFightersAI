using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.Interfaces;

namespace KombatFightersAI.Services.AiBehavior
{
    public static class EvaluationLogicHelper
    {
        public static double EvaluatePlayerStateAdvantage(PlayerState currSlctState, PlayerState currOpptState, PlayerState updatedSlctState, PlayerState updatedOpptState, bool randomPriorityOccurrence, int depth)
        {
            double evalScore = 0.0;
            double depthFactor = (1 + 0.1 * depth);

            // Evaluation based on PlayerState properties:
            // 1. Damage dealt vs damage received?
            // 2. If no damage dealt, energy gained or lost?
            // 3. No damage or energy changes, displacement changes from movement

            // Current user has died - lowest eval score
            // What if the card used was based on random Id? If it results in a 
            if ((updatedSlctState.Health == 0) || (randomPriorityOccurrence && (updatedSlctState.Health == 0 || updatedOpptState.Health == 0)))
            {
                return Globals.LowestEvalScore;
            }

            int damageRatio = (updatedSlctState.Health - currSlctState.Health) - (updatedOpptState.Health - currOpptState.Health);
            double damageEvalScore = damageRatio * Globals.AttackStrikeMultiplier * depthFactor;

            int energyGain = (updatedSlctState.Energy - currSlctState.Energy);
            double energyEvalScore = energyGain * Globals.EnergyUsedMultiplier * depthFactor;

            int movementDisplacement = Math.Abs((updatedSlctState.StagePosition.X - currSlctState.StagePosition.X) + (updatedSlctState.StagePosition.Y - currSlctState.StagePosition.Y));
            double displacementEvalScore = movementDisplacement * depthFactor;

            if (damageEvalScore != 0)
                return damageEvalScore;

            if (energyEvalScore != 0)
                return energyEvalScore;

            if (displacementEvalScore != 0) 
                return displacementEvalScore;
            
            return evalScore;
        }
    }
}
