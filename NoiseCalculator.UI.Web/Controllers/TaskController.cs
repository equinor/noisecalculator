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
using NoiseCalculator.UI.Web.Models;
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
        private readonly IRoleDAO _roleDAO;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, IPdfExporter pdfExporter, INoiseLevelService noiseLevelService, IRoleDAO roleDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _pdfExporter = pdfExporter;
            _noiseLevelService = noiseLevelService;
            _roleDAO = roleDAO;
        }
        

        public ActionResult Index()
        {
            IList<SelectedTaskViewModel> selectedTasks = new List<SelectedTaskViewModel>();
            foreach (SelectedTask selectedTask in _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now))
            {
                selectedTasks.Add(CreateViewModel(selectedTask));
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            return View(selectedTasks);
        }


        public ActionResult PdfReport(ReportInfo reportInfo)
        {
            reportInfo.CreatedBy = UserHelper.GetUsernameWithoutDomain(User.Identity.Name);
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            if (selectedTasks.Count() > 0)
            {
                reportInfo.Footnotes.Add(TaskResources.FooterGL0169);
                reportInfo.Footnotes.Add(TaskResources.Footer80dBA);
                reportInfo.Footnotes.Add(TaskResources.FooterNoiseProtectionDefinition);
                foreach (string dynamicFootnote in GetDynamicFootnotes(selectedTasks))
                {
                    reportInfo.Footnotes.Add(dynamicFootnote);
                }

                Stream memoryStream = _pdfExporter.GenerateSelectedTasksPDF(selectedTasks, reportInfo);
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=MyTasks-" + DateTime.Now.Date.ToShortDateString() + ".pdf");
                return new FileStreamResult(memoryStream, "application/pdf");
            }

            // No tasks found - Redirect to main page
            return RedirectToAction("Index");
        }


        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAllOrdered();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_TaskDialog", tasks);
        }


        public ActionResult GetCreateFormForTask(int id)
        {
            Task task = _taskDAO.GetFilteredByCurrentCulture(id);

            switch (task.Role.Title)
            {
                case "Helideck":
                    return RedirectToAction("AddTaskHelideck", "Helideck", new { TaskId = task.Id });
                default:
                    return RedirectToAction("AddTaskRegular", "Regular", new { TaskId = task.Id });
            }
        }

        public ActionResult GetEditFormForSelectedTask(int id)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            Task task = _taskDAO.Get(selectedTask.TaskId);

            switch (task.Role.Title)
            {
                case "Helideck":
                    return RedirectToAction("EditTaskHelideck", "Helideck", new { selectedTaskId = selectedTask.Id });
                default:
                    return RedirectToAction("EditTaskRegular", "Regular", new { selectedTaskId = selectedTask.Id });
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

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

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

        [HttpPost]
        public ActionResult RemoveAllTasks()
        {
            try
            {
                // OPTIMIZE OPTIMIZE OPTIMIZE OPTIMIZE
                foreach (SelectedTask selectedTask in _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now))
                {
                    _selectedTaskDAO.Delete(selectedTask);
                }
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
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now).ToList();

            TotalNoiseDosageViewModel totalNoiseDosage = new TotalNoiseDosageViewModel();
            totalNoiseDosage.Percentage = selectedTasks.Sum(x => x.Percentage);
            NoiseLevelEnum noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage.Percentage);
            totalNoiseDosage.StatusText = _noiseLevelService.GetNoiseLevelStatusText(noiseLevelEnum);

            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Normal:
                    totalNoiseDosage.CssClass = "noiseLevelNormal";
                    break;
                case NoiseLevelEnum.Warning:
                    totalNoiseDosage.CssClass = "noiseLevelWarning";
                    break;
                case NoiseLevelEnum.Critical:
                    totalNoiseDosage.CssClass = "noiseLevelCritical";
                    break;
            }

            //totalNoiseDosage.DynamicFootnotes.Add("Dynamic Footer 1");
            //totalNoiseDosage.DynamicFootnotes.Add("Dynamic Footer 2");
            totalNoiseDosage.DynamicFootnotes = GetDynamicFootnotes(selectedTasks);

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
        }

        public IList<string> GetDynamicFootnotes(IEnumerable<SelectedTask> selectedTasks)
        {
            bool hasNoisyWork = false;
            var roleIDs = _roleDAO.GetAreaNoiseRoleIds();
            
            foreach (SelectedTask selectedTask in selectedTasks)
            {
                if(hasNoisyWork == false)
                {
                    Task task = _taskDAO.Get(selectedTask.TaskId);
                    if(roleIDs.Contains(task.Role.Id) == false)
                    {
                        hasNoisyWork = true;
                    }
                }
            }

            IList<string> dynamicFootnotes = new List<string>();
            
            if(hasNoisyWork)
            {
                dynamicFootnotes.Add(TaskResources.FooterDynamicNoiseProtection);
                dynamicFootnotes.Add(TaskResources.FooterDynamicCorrectionForMeasuredNoiseLevel);
                dynamicFootnotes.Add(TaskResources.FooterDynamicValidForAreaNoiseUpTo90dBA);
            }

            return dynamicFootnotes;
        }

        public SelectedTaskViewModel CreateViewModel(SelectedTask selectedTask)
        {
            SelectedTaskViewModel viewModel = new SelectedTaskViewModel
            {
                Id = selectedTask.Id,
                Title = selectedTask.Title,
                Role = selectedTask.Role,
                NoiseProtection = selectedTask.NoiseProtection,
                TaskId = selectedTask.TaskId,
                HelicopterTaskId = selectedTask.HelicopterTaskId
            };
            
            Task task = _taskDAO.Get(selectedTask.TaskId);

            if(selectedTask.IsNoiseMeassured)
            {
                viewModel.NoiseLevel = string.Format("{0} dBA {1}", selectedTask.NoiseLevel, TaskResources.SelectedTaskNoiseMeasured);
            }
            else
            {
                viewModel.NoiseLevel = selectedTask.NoiseLevel.ToString(CultureInfo.InvariantCulture);
            }
            
            
            
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
