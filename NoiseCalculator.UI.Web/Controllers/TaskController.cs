using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.Infrastructure.Pdf;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IPdfExporter _pdfExporter;
        private readonly INoiseLevelService _noiseLevelService;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, IPdfExporter pdfExporter, INoiseLevelService noiseLevelService)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _pdfExporter = pdfExporter;
            _noiseLevelService = noiseLevelService;
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


        public ActionResult PdfReport()
        {
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);
            
            if(selectedTasks.Count() > 0)
            {
                Stream memoryStream = _pdfExporter.GenerateSelectedTasksPDF(selectedTasks);
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=MyTasks-" + DateTime.Now.Date.ToShortDateString() + ".pdf");
                return new FileStreamResult(memoryStream, "application/pdf");
            }

            return new EmptyResult();
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

        public ActionResult GetRemoveTaskConfirmationDialog(int id)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);

            RemoveConfirmationViewModel viewModel = new RemoveConfirmationViewModel
                                                        {
                                                            Title = selectedTask.Title,
                                                            Role = selectedTask.Role,
                                                            SelectedTaskId = selectedTask.Id
                                                        };

            return PartialView("_RemoveTaskConfirmation", viewModel);
        }

        [HttpPost]
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
            NoiseLevelEnum noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage.Percentage);

            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Normal:
                    totalNoiseDosage.StatusText = TaskResources.NoiseLevelStatusTextNormal;
                    totalNoiseDosage.CssClass = "noiseLevelNormal";
                    break;
                case NoiseLevelEnum.Warning:
                    totalNoiseDosage.StatusText = TaskResources.NoiseLevelStatusTextWarning;
                    totalNoiseDosage.CssClass = "noiseLevelWarning";
                    break;
                case NoiseLevelEnum.Critical:
                    totalNoiseDosage.StatusText = TaskResources.NoiseLevelStatusTextCritical;
                    totalNoiseDosage.CssClass = "noiseLevelCritical";
                    break;
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
