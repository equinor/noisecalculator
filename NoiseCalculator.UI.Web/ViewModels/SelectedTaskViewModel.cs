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

        public int TaskId { get; set; }
        public int HelicopterTaskId { get; set; }
    
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


            if (selectedTask.IsNoiseMeassured)
            {
                NoiseLevel = string.Format("{0} dBA {1}", selectedTask.NoiseLevel, TaskResources.SelectedTaskNoiseMeasured);
            }
            else
            {
                NoiseLevel = string.Format("{0} dBA", selectedTask.NoiseLevel.ToString());
            }
        }
    }
}