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
        
        public RotationController(ITaskDAO taskDAO, IRotationDAO rotationDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _rotationDAO = rotationDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }

        public PartialViewResult AddTaskRotation(int id)
        {
            Task task = _taskDAO.Get(id);
            Rotation rotation = _rotationDAO.GetByTaskId(id);

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

            SelectedTask selectedTaskOperator = CreateSelectedTask(viewModel.OperatorNoiseLevelMeasured, rotation.OperatorTask);
            SelectedTask selectedTaskAssistant = CreateSelectedTask(viewModel.AssistantNoiseLevelMeasured, rotation.AssistantTask);



            TimeSpan timeSpan = new TimeSpan(CreateTimeSpan(viewModel.Hours, viewModel.Minutes).Ticks / 2);
                
            int percentageOperator = (int)rotation.OperatorTask.CalculatePercentage(selectedTaskOperator.NoiseLevel, timeSpan);
            int percentageAssistant = (int)rotation.AssistantTask.CalculatePercentage(selectedTaskAssistant.NoiseLevel, timeSpan);
                
            selectedTaskOperator.AddWorkTime(timeSpan, percentageOperator);
            selectedTaskAssistant.AddWorkTime(timeSpan, percentageAssistant);



            _selectedTaskDAO.Store(selectedTaskOperator);
            _selectedTaskDAO.Store(selectedTaskAssistant);

            SelectedTasksRotationViewModel selectedTaskRotationViewModel =
                new SelectedTasksRotationViewModel
                {
                    OperatorSelectedTaskViewModel = new SelectedTaskViewModel(selectedTaskOperator),
                    AssistantSelectedTaskViewModel = new SelectedTaskViewModel(selectedTaskAssistant)
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
                CreatedBy = string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name,
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

            if (viewModel.OperatorNoiseLevelMeasured - rotation.OperatorTask.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineOperator);
            }

            if (viewModel.AssistantNoiseLevelMeasured - rotation.AssistantTask.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseLevelToHighAboveGuidlineAssistant);
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorWorkTimeRequiredRotation);
            }

            TimeSpan timeSpan = CreateTimeSpan(viewModel.Hours, viewModel.Minutes);
            if(timeSpan.TotalHours > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorRotationMaximum6Hours);
            }

            return errorSummaryViewModel;
        }
    }
}
