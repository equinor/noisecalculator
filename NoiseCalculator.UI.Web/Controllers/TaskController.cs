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
    public class TaskController : Controller
    {
        private const string InputChecked = "checked=\"checked\"";
        private const string InputNotChecked = "";
        
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly IDAO<HelicopterNoiseProtection, int> _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;


        public TaskController(ITaskDAO taskDAO, 
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
        

        public ActionResult Index()
        {
            IList<SelectedTaskViewModel> selectedTasks = new List<SelectedTaskViewModel>();
            foreach (SelectedTask selectedTask in _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now))
            {
                selectedTasks.Add(CreateViewModel(selectedTask));
            }

            return View(selectedTasks);
        }


        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAllOrdered();
            return PartialView("_TaskDialog", tasks);
        }


        public ActionResult GetCreateFormForTask(int id)
        {
            Task task = _taskDAO.Get(id);

            switch (task.Role.Title)
            {
                case "Helideck":
                        return AddTaskHelideck(task);
                case "Rotation":
                        return AddTaskRotation(task);
                default:
                        return AddTaskRegular(task);
            }
        }

        public ActionResult GetEditFormForSelectedTask(int id)
        {
            SelectedTask selectedTask = _selectedTaskDAO.Get(id);
            Task task = _taskDAO.Get(selectedTask.TaskId);

            switch (task.Role.Title)
            {
                case "Helideck":
                    return EditTaskHelideck(selectedTask);
                    //break;
                case "Rotation":
                    //    return AddTaskRotation(selectedTask);
                    //break;
                default:
                    return EditTaskRegular(selectedTask);
            }
        }

        // ---------------------------------------------------------------------------------
        public PartialViewResult AddTaskRegular(Task task)
        {
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

            return PartialView("_CreateRegularTask", viewModel);
        }


        private ValidationErrorSummaryViewModel ValidateInput(HelideckViewModel viewModel)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();
            
            if(viewModel.HelicopterId == 0 || viewModel.NoiseProtectionId == 0 || viewModel.WorkIntervalId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add("Helicopter, noise protection and work interval must be selected to add the task.");
            }

            return errorSummaryViewModel;
        }



        private ValidationErrorSummaryViewModel ValidateInput(RegularViewModel viewModel, Task task)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();
            
            if(!string.IsNullOrEmpty(viewModel.NoiseLevelMeassured) && int.Parse(viewModel.NoiseLevelMeassured) - task.NoiseLevelGuideline > 6)
            {
                errorSummaryViewModel.ValidationErrors.Add("Calculation is invalid for noise levels that are 7 dBA or more above the guideline.");
            }

            if( string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes) && string.IsNullOrEmpty(viewModel.Percentage) )
            {
                errorSummaryViewModel.ValidationErrors.Add("Task work time must be specified, either as hours/minutes or percentage of daily dosage");
            }

            return errorSummaryViewModel;
        }


        [HttpPost]
        public PartialViewResult AddTaskRegular(RegularViewModel viewModel)
        {
            Task task = _taskDAO.Get(viewModel.TaskId);
            
            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if(validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }
            
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
                selectedTask.Percentage = (int) task.CalculatePercentage(noiseLevelMeasured, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0));
            }
            else
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan timeSpan = task.CalculateTimeSpan(noiseLevelMeasured, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
            }

            _selectedTaskDAO.Store(selectedTask);

            SelectedTaskViewModel selectedTaskViewModel = CreateViewModel(selectedTask);

            return PartialView("_SelectedTask", selectedTaskViewModel);
        }


        public PartialViewResult EditTaskRegular(SelectedTask selectedTask)
        {
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
            if(string.IsNullOrEmpty(viewModel.Hours ) && string.IsNullOrEmpty(viewModel.Minutes))
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

        public PartialViewResult EditTaskHelideck(SelectedTask selectedTask)
        {
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

            bool taskValuesHaveBeenChanged = (  viewModel.HelicopterId != helicopterTask.HelicopterType.Id
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



        public PartialViewResult AddTaskHelideck(Task task)
        {
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


        public PartialViewResult AddTaskRotation(Task task)
        {
            return PartialView("_TaskFormRotation");
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
            
            if(totalNoiseDosage.Percentage < 75)
            {
                totalNoiseDosage.StatusText = "Noise level is considered safe";
                totalNoiseDosage.CssClass = "noiseLevelNormal";
            }
            else if(totalNoiseDosage.Percentage >= 75 && totalNoiseDosage.Percentage < 100)
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
            
            if(task.Role.RoleType == RoleTypeEnum.Regular)
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
