using System;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class RotationController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";

        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IRotationDAO _rotationDAO;
        private readonly ITaskDAO _taskDAO;
        private readonly INoiseProtectionDAO _noiseProtectionDAO;

        public RotationController(ITaskDAO taskDAO, IRotationDAO rotationDAO, ISelectedTaskDAO selectedTaskDAO, INoiseProtectionDAO noiseProtectionDAO)
        {
            _taskDAO = taskDAO;
            _rotationDAO = rotationDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _noiseProtectionDAO = noiseProtectionDAO;
        }

        public PartialViewResult AddTaskRotation(int id)
        {
            var task = _taskDAO.Get(id);
            var rotation = _rotationDAO.GetByTaskId(id);

            var viewModel = new RotationViewModel()
            {
                RotationId = rotation.Id,
                Title = task.Title,
                OperatorNoiseLevelGuideline = rotation.OperatorTask.NoiseLevelGuideline.ToString(),
                OperatorTitle = rotation.OperatorTask.Title,
                AssistantNoiseLevelGuideline = rotation.AssistantTask.NoiseLevelGuideline.ToString(),
                AssistantTitle = rotation.AssistantTask.Title,
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked,
                RoleType = RoleTypeEnum.Rotation.ToString()
            };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateTask", viewModel);
        }

        [HttpPost]
        public PartialViewResult AddTaskRotation(RotationViewModel viewModel)
        {
            var rotation = _rotationDAO.Get(viewModel.RotationId);

            var validationViewModel = ValidateInput(viewModel, rotation);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            var selectedTaskOperator = CreateSelectedTask(viewModel.OperatorNoiseLevelMeasured, rotation.OperatorTask);
            var selectedTaskAssistant = CreateSelectedTask(viewModel.AssistantNoiseLevelMeasured, rotation.AssistantTask);
            
            var noiseProtectionAssistant = _noiseProtectionDAO.Get(selectedTaskAssistant.NoiseProtectionId);
            var noiseProtectionOperator = _noiseProtectionDAO.Get(selectedTaskOperator.NoiseProtectionId);
            
            var timeSpan = new TimeSpan(CreateTimeSpan(viewModel.Hours, viewModel.Minutes).Ticks / 2);

            var percentageOperator = (int)Math.Round(rotation.OperatorTask.CalculatePercentage(selectedTaskOperator.Task.TaskDefinition.RoleType.ToString(), selectedTaskOperator.NoiseLevel, selectedTaskOperator.ButtonPressed, selectedTaskOperator.BackgroundNoise, selectedTaskOperator.Task.Frequency, noiseProtectionOperator, timeSpan));
            var percentageAssistant = (int)Math.Round(rotation.AssistantTask.CalculatePercentage(selectedTaskAssistant.Task.TaskDefinition.RoleType.ToString(), selectedTaskAssistant.NoiseLevel, selectedTaskAssistant.ButtonPressed, selectedTaskAssistant.BackgroundNoise, selectedTaskOperator.Task.Frequency, noiseProtectionAssistant, timeSpan));
                
            selectedTaskOperator.AddWorkTime(timeSpan, percentageOperator);
            selectedTaskAssistant.AddWorkTime(timeSpan, percentageAssistant);
            
            _selectedTaskDAO.Store(selectedTaskOperator);
            _selectedTaskDAO.Store(selectedTaskAssistant);

            var selectedTaskRotationViewModel =
                new SelectedTasksRotationViewModel
                {
                    OperatorSelectedTaskViewModel = new SelectedTaskViewModel(selectedTaskOperator),
                    AssistantSelectedTaskViewModel = new SelectedTaskViewModel(selectedTaskAssistant)
                };

            return PartialView("_SelectedTasksRotation", selectedTaskRotationViewModel);
        }


        private TimeSpan CreateTimeSpan(string hoursString, string minutesString)
        {
            var hours = string.IsNullOrEmpty(hoursString) ? 0 : int.Parse(hoursString);
            var minutes = string.IsNullOrEmpty(minutesString) ? 0 : int.Parse(minutesString);
            return new TimeSpan(0, hours, minutes, 0);
        }


        private SelectedTask CreateSelectedTask(decimal noiseLevelMeasured, Task task)
        {
            var selectedTask = new SelectedTask
            {
                Title = task.Title,
                Role = task.Role.Title,
                NoiseProtection = task.NoiseProtection.Title,
                Task = task,
                CreatedBy = string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name,
                CreatedDate = DateTime.Now.Date,
                NoiseProtectionId = task.NoiseProtection.Id
            };

            if (noiseLevelMeasured > task.NoiseLevelGuideline)
            {
                selectedTask.NoiseLevel = noiseLevelMeasured;
                selectedTask.IsNoiseMeassured = true;
            }
            else
            {
                selectedTask.NoiseLevel = task.NoiseLevelGuideline;
                selectedTask.IsNoiseMeassured = false;
            }

            return selectedTask;
        }


        private ValidationErrorSummaryViewModel ValidateInput(RotationViewModel viewModel, Rotation rotation)
        {
            var errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (viewModel.OperatorNoiseLevelMeasured - rotation.OperatorTask.NoiseLevelGuideline > 6)
                errorSummaryViewModel.ValidationErrors.Add(
                    TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineOperator);

            if (viewModel.AssistantNoiseLevelMeasured - rotation.AssistantTask.NoiseLevelGuideline > 6)
                errorSummaryViewModel.ValidationErrors.Add(
                    TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineAssistant);

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorWorkTimeRequiredRotation);

            var timeSpan = CreateTimeSpan(viewModel.Hours, viewModel.Minutes);

            if (timeSpan.TotalHours > 6)
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorRotationMaximum6Hours);

            return errorSummaryViewModel;
        }
    }
}
