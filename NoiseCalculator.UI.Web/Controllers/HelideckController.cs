using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain;
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

            SelectedTaskViewModel selectedTaskViewModel = CreateViewModel(selectedTask);

            return PartialView("_SelectedTask", selectedTaskViewModel);
        }

        public PartialViewResult EditTaskHelideck(int selectedTaskId)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(selectedTaskId);

            Task task = _taskDAO.Get(selectedTask.TaskId);
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(selectedTask.HelicopterTaskId);

            HelideckViewModel viewModel = new HelideckViewModel
            {
                TaskId = task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = RoleTypeEnum.Helideck.ToString(),
                HelicopterId = helicopterTask.HelicopterType.Id,
                NoiseProtectionId = helicopterTask.HelicopterNoiseProtection.Id,
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

            bool taskValuesHaveBeenChanged = (viewModel.HelicopterId != helicopterTask.HelicopterType.Id
                                                || viewModel.NoiseProtectionId != helicopterTask.HelicopterNoiseProtection.Id
                                                || viewModel.WorkIntervalId != helicopterTask.HelicopterWorkInterval.Id);

            if (taskValuesHaveBeenChanged)
            {
                Task task = _taskDAO.Get(selectedTask.TaskId);
                HelicopterTask newHelicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, viewModel.NoiseProtectionId, viewModel.WorkIntervalId);

                selectedTask.Title = string.Format("{0} - {1}", task.Title, newHelicopterTask.HelicopterType.Title);
                selectedTask.NoiseProtection = newHelicopterTask.HelicopterNoiseProtection.Title;
                selectedTask.Percentage = newHelicopterTask.Percentage;
                selectedTask.Minutes = newHelicopterTask.GetMaximumAllowedMinutes();
                selectedTask.HelicopterTaskId = newHelicopterTask.Id;

                _selectedTaskDAO.Store(selectedTask);
            }

            SelectedTaskViewModel selectedTaskViewModel = CreateViewModel(selectedTask);

            return PartialView("_SelectedTask", selectedTaskViewModel);
        }


        private ValidationErrorSummaryViewModel ValidateInput(HelideckViewModel viewModel)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (viewModel.HelicopterId == 0)
            {
                //errorSummaryViewModel.ValidationErrors.Add("Helicopter, noise protection and work interval must be selected to add the task.");
                errorSummaryViewModel.ValidationErrors.Add("Helikopter må være valgt.");
            }

            if (viewModel.NoiseProtectionId == 0)
            {
                //errorSummaryViewModel.ValidationErrors.Add("Helicopter, noise protection and work interval must be selected to add the task.");
                errorSummaryViewModel.ValidationErrors.Add("Høselsvern må være valgt.");
            }

            if (viewModel.WorkIntervalId == 0)
            {
                //errorSummaryViewModel.ValidationErrors.Add("Helicopter, noise protection and work interval must be selected to add the task.");
                errorSummaryViewModel.ValidationErrors.Add("Arbeidstid må være valgt.");
            }

            return errorSummaryViewModel;
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

            if (task.Role.RoleType == RoleTypeEnum.Regular)
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

        private void AppendHelideckMasterData(HelideckViewModel viewModel)
        {
            viewModel.Helicopters.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                SelectListItem selectListItem = new SelectListItem { Text = type.Title, Value = type.Id.ToString() };
                if (viewModel.HelicopterId == type.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.Helicopters.Add(selectListItem);
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterNoiseProtection noiseProtection in _helicopterNoiseProtectionDAO.GetAll())
            {
                SelectListItem selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() };
                if (viewModel.NoiseProtectionId == noiseProtection.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.NoiseProtection.Add(selectListItem);
            }

            viewModel.WorkIntervals.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
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
