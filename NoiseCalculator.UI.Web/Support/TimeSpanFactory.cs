using System;

namespace NoiseCalculator.UI.Web.Support
{
    public class TimeSpanFactory
    {
        public TimeSpan CreateFromStrings(string hours, string minutes)
        {
            int hoursTemp = string.IsNullOrEmpty(hours) ? 0 : int.Parse(hours);
            int minutesTemp = string.IsNullOrEmpty(minutes) ? 0 : int.Parse(minutes);
            
            return new TimeSpan(0, hoursTemp, minutesTemp, 0);
        }

        public TimeSpan CreateFromMinutes(int minutes)
        {
            return new TimeSpan(0, 0, minutes, 0);
        }
    }
}