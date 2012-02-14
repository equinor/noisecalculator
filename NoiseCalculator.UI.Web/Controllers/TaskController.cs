using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }
        

        public ActionResult Index()
        {
            IList<SelectedTaskViewModel> selectedTasks = new List<SelectedTaskViewModel>();
            foreach (SelectedTask selectedTask in _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now))
            {
                selectedTasks.Add(CreateViewModel(selectedTask));
            }

            return View(selectedTasks);
        }


        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAllOrdered();
            return PartialView("_TaskDialog", tasks);
        }


        public ActionResult GetCreateFormForTask(int id)
        {
            Task task = _taskDAO.Get(id);

            switch (task.Role.Title)
            {
                case "Helideck":
                    return RedirectToAction("AddTaskHelideck", "Helideck", new {TaskId = task.Id});
                default:
                    return RedirectToAction("AddTaskRegular", "Regular", new {TaskId = task.Id});
            }
        }

        public ActionResult GetEditFormForSelectedTask(int id)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            Task task = _taskDAO.Get(selectedTask.TaskId);

            switch (task.Role.Title)
            {
                case "Helideck":
                    return RedirectToAction("EditTaskHelideck", "Helideck", new {selectedTaskId = selectedTask.Id});
                default:
                    return RedirectToAction("EditTaskRegular", "Regular", new {selectedTaskId = selectedTask.Id});
            }
        }

        public ActionResult RemoveTask(int id)
        {
            try
            {
                _selectedTaskDAO.Delete(_selectedTaskDAO.Load(id));
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }

        public JsonResult GetTotalPercentage()
        {
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            TotalNoiseDosageViewModel totalNoiseDosage = new TotalNoiseDosageViewModel();
            totalNoiseDosage.Percentage = selectedTasks.Sum(x => x.Percentage);
            
            if(totalNoiseDosage.Percentage < 75)
            {
                totalNoiseDosage.StatusText = "Noise level is considered safe";
                totalNoiseDosage.CssClass = "noiseLevelNormal";
            }
            else if(totalNoiseDosage.Percentage >= 75 && totalNoiseDosage.Percentage < 100)
            {
                totalNoiseDosage.StatusText = "Noise level is approaching allowed limits";
                totalNoiseDosage.CssClass = "noiseLevelWarning";
            }
            else if (totalNoiseDosage.Percentage >= 100)
            {
                totalNoiseDosage.StatusText = "Unsafe daily noise dosage";
                totalNoiseDosage.CssClass = "noiseLevelCritical";
            }

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
        }


        public SelectedTaskViewModel CreateViewModel(SelectedTask selectedTask)
        {
            SelectedTaskViewModel viewModel = new SelectedTaskViewModel
            {
                Id = selectedTask.Id,
                Title = selectedTask.Title,
                Role = selectedTask.Role,
                NoiseProtection = selectedTask.NoiseProtection,
                NoiseLevel = selectedTask.NoiseLevel.ToString(CultureInfo.InvariantCulture),
                TaskId = selectedTask.TaskId,
                HelicopterTaskId = selectedTask.HelicopterTaskId
            };
            
            Task task = _taskDAO.Get(selectedTask.TaskId);
            
            if(task.Role.RoleType == RoleTypeEnum.Regular)
            {
                viewModel.Percentage = selectedTask.Percentage.ToString(CultureInfo.InvariantCulture);
                viewModel.Hours = selectedTask.Hours.ToString(CultureInfo.InvariantCulture);
                viewModel.Minutes = selectedTask.Minutes.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                // HELIDECK
                viewModel.Percentage = selectedTask.Percentage.ToString(CultureInfo.InvariantCulture);
                viewModel.Hours = "0";
                viewModel.Minutes = selectedTask.Minutes.ToString(CultureInfo.InvariantCulture);
            }
            
            return viewModel;
        }
    }
}
