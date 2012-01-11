namespace NoiseCalculator.Domain.Entities
{
    public class SelectedTask
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseProtection { get; set; }
        public int NoiseLevel { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Percentage { get; set; }

        public int TaskId { get; set; }
        public int HelicopterTaskId { get; set; }
    }
}