namespace NoiseCalculator.Domain.Entities
{
    public class Rotation
    {
        public virtual int Id { get; set; }
        public virtual Task Task { get; set; }
        public virtual Task OperatorTask { get; set; }
        public virtual Task AssistantTask { get; set; }
    }
}
