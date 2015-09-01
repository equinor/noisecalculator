using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.UI.Web.Resources;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class SelectedTaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseProtection { get; set; }
        public string NoiseLevel { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }
        public string ButtonPressed { get; set; }
        public string BackgroundNoise { get; set; }

        public int TaskId { get; set; }
        public int HelicopterTaskId { get; set; }
        public string RoleType { get; set; }
    
        public SelectedTaskViewModel(SelectedTask selectedTask)
        {
            Id = selectedTask.Id;
            Title = selectedTask.Title;
            Role = selectedTask.Role;
            NoiseProtection = selectedTask.NoiseProtection;
            TaskId = selectedTask.Task.Id;
            Percentage = selectedTask.Percentage.ToString();
            Hours = selectedTask.Hours.ToString();
            Minutes = selectedTask.Minutes.ToString();
            RoleType = selectedTask.Task.Role.RoleType.ToString();
            ButtonPressed = selectedTask.ButtonPressed.ToString();
            BackgroundNoise = selectedTask.BackgroundNoise.ToString();

            if (selectedTask.BackgroundNoise == 0)
            {
                BackgroundNoise = "<80";
            }

            if (selectedTask.HelicopterTaskId > 0)
            {
                NoiseLevel = TaskResources.SelectedTaskNoiseLevelNotApplicable;
            }
            else
            {
                var noiseLevelGuideLine = selectedTask.NoiseLevel.ToString(CultureInfo.InvariantCulture);

                if (noiseLevelGuideLine.IndexOf(".", System.StringComparison.Ordinal) > 0)
                    noiseLevelGuideLine = noiseLevelGuideLine.Substring(0, noiseLevelGuideLine.IndexOf(".", System.StringComparison.Ordinal));


                if (selectedTask.IsNoiseMeassured)
                {
                    NoiseLevel = string.Format("{0} dBA {1}", noiseLevelGuideLine, TaskResources.SelectedTaskNoiseMeasured);
                }
                else
                {
                    NoiseLevel = string.Format("{0} dBA", noiseLevelGuideLine);
                }
            }
        }

    }
}