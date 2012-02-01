using System;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.UI.Web.Controllers
{
    public class HelideckController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly IDAO<HelicopterNoiseProtection, int> _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;

        
        public HelideckController(ITaskDAO taskDAO, 
                                ISelectedTaskDAO selectedTaskDAO, 
                                IHelicopterTaskDAO helicopterTaskDAO,
                                IDAO<HelicopterType, int> helicopterTypeDAO,
                                IDAO<HelicopterNoiseProtection, int> helicopterNoiseProtectionDAO,
                                IDAO<HelicopterWorkInterval, int> helicopterWorkIntervalDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _helicopterNoiseProtectionDAO = helicopterNoiseProtectionDAO;
            _helicopterWorkIntervalDAO = helicopterWorkIntervalDAO;
        }


        //public PartialViewResult AddTaskHelideck(Task task)
        public PartialViewResult AddTaskHelideck(int taskId)
        {
            Task task = _taskDAO.Get(taskId);

            HelideckViewModel viewModel = new HelideckViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
            };

            viewModel.Helicopters.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectListItem { Text = type.Title, Value = type.Id.ToString() });
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterNoiseProtection noiseProtection in _helicopterNoiseProtectionDAO.GetAll())
            {
                viewModel.NoiseProtection.Add(new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() });
            }

            viewModel.WorkIntervals.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterWorkInterval workInterval in _helicopterWorkIntervalDAO.GetAll())
            {
                viewModel.WorkIntervals.Add(new SelectListItem { Text = workInterval.Title, Value = workInterval.Id.ToString() });
            }

            return PartialView("_TaskFormHelideck", viewModel);
        }


        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModel)
        {
            if (viewModel.HelicopterId == 0 || viewModel.NoiseProtectionId == 0 || viewModel.WorkIntervalId == 0)
            {
                Response.StatusCode = 500;
                return Json("Helicopter data is missing"); // TRANSLATION MUST BE ADDED
            }

            Task task = _taskDAO.Get(viewModel.TaskId);
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, viewModel.NoiseProtectionId, viewModel.WorkIntervalId);

            SelectedTask selectedTask = new SelectedTask
            {
                Title = string.Format("{0} - {1}", task.Title, helicopterTask.HelicopterType.Title),
                Role = task.Role.Title,
                NoiseProtection = helicopterTask.HelicopterNoiseProtection.Title,
                Percentage = helicopterTask.Percentage,
                Minutes = helicopterTask.GetMaximumAllowedMinutes(),
                TaskId = task.Id,
                HelicopterTaskId = helicopterTask.Id,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now.Date
            };

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", selectedTask);
        }

    }
}
