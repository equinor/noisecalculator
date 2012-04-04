using System;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.UI.Web.Controllers
{
    public class HelideckController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly IHelicopterNoiseProtectionDAO _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;

        
        public HelideckController(ITaskDAO taskDAO, 
                                ISelectedTaskDAO selectedTaskDAO, 
                                IHelicopterTaskDAO helicopterTaskDAO,
                                IDAO<HelicopterType, int> helicopterTypeDAO,
                                IHelicopterNoiseProtectionDAO helicopterNoiseProtectionDAO,
                                IDAO<HelicopterWorkInterval, int> helicopterWorkIntervalDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _helicopterNoiseProtectionDAO = helicopterNoiseProtectionDAO;
            _helicopterWorkIntervalDAO = helicopterWorkIntervalDAO;
        }


        public PartialViewResult AddTaskHelideck(int taskId)
        {
            Task task = _taskDAO.Get(taskId);

            HelideckViewModel viewModel = new HelideckViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = RoleTypeEnum.Helideck.ToString()
            };

            AppendHelideckMasterData(viewModel);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateHelideckTask", viewModel);
        }

        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModel)
        {
            Task task = _taskDAO.Get(viewModel.TaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(viewModel.NoiseProtectionId);
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, helicopterNoiseProtection.HelicopterNoiseProtectionDefinition, viewModel.WorkIntervalId);

            SelectedTask selectedTask = new SelectedTask
            {
                Title = string.Format("{0} - {1}", task.Title, helicopterTask.HelicopterType.Title),
                Role = task.Role.Title,
                NoiseProtection = helicopterNoiseProtection.Title,
                Percentage = helicopterTask.Percentage,
                Minutes = helicopterTask.GetMaximumAllowedMinutes(),
                Task = task,
                HelicopterTaskId = helicopterTask.Id,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now.Date,
                IsNoiseMeassured = false
            };

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }

        public PartialViewResult EditTaskHelideck(int selectedTaskId)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(selectedTaskId);
            
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(selectedTask.HelicopterTaskId);
            HelicopterNoiseProtection helicopterNoiseProtection = 
                _helicopterNoiseProtectionDAO.GetByDefinitionAndCurrentCulture(helicopterTask.HelicopterNoiseProtectionDefinition);

            HelideckViewModel viewModel = new HelideckViewModel
            {
                TaskId = selectedTask.Task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = selectedTask.Task.Title,
                Role = selectedTask.Task.Role.Title,
                RoleType = RoleTypeEnum.Helideck.ToString(),
                HelicopterId = helicopterTask.HelicopterType.Id,
                NoiseProtectionId = helicopterNoiseProtection.Id,
                WorkIntervalId = helicopterTask.HelicopterWorkInterval.Id
            };

            AppendHelideckMasterData(viewModel);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditHelideckTask", viewModel);
        }

        [HttpPost]
        public PartialViewResult EditTaskHelideck(int id, HelideckViewModel viewModel)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(selectedTask.HelicopterTaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.GetByDefinitionAndCurrentCulture( helicopterTask.HelicopterNoiseProtectionDefinition);
            
            bool taskValuesHaveBeenChanged = (viewModel.HelicopterId != helicopterTask.HelicopterType.Id
                                                || (viewModel.NoiseProtectionId != helicopterNoiseProtection.Id)
                                                || viewModel.WorkIntervalId != helicopterTask.HelicopterWorkInterval.Id);

            if (taskValuesHaveBeenChanged)
            {
                HelicopterNoiseProtection newHelicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(viewModel.NoiseProtectionId);
                HelicopterTask newHelicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, newHelicopterNoiseProtection.HelicopterNoiseProtectionDefinition, viewModel.WorkIntervalId);

                selectedTask.Title = string.Format("{0} - {1}", selectedTask.Task.Title, newHelicopterTask.HelicopterType.Title);
                selectedTask.NoiseProtection = newHelicopterNoiseProtection.Title;
                selectedTask.Percentage = newHelicopterTask.Percentage;
                selectedTask.Minutes = newHelicopterTask.GetMaximumAllowedMinutes();
                selectedTask.HelicopterTaskId = newHelicopterTask.Id;

                _selectedTaskDAO.Store(selectedTask);
            }

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }


        private ValidationErrorSummaryViewModel ValidateInput(HelideckViewModel viewModel)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (viewModel.HelicopterId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterTypeRequired);
            }

            if (viewModel.NoiseProtectionId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);
            }

            if (viewModel.WorkIntervalId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterWorkIntervalRequired);
            }

            return errorSummaryViewModel;
        }


        private void AppendHelideckMasterData(HelideckViewModel viewModel)
        {
            viewModel.Helicopters.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                SelectListItem selectListItem = new SelectListItem { Text = type.Title, Value = type.Id.ToString() };
                if (viewModel.HelicopterId == type.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.Helicopters.Add(selectListItem);
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (HelicopterNoiseProtection noiseProtection in _helicopterNoiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                SelectListItem selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() };
                if (viewModel.NoiseProtectionId == noiseProtection.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.NoiseProtection.Add(selectListItem);
            }

            viewModel.WorkIntervals.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (HelicopterWorkInterval workInterval in _helicopterWorkIntervalDAO.GetAll())
            {
                SelectListItem selectListItem = new SelectListItem { Text = workInterval.Title, Value = workInterval.Id.ToString() };
                if (viewModel.WorkIntervalId == workInterval.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.WorkIntervals.Add(selectListItem);
            }
        }
    }
}
