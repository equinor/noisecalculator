using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.UI.Web.Controllers
{
    public class RegularController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";
        private const string InputNotChecked = "";

        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IRoleDAO _roleDAO;


        public RegularController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO, IRoleDAO roleDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _roleDAO = roleDAO;
        }

        //public PartialViewResult AddTaskRegular(Task task)
        public PartialViewResult AddTaskRegular(int taskId)
        {
            Task task = _taskDAO.Get(taskId);

            RegularViewModel viewModel = new RegularViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = RoleTypeEnum.Regular.ToString(),
                NoiseLevelGuideline = task.NoiseLevelGuideline.ToString(),
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked
            };

            if (task.Role.RoleType == RoleTypeEnum.Rotation)
            {
                // Override the style="display: none;" hiding of rotation HTML elements
                viewModel.RotationDiplayStyle = string.Empty;
                viewModel.RoleType = RoleTypeEnum.Rotation.ToString();

                return PartialView("_CreateRotationTask", viewModel);
            }

            return PartialView("_CreateRegularTask", viewModel);
        }


        private ValidationErrorSummaryViewModel ValidateInput(RegularViewModel viewModel, Task task)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (!string.IsNullOrEmpty(viewModel.NoiseLevelMeassured) && int.Parse(viewModel.NoiseLevelMeassured) - task.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add("Calculation is invalid for noise levels that are 7 dBA or more above the guideline.");
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes) && string.IsNullOrEmpty(viewModel.Percentage))
            {
                errorSummaryViewModel.ValidationErrors.Add("Task work time must be specified, either as hours/minutes or percentage of daily dosage");
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
                TaskId = task.Id,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now.Date
            };

            int noiseLevelMeasured = string.IsNullOrEmpty(viewModel.NoiseLevelMeassured) ? 0 : int.Parse(viewModel.NoiseLevelMeassured);
            if (noiseLevelMeasured > task.NoiseLevelGuideline)
            {
                selectedTask.NoiseLevel = noiseLevelMeasured;
            }
            else
            {
                selectedTask.NoiseLevel = task.NoiseLevelGuideline;
            }

            if (string.IsNullOrEmpty(viewModel.Percentage))
            {
                selectedTask.Hours = string.IsNullOrEmpty(viewModel.Hours) ? 0 : int.Parse(viewModel.Hours);
                selectedTask.Minutes = string.IsNullOrEmpty(viewModel.Minutes) ? 0 : int.Parse(viewModel.Minutes);
                selectedTask.Percentage = (int)task.CalculatePercentage(noiseLevelMeasured, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0));
            }
            else
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = task.CalculateTimeSpan(noiseLevelMeasured, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }

            return selectedTask;
        }

        [HttpPost]
        public PartialViewResult AddTaskRegular(RegularViewModel viewModel)
        {
            Task task = _taskDAO.Get(viewModel.TaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            SelectedTask selectedTask = CreateSelectedTaskRegular(viewModel, task);
            _selectedTaskDAO.Store(selectedTask);

            SelectedTaskViewModel selectedTaskViewModel = CreateViewModel(selectedTask);
            return PartialView("_SelectedTask", selectedTaskViewModel);
        }


        //public PartialViewResult EditTaskRegular(SelectedTask selectedTask)
        public PartialViewResult EditTaskRegular(int selectedTaskId)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(selectedTaskId);
            Task task = _taskDAO.Get(selectedTask.TaskId);

            bool noiseLevelIsMeassured = (selectedTask.NoiseLevel != task.NoiseLevelGuideline);
            bool workIsEnteredAsTime = (selectedTask.Hours > 0 || selectedTask.Minutes > 0);

            RegularViewModel viewModel = new RegularViewModel
            {
                TaskId = task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = RoleTypeEnum.Regular.ToString(),
                NoiseLevelGuideline = task.NoiseLevelGuideline.ToString(),
                NoiseLevelMeassured = selectedTask.NoiseLevel.ToString(),
                RadioNoiseMeassuredNoCheckedAttr = noiseLevelIsMeassured ? InputNotChecked : InputChecked,
                RadioNoiseMeassuredYesCheckedAttr = noiseLevelIsMeassured ? InputChecked : InputNotChecked,
                RadioTimeCheckedAttr = workIsEnteredAsTime ? InputChecked : InputNotChecked,
                RadioPercentageCheckedAttr = workIsEnteredAsTime ? InputNotChecked : InputChecked,
                Hours = selectedTask.Hours.ToString(),
                Minutes = selectedTask.Minutes.ToString(),
                Percentage = selectedTask.Percentage.ToString()
            };

            return PartialView("_EditRegularTask", viewModel);
        }


        [HttpPost]
        public PartialViewResult EditTaskRegular(int id, RegularViewModel viewModel)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            Task task = _taskDAO.Get(selectedTask.TaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            int noiseLevel = string.IsNullOrEmpty(viewModel.NoiseLevelMeassured) ? 0 : int.Parse(viewModel.NoiseLevelMeassured);

            selectedTask.NoiseLevel = noiseLevel;
            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = task.CalculateTimeSpan(noiseLevel, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }
            else
            {
                selectedTask.Hours = string.IsNullOrEmpty(viewModel.Hours) ? 0 : int.Parse(viewModel.Hours);
                selectedTask.Minutes = string.IsNullOrEmpty(viewModel.Minutes) ? 0 : int.Parse(viewModel.Minutes);
                selectedTask.Percentage = (int)task.CalculatePercentage(noiseLevel, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0));
            }

            _selectedTaskDAO.Store(selectedTask);

            SelectedTaskViewModel selectedTaskViewModel = CreateViewModel(selectedTask);

            return PartialView("_SelectedTask", selectedTaskViewModel);
        }


        [HttpPost]
        public PartialViewResult AddTaskRotation(RegularViewModel viewModel)
        {
            Task task = _taskDAO.Get(viewModel.TaskId);

            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            SelectedTask selectedTaskOperator = CreateSelectedTaskRegular(viewModel, task);
            selectedTaskOperator.Role = _roleDAO.Get("Operator").Title;

            SelectedTask selectedTaskAssistant = CreateSelectedTaskRegular(viewModel, task);
            selectedTaskAssistant.Role = _roleDAO.Get("Assistant").Title;

            _selectedTaskDAO.Store(selectedTaskAssistant);
            _selectedTaskDAO.Store(selectedTaskOperator);

            SelectedTasksRotationViewModel selectedTaskRotationViewModel =
                new SelectedTasksRotationViewModel
                {
                    OperatorSelectedTaskViewModel = CreateViewModel(selectedTaskOperator),
                    AssistantSelectedTaskViewModel = CreateViewModel(selectedTaskAssistant)
                };

            return PartialView("_SelectedTasksRotation", selectedTaskRotationViewModel);
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

        public JsonResult GetTotalPercentage()
        {
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            TotalNoiseDosageViewModel totalNoiseDosage = new TotalNoiseDosageViewModel();
            totalNoiseDosage.Percentage = selectedTasks.Sum(x => x.Percentage);

            if (totalNoiseDosage.Percentage < 75)
            {
                totalNoiseDosage.StatusText = "Noise level is considered safe";
                totalNoiseDosage.CssClass = "noiseLevelNormal";
            }
            else if (totalNoiseDosage.Percentage >= 75 && totalNoiseDosage.Percentage < 100)
            {
                totalNoiseDosage.StatusText = "Noise level is approaching allowed limits";
                totalNoiseDosage.CssClass = "noiseLevelWarning";
            }
            else if (totalNoiseDosage.Percentage >= 100)
            {
                totalNoiseDosage.StatusText = "Unsafe daily noise dosage";
                totalNoiseDosage.CssClass = "noiseLevelCritical";
            }

            return Json(totalNoiseDosage, JsonRequestBehavior.AllowGet);
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
    }
}
