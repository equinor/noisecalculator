using System;

namespace NoiseCalculator.Domain.Entities
{
    public class SelectedTask
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Role { get; set; }
        public virtual string NoiseProtection { get; set; }
        public virtual int NoiseLevel { get; set; }
        public virtual int Hours { get; set; }
        public virtual int Minutes { get; set; }
        public virtual int Percentage { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual bool IsNoiseMeassured { get; set; }

        public virtual Task Task { get; set; }
        public virtual int HelicopterTaskId { get; set; }

        
        public virtual void AddWorkTime(TimeSpan timeSpan, int percentage)
        {
            Hours = timeSpan.Hours;
            Minutes = timeSpan.Minutes;
            Percentage = percentage;
        }
    }
}