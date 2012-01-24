using System;
using System.Collections.Generic;
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
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            return View(selectedTasks);
        }


        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAllOrdered();
            return PartialView("_TaskDialog", tasks);
        }


        public ActionResult GetFormForTask(int id)
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

        // ------------------------------------------------
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

            return PartialView("_TaskFormRegular", viewModel);
        }



        private ValidationErrorSummaryViewModel ValidateInput(HelideckViewModel viewModel, Task task)
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

            int noiseLevelMeasured = (string.IsNullOrEmpty(viewModel.NoiseLevelMeassured)) ? 0 : int.Parse(viewModel.NoiseLevelMeassured);
            if( noiseLevelMeasured > task.NoiseLevelGuideline)
            {
                selectedTask.NoiseLevel = noiseLevelMeasured;
            }
            else
            {
                selectedTask.NoiseLevel = task.NoiseLevelGuideline;
            }

            if (string.IsNullOrEmpty(viewModel.Percentage))
            {
                // Calculate percentage from time
                int hours = (string.IsNullOrEmpty(viewModel.Hours)) ? 0 : int.Parse(viewModel.Hours);
                int minutes = (string.IsNullOrEmpty(viewModel.Minutes)) ? 0 : int.Parse(viewModel.Minutes);;
                
                TimeSpan actualExposure = new TimeSpan(0, hours, minutes, 0);
                decimal tempPercentage = task.CalculatePercentage(noiseLevelMeasured, actualExposure);
                
                selectedTask.Percentage = (int) Math.Round(tempPercentage);
                selectedTask.Hours = hours;
                selectedTask.Minutes = minutes;
            }
            else
            {
                // Calculate time from percentage
                int percentage = (string.IsNullOrEmpty(viewModel.Percentage)) ? 0 : int.Parse(viewModel.Percentage);
                TimeSpan actualExposure = task.CalculateTimeSpan(noiseLevelMeasured, percentage);

                selectedTask.Percentage = percentage;
                selectedTask.Hours = actualExposure.Hours;
                selectedTask.Minutes = actualExposure.Minutes;
            }

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", selectedTask);
        }



        public PartialViewResult AddTaskHelideck(Task task)
        {
            HelideckViewModel viewModel = new HelideckViewModel
                                              {
                                                  TaskId = task.Id,
                                                  Title = task.Title,
                                                  Role = task.Role.Title,
                                                  RoleType = RoleTypeEnum.Helideck.ToString(),
                                              };

            viewModel.Helicopters.Add(new SelectListItem{Text = "-- Select One --", Value = "0"} );
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectListItem { Text = type.Title, Value = type.Id.ToString() });
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterNoiseProtection noiseProtection in _helicopterNoiseProtectionDAO.GetAll())
            {
                viewModel.NoiseProtection.Add(new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() });
            }

            viewModel.WorkIntervals.Add(new SelectListItem { Text = "-- Select One --", Value = "0" });
            foreach (HelicopterWorkInterval workInterval in _helicopterWorkIntervalDAO.GetAll())
            {
                viewModel.WorkIntervals.Add(new SelectListItem { Text = workInterval.Title, Value = workInterval.Id.ToString() });
            }

            return PartialView("_TaskFormHelideck", viewModel);
        }


        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModel)
        {
            Task task = _taskDAO.Get(viewModel.TaskId);
            
            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(viewModel, task);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }
            
            //if(viewModel.HelicopterId == 0 || viewModel.NoiseProtectionId == 0 || viewModel.WorkIntervalId == 0)
            //{
            //    Response.StatusCode = 500;
            //    return Json("Helicopter data is missing"); // TRANSLATION MUST BE ADDED
            //}

            
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, viewModel.NoiseProtectionId, viewModel.WorkIntervalId);

            char[] splitters = new char[] {' ', '-'};
            string[] minuteElements = helicopterTask.HelicopterWorkInterval.Title.Split(splitters);


            SelectedTask selectedTask = new SelectedTask
                                            {
                                                Title = string.Format("{0} - {1}", task.Title, helicopterTask.HelicopterType.Title),
                                                Role = task.Role.Title,
                                                NoiseProtection = helicopterTask.HelicopterNoiseProtection.Title,
                                                Percentage = helicopterTask.Percentage,
                                                Minutes = int.Parse(minuteElements[1]),
                                                TaskId = task.Id,
                                                HelicopterTaskId = helicopterTask.Id,
                                                CreatedBy = User.Identity.Name,
                                                CreatedDate = DateTime.Now.Date
                                            };

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", selectedTask);
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
    }
}
