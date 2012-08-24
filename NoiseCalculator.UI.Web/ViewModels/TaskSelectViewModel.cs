using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class TaskSelectViewModel
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Role { get; private set; }
        public string RoleType { get; private set; }
        
        public TaskSelectViewModel(Task task)
        {
            Id = task.Id;
            Title = task.Title;
            Role = task.Role.Title;
            RoleType = task.Role.RoleType.ToString();
        }
    }
}