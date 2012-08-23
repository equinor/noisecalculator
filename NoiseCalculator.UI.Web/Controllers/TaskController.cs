using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
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
        private readonly IAdministratorDAO _administratorDAO;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, IPdfExporter pdfExporter, INoiseLevelService noiseLevelService, IRoleDAO roleDAO, IAdministratorDAO administratorDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _pdfExporter = pdfExporter;
            _noiseLevelService = noiseLevelService;
            _roleDAO = roleDAO;
            _administratorDAO = administratorDAO;
        }
        

        public ActionResult Index()
        {
            TaskIndexViewModel viewModel = new TaskIndexViewModel();
            viewModel.IsAdmin = _administratorDAO.UserIsAdmin(UserHelper.CreateUsernameWithoutDomain(User.Identity.Name));

            foreach (SelectedTask selectedTask in _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now))
            {
                viewModel.SelectedTasks.Add(new SelectedTaskViewModel(selectedTask));
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            return View(viewModel);
        }


        public ActionResult PdfReport(ReportInfo reportInfo)
        {
            reportInfo.CreatedBy = UserHelper.CreateUsernameWithoutDomain(User.Identity.Name);
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            if (selectedTasks.Count() > 0)
            {
                reportInfo.Footnotes.AddRange(FootnoteResources.GetStaticFootnotes());
                reportInfo.Footnotes.AddRange(GetDynamicFootnotes(selectedTasks));

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
            
            switch (task.Role.RoleType)
            {
                case RoleTypeEnum.Helideck:
                    return RedirectToAction("AddTaskHelideck", "Helideck", new { TaskId = task.Id });
                case RoleTypeEnum.Rotation:
                    return RedirectToAction("AddTaskRotation", "Rotation", new { TaskId = task.Id });
                default:
                    return RedirectToAction("AddTaskRegular", "Regular", new { TaskId = task.Id });
            }
        }

        public ActionResult GetEditFormForSelectedTask(int id)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            
            switch (selectedTask.Task.Role.Title)
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
                case NoiseLevelEnum.MaximumAllowedDosage:
                    totalNoiseDosage.CssClass = "noiseLevelWarning";
                    break;
                case NoiseLevelEnum.Critical:
                    totalNoiseDosage.CssClass = "noiseLevelCritical";
                    break;
                case NoiseLevelEnum.DangerOfWorkRelatedInjury:
                    totalNoiseDosage.CssClass = "noiseLevelCritical";
                    break;
            }

            totalNoiseDosage.DynamicFootnotes = GetDynamicFootnotes(selectedTasks);

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
        }

        public IList<string> GetDynamicFootnotes(IEnumerable<SelectedTask> selectedTasks)
        {
            bool hasNoisyWork = false;
            bool hasRegularTasks = false;
            var roleIDs = _roleDAO.GetAreaNoiseRoleIds();
            
            foreach (SelectedTask selectedTask in selectedTasks)
            {
                if(roleIDs.Contains(selectedTask.Task.Role.Id) == false)
                {
                    hasNoisyWork = true;

                    if (selectedTask.HelicopterTaskId == 0)
                    {
                        hasRegularTasks = true;
                    }
                }
            }

            IList<string> dynamicFootnotes = new List<string>();
            
            if(hasNoisyWork)
            {
                dynamicFootnotes.Add(TaskResources.FooterDynamicNoiseProtection);
                dynamicFootnotes.Add(TaskResources.FooterDynamicCorrectionForMeasuredNoiseLevel);

                if (hasRegularTasks)
                {
                    dynamicFootnotes.Add(TaskResources.FooterDynamicValidForAreaNoiseUpTo90dBA);
                }
            }

            return dynamicFootnotes;
        }
    }
}
