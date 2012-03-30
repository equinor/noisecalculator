using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class RotationController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";
        private const string InputNotChecked = "";

        private ISelectedTaskDAO _selectedTaskDAO;
        private IRotationDAO _rotationDAO;
        private ITaskDAO _taskDAO;
        
        
        public RotationController(ITaskDAO taskDAO, IRotationDAO rotationDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _rotationDAO = rotationDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }

        public PartialViewResult AddTask(int taskId)
        {
            Task task = _taskDAO.Get(taskId);
            Rotation rotation = _rotationDAO.GetByTaskId(taskId);

            RotationViewModel viewModel = new RotationViewModel()
            {
                RotationId = rotation.Id,
                Title = task.Title,
                OperatorNoiseLevelGuideline = rotation.OperatorTask.NoiseLevelGuideline.ToString(),
                OperatorTitle = rotation.OperatorTask.Title,
                AssistantNoiseLevelGuideline = rotation.AssistantTask.NoiseLevelGuideline.ToString(),
                AssistantTitle = rotation.AssistantTask.Title,
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked,
                RoleType = RoleTypeEnum.Rotation.ToString()
            };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateTask", viewModel);
        }

        //[HttpPost]
        //public PartialViewResult AddTask(int taskId)
        //{
        //    return new EmptyResult();
        //    //return PartialView("_SelectedTask", selectedTaskViewModel);
        //}
    }
}
