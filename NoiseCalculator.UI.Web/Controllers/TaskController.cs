using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly INoiseLevelService _noiseLevelService;
        private readonly IAdministratorDAO _administratorDAO;
        private readonly IFootnotesService _footnotesService;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, INoiseLevelService noiseLevelService, IAdministratorDAO administratorDAO, 
            IFootnotesService footnotesService, ITaskDefinitionDAO taskDefinitionDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _noiseLevelService = noiseLevelService;
            _administratorDAO = administratorDAO;
            _footnotesService = footnotesService;
            _taskDefinitionDAO = taskDefinitionDAO;
        }
        

        public ActionResult Index()
        {
            ViewBag.UserName = UserHelper.CreateUsernameWithoutDomain2(User as ClaimsPrincipal);
            var user = UserHelper.CreateUsernameWithoutDomain2(User as ClaimsPrincipal);

            var viewModel = new TaskIndexViewModel
            {
                IsAdmin = _administratorDAO.UserIsAdmin(UserHelper.CreateUsernameWithoutDomain(string.IsNullOrEmpty(user) ? Session.SessionID : user)),
                IsLoggedIn = !string.IsNullOrEmpty(user)
            };

            foreach (var selectedTask in _selectedTaskDAO.GetAllChronologically(string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name, DateTime.Now))
            {
                viewModel.SelectedTasks.Add(new SelectedTaskViewModel(selectedTask));
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            return View(viewModel);
        }


        public PartialViewResult AddTask()
        {
            var viewModel = new TaskSelectViewModel();

            viewModel.TaskDefinitions.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });


            if (Thread.CurrentThread.CurrentCulture.Equals(new CultureInfo("nb-NO")))
            {
                foreach (var tDef in _taskDefinitionDAO.GetAllOrderedByCurrentCulture())
                {
                    var selectListItem = new SelectListItem { Text = tDef.SystemName.Replace("+", ""), Value = tDef.Id.ToString(CultureInfo.InvariantCulture) };
                    if (viewModel.TaskDefinitionId == tDef.Id)
                    {
                        selectListItem.Selected = true;
                    }
                    viewModel.TaskDefinitions.Add(selectListItem);
                }
            }
            else
            {
                foreach (var tDef in _taskDefinitionDAO.GetAllOrderedByENCulture())
                {
                    var selectListItem = new SelectListItem { Text = tDef.SystemNameEN.Replace("+", ""), Value = tDef.Id.ToString(CultureInfo.InvariantCulture) };
                    if (viewModel.TaskDefinitionId == tDef.Id)
                    {
                        selectListItem.Selected = true;
                    }
                    viewModel.TaskDefinitions.Add(selectListItem);
                }
            }

            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_TaskDialog", viewModel);
        }
        
        public ActionResult GetRemoveTaskConfirmationDialog(int id)
        {
            var selectedTask = _selectedTaskDAO.Get(id);

            var viewModel = new RemoveConfirmationViewModel
                                                        {
                                                            Title = selectedTask.Title,
                                                            Role = selectedTask.Role,
                                                            SelectedTaskId = selectedTask.Id
                                                        };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_RemoveTaskConfirmation", viewModel);
        }

        [HttpPost]
        public JsonResult GetTasks(int id)
        {
            var viewModel = new TaskSelectViewModel();
            
            foreach (var task in _taskDAO.GetAllByTaskDefinitionIdOrdered(id))
            {
                var taskText = task.Title;
                var taskValue = task.Id.ToString(CultureInfo.InvariantCulture) + "-" + task.Role.RoleType + "-" +
                                task.TaskDefinition.SystemName;
                var selectListItem = new SelectListItem { Text = taskText, Value = taskValue };
                if (viewModel.TaskId == task.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.Tasks.Add(selectListItem);
            }

            // Hack for helideck tasks
            if (viewModel.Tasks.Count == 0)
            {
                foreach (var task in _taskDAO.GetAllHelideckByTaskDefinitionIdOrdered(id))
                {
                    var taskText = task.Title;
                    var taskValue = task.Id.ToString(CultureInfo.InvariantCulture) + "-" + task.Role.RoleType + "-" +
                                    task.TaskDefinition.SystemName;
                    var selectListItem = new SelectListItem { Text = taskText, Value = taskValue };
                    if (viewModel.TaskId == task.Id)
                    {
                        selectListItem.Selected = true;
                    }
                    viewModel.Tasks.Add(selectListItem);
                }
            }

            return Json(viewModel);
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
                foreach (var selectedTask in _selectedTaskDAO.GetAllChronologically(string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name, DateTime.Now))
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

            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name, DateTime.Now).ToList();

            var totalNoiseDosage = new TotalNoiseDosageViewModel {Percentage = selectedTasks.Sum(x => x.Percentage)};
            var noiseLevelEnum = _noiseLevelService.CalculateNoiseLevelEnum(totalNoiseDosage.Percentage);
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
                    totalNoiseDosage.CssClass = "noiseLevelMaximumAllowedDosage";
                    break;
                case NoiseLevelEnum.Critical:
                    totalNoiseDosage.CssClass = "noiseLevelCritical";
                    break;
                case NoiseLevelEnum.DangerOfWorkRelatedInjury:
                    totalNoiseDosage.CssClass = "noiseLevelDangerOfWorkRelatedInjury";
                    break;
            }

            totalNoiseDosage.Footnotes = _footnotesService.CalculateFootnotes(selectedTasks);

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
        }
    }
}
