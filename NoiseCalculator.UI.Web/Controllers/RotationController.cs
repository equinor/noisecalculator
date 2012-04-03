using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
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
        
        public RotationController(ITaskDAO taskDAO, IRotationDAO rotationDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _rotationDAO = rotationDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }

        public PartialViewResult AddTaskRotation(int taskId)
        {
            Task task = _taskDAO.Get(taskId);
            Rotation rotation = _rotationDAO.GetByTaskId(taskId);

            RotationViewModel viewModel = new RotationViewModel()
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
            Rotation rotation = _rotationDAO.Get(viewModel.RotationId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, rotation);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            int operatorNoiseLevelMeasured = string.IsNullOrEmpty(viewModel.OperatorNoiseLevelMeasured) ? 0 : int.Parse(viewModel.OperatorNoiseLevelMeasured);
            int assistantNoiseLevelMeasured = string.IsNullOrEmpty(viewModel.AssistantNoiseLevelMeasured) ? 0 : int.Parse(viewModel.AssistantNoiseLevelMeasured);            

            SelectedTask selectedTaskOperator = CreateSelectedTask(operatorNoiseLevelMeasured, rotation.OperatorTask);
            SelectedTask selectedTaskAssistant = CreateSelectedTask(assistantNoiseLevelMeasured, rotation.AssistantTask);

            if(string.IsNullOrEmpty(viewModel.Percentage))
            {
                TimeSpan timeSpan = CreateTimeSpan(viewModel.Hours, viewModel.Minutes);
                int percentage = (int)rotation.OperatorTask.CalculatePercentage(operatorNoiseLevelMeasured, timeSpan);
                selectedTaskOperator.AddWorkTime(timeSpan, percentage);
                selectedTaskAssistant.AddWorkTime(timeSpan, percentage);
            }
            else
            {
                int percentage = int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = rotation.OperatorTask.CalculateTimeSpan(operatorNoiseLevelMeasured, percentage);
                selectedTaskOperator.AddWorkTime(timeSpan, percentage);
                selectedTaskAssistant.AddWorkTime(timeSpan, percentage);
            }

            _selectedTaskDAO.Store(selectedTaskOperator);
            _selectedTaskDAO.Store(selectedTaskAssistant);

            SelectedTasksRotationViewModel selectedTaskRotationViewModel =
                new SelectedTasksRotationViewModel
                {
                    OperatorSelectedTaskViewModel = CreateViewModel(selectedTaskOperator),
                    AssistantSelectedTaskViewModel = CreateViewModel(selectedTaskAssistant)
                };

            return PartialView("_SelectedTasksRotation", selectedTaskRotationViewModel);
        }


        private TimeSpan CreateTimeSpan(string hoursString, string minutesString)
        {
            int hours = string.IsNullOrEmpty(hoursString) ? 0 : int.Parse(hoursString);
            int minutes = string.IsNullOrEmpty(minutesString) ? 0 : int.Parse(minutesString);
            return new TimeSpan(0, hours, minutes, 0);
        }


        private SelectedTask CreateSelectedTask(int noiseLevelMeasured, Task task)
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
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (!string.IsNullOrEmpty(viewModel.OperatorNoiseLevelMeasured) && int.Parse(viewModel.OperatorNoiseLevelMeasured) - rotation.OperatorTask.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineOperator);
            }

            if (!string.IsNullOrEmpty(viewModel.AssistantNoiseLevelMeasured) && int.Parse(viewModel.AssistantNoiseLevelMeasured) - rotation.AssistantTask.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineAssistant);
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes) && string.IsNullOrEmpty(viewModel.Percentage))
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorWorkTimeRequired);
            }

            return errorSummaryViewModel;
        }


        // DRY this up... TaskController has similar code...
        private SelectedTaskViewModel CreateViewModel(SelectedTask selectedTask)
        {
            SelectedTaskViewModel viewModel = new SelectedTaskViewModel
            {
                Id = selectedTask.Id,
                Title = selectedTask.Title,
                Role = selectedTask.Role,
                NoiseProtection = selectedTask.NoiseProtection,
                TaskId = selectedTask.Task.Id,
                Percentage = selectedTask.Percentage.ToString(CultureInfo.InvariantCulture),
                Hours = selectedTask.Hours.ToString(CultureInfo.InvariantCulture),
                Minutes = selectedTask.Minutes.ToString(CultureInfo.InvariantCulture)
            };

            if (selectedTask.IsNoiseMeassured)
            {
                viewModel.NoiseLevel = string.Format("{0} dBA {1}", selectedTask.NoiseLevel, TaskResources.SelectedTaskNoiseMeasured);
            }
            else
            {
                viewModel.NoiseLevel = string.Format("{0} dBA", selectedTask.NoiseLevel.ToString(CultureInfo.InvariantCulture));
            }

            return viewModel;
        }
    }
}
