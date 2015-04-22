using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.UI.Web.Controllers
{
    public class RegularController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";
        private const string InputNotChecked = "";

        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly INoiseProtectionDAO _noiseProtectionDAO;


        public RegularController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, INoiseProtectionDAO noiseProtectionDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _noiseProtectionDAO = noiseProtectionDAO;
        }

        public PartialViewResult AddTaskRegular(int id)
        {
            var task = _taskDAO.GetFilteredByCurrentCulture(id);
            
            var viewModel = new RegularViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = task.Role.RoleType.ToString(),
                NoiseLevelGuideline = task.NoiseLevelGuideline.ToString(CultureInfo.InvariantCulture),
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked,
                ButtonPressed = task.ButtonPressed,
                BackgroundNoise = task.BackgroundNoise
            };

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (NoiseProtection noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                SelectListItem selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() };
                if (viewModel.NoiseProtectionId == noiseProtection.Id)
                {
                    selectListItem.Selected = true;
                }
                viewModel.NoiseProtection.Add(selectListItem);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateRegularTask", viewModel);
        }

        [HttpPost]
        public PartialViewResult AddTaskRegular(RegularViewModel viewModel)
        {
            var task = _taskDAO.GetFilteredByCurrentCulture(viewModel.TaskId);
            
            var validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            var selectedTask = CreateSelectedTaskRegular(viewModel, task);
            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }


        public PartialViewResult EditTaskRegular(int id)
        {
            var selectedTask = _selectedTaskDAO.Get(id);

            var viewModel = new RegularViewModel
            {
                TaskId = selectedTask.Task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = selectedTask.Task.Title,
                Role = selectedTask.Task.Role.Title,
                RoleType = selectedTask.Task.Role.RoleType.ToString(),
                NoiseLevelGuideline = selectedTask.Task.NoiseLevelGuideline.ToString(CultureInfo.InvariantCulture),
                NoiseLevelMeassured = selectedTask.NoiseLevel,
                RadioNoiseMeassuredNoCheckedAttr = selectedTask.IsNoiseMeassured ? InputNotChecked : InputChecked,
                RadioNoiseMeassuredYesCheckedAttr = selectedTask.IsNoiseMeassured ? InputChecked : InputNotChecked,
                RadioTimeCheckedAttr = InputChecked,
                ButtonPressed = selectedTask.ButtonPressed,
                BackgroundNoise = selectedTask.BackgroundNoise,
                
                Hours = selectedTask.Hours.ToString(CultureInfo.InvariantCulture),
                Minutes = selectedTask.Minutes.ToString(CultureInfo.InvariantCulture)
            };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditRegularTask", viewModel);
        }


        [HttpPost]
        public PartialViewResult EditTaskRegular(int id, RegularViewModel viewModel)
        {
            var selectedTask = _selectedTaskDAO.Get(id);

            var validationViewModel = ValidateInput(viewModel, selectedTask.Task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            if (viewModel.ButtonPressed == 0)
                selectedTask.ButtonPressed = selectedTask.Task.ButtonPressed;
            else if (viewModel.ButtonPressed >= selectedTask.Task.ButtonPressed)
                selectedTask.ButtonPressed = viewModel.ButtonPressed;

            if (viewModel.BackgroundNoise == 0)
                selectedTask.BackgroundNoise = selectedTask.Task.BackgroundNoise;
            else if (viewModel.BackgroundNoise >= selectedTask.Task.BackgroundNoise)
                selectedTask.BackgroundNoise = viewModel.BackgroundNoise;

            if(viewModel.NoiseLevelMeassured == 0)
            {
                selectedTask.NoiseLevel = selectedTask.Task.NoiseLevelGuideline;
                selectedTask.IsNoiseMeassured = false;
            }
            else if (viewModel.NoiseLevelMeassured >= selectedTask.Task.NoiseLevelGuideline)
            {
                selectedTask.NoiseLevel = viewModel.NoiseLevelMeassured;
                selectedTask.IsNoiseMeassured = true;
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                var timeSpan = selectedTask.Task.CalculateTimeSpan(selectedTask.NoiseLevel, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }
            else
            {
                var timeSpan = new TimeSpanFactory().CreateFromStrings(viewModel.Hours, viewModel.Minutes);

                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.Percentage = (int)selectedTask.Task.CalculatePercentage(selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0));
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
            var errorSummaryViewModel = new ValidationErrorSummaryViewModel();

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
            var selectedTask = new SelectedTask
            {
                Title = task.Title,
                Role = task.Role.Title,
                NoiseProtection = task.NoiseProtection.Title,
                ButtonPressed = task.ButtonPressed,
                BackgroundNoise = task.BackgroundNoise,
                Task = task,
                CreatedBy = string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name,
                CreatedDate = DateTime.Now.Date
            };

            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(viewModel.NoiseProtectionId);
            if (noiseProtection != null)
                selectedTask.NoiseProtection = noiseProtection.Title;

            if (viewModel.ButtonPressed != task.ButtonPressed)
                selectedTask.ButtonPressed = viewModel.ButtonPressed;

            if (viewModel.BackgroundNoise != task.BackgroundNoise)
                selectedTask.BackgroundNoise = viewModel.BackgroundNoise;

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
                var timeSpan = new TimeSpanFactory().CreateFromStrings(viewModel.Hours, viewModel.Minutes);
                
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.Percentage = (int)task.CalculatePercentage(selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, timeSpan);
            }
            else
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                var timeSpan = task.CalculateTimeSpan(selectedTask.NoiseLevel, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }

            return selectedTask;
        }
    }
}
