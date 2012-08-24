using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly INoiseLevelService _noiseLevelService;
        private readonly IAdministratorDAO _administratorDAO;
        private readonly IFootnotesService _footnotesService;

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, INoiseLevelService noiseLevelService, IAdministratorDAO administratorDAO, IFootnotesService footnotesService)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _noiseLevelService = noiseLevelService;
            _administratorDAO = administratorDAO;
            _footnotesService = footnotesService;
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


        public PartialViewResult AddTask()
        {
            IEnumerable<TaskSelectViewModel> tasks = _taskDAO.GetAllOrdered().Select(task => new TaskSelectViewModel(task));
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_TaskDialog", tasks);
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

            totalNoiseDosage.Footnotes = _footnotesService.CalculateFootnotes(selectedTasks);

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
        }
    }
}
