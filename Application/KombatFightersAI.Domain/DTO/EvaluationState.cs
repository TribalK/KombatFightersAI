namespace KombatFightersAI.Domain.DTO
{
    public class EvaluationState
    {
        public required GameState GameState { get; set; }
        public float EvalScore { get; set; }
    }
}
