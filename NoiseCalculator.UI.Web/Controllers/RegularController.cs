using System;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.UI.Web.Controllers
{
    public class RegularController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";
        private const string InputNotChecked = "";

        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;


        public RegularController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }

        public PartialViewResult AddTaskRegular(int taskId)
        {
            Task task = _taskDAO.GetFilteredByCurrentCulture(taskId);

            RegularViewModel viewModel = new RegularViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = task.Role.RoleType.ToString(),
                NoiseLevelGuideline = task.NoiseLevelGuideline.ToString(),
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked
            };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateRegularTask", viewModel);
        }

        [HttpPost]
        public PartialViewResult AddTaskRegular(RegularViewModel viewModel)
        {
            Task task = _taskDAO.GetFilteredByCurrentCulture(viewModel.TaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            SelectedTask selectedTask = CreateSelectedTaskRegular(viewModel, task);
            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }


        public PartialViewResult EditTaskRegular(int selectedTaskId)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(selectedTaskId);

            bool noiseLevelIsMeassuredAboveGuideline = (selectedTask.NoiseLevel > selectedTask.Task.NoiseLevelGuideline);
            bool workIsEnteredAsTime = (selectedTask.Hours > 0 || selectedTask.Minutes > 0);

            RegularViewModel viewModel = new RegularViewModel
            {
                TaskId = selectedTask.Task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = selectedTask.Task.Title,
                Role = selectedTask.Task.Role.Title,
                RoleType = selectedTask.Task.Role.RoleType.ToString(),
                NoiseLevelGuideline = selectedTask.Task.NoiseLevelGuideline.ToString(),
                NoiseLevelMeassured = selectedTask.NoiseLevel,
                RadioNoiseMeassuredNoCheckedAttr = noiseLevelIsMeassuredAboveGuideline ? InputNotChecked : InputChecked,
                RadioNoiseMeassuredYesCheckedAttr = noiseLevelIsMeassuredAboveGuideline ? InputChecked : InputNotChecked,
                RadioTimeCheckedAttr = workIsEnteredAsTime ? InputChecked : InputNotChecked,
                RadioPercentageCheckedAttr = workIsEnteredAsTime ? InputNotChecked : InputChecked,
                Hours = selectedTask.Hours.ToString(),
                Minutes = selectedTask.Minutes.ToString(),
                Percentage = selectedTask.Percentage.ToString()
            };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditRegularTask", viewModel);
        }


        [HttpPost]
        public PartialViewResult EditTaskRegular(int id, RegularViewModel viewModel)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, selectedTask.Task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            if(viewModel.NoiseLevelMeassured == 0)
            {
                selectedTask.NoiseLevel = selectedTask.Task.NoiseLevelGuideline;
            }
            else if(viewModel.NoiseLevelMeassured > selectedTask.NoiseLevel)
            {
                selectedTask.NoiseLevel = viewModel.NoiseLevelMeassured;
            }
            
            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = selectedTask.Task.CalculateTimeSpan(selectedTask.NoiseLevel, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }
            else
            {
                int hours = string.IsNullOrEmpty(viewModel.Hours) ? 0 : int.Parse(viewModel.Hours);
                int minutes = string.IsNullOrEmpty(viewModel.Minutes) ? 0 : int.Parse(viewModel.Minutes);
                TimeSpan timeSpan = new TimeSpan(0, hours, minutes, 0);

                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.Percentage = (int)selectedTask.Task.CalculatePercentage(selectedTask.NoiseLevel, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0));
            }

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
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

        
        private ValidationErrorSummaryViewModel ValidateInput(RegularViewModel viewModel, Task task)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (viewModel.NoiseLevelMeassured - task.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseLevelToHighAboveGuidline);
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes) && string.IsNullOrEmpty(viewModel.Percentage))
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorWorkTimeRequired);
            }

            return errorSummaryViewModel;
        }


        private SelectedTask CreateSelectedTaskRegular(RegularViewModel viewModel, Task task)
        {
            SelectedTask selectedTask = new SelectedTask
            {
                Title = task.Title,
                Role = task.Role.Title,
                NoiseProtection = task.NoiseProtection.Title,
                Task = task,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now.Date
            };

            if (viewModel.NoiseLevelMeassured > task.NoiseLevelGuideline)
            {
                selectedTask.NoiseLevel = viewModel.NoiseLevelMeassured;
                selectedTask.IsNoiseMeassured = true;
            }
            else
            {
                selectedTask.NoiseLevel = task.NoiseLevelGuideline;
                selectedTask.IsNoiseMeassured = false;
            }

            if (string.IsNullOrEmpty(viewModel.Percentage))
            {
                int hours = string.IsNullOrEmpty(viewModel.Hours) ? 0 : int.Parse(viewModel.Hours);
                int minutes = string.IsNullOrEmpty(viewModel.Minutes) ? 0 : int.Parse(viewModel.Minutes);
                TimeSpan timeSpan = new TimeSpan(0, hours, minutes, 0);
                
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.Percentage = (int)task.CalculatePercentage(selectedTask.NoiseLevel, timeSpan);
            }
            else
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = task.CalculateTimeSpan(selectedTask.NoiseLevel, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }

            return selectedTask;
        }
    }
}
