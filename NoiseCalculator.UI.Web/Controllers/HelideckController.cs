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
    public class HelideckController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly INoiseProtectionDAO _noiseProtectionDAO;

        
        public HelideckController(ITaskDAO taskDAO, 
                                ISelectedTaskDAO selectedTaskDAO, 
                                IHelicopterTaskDAO helicopterTaskDAO,
                                IDAO<HelicopterType, int> helicopterTypeDAO,
                                INoiseProtectionDAO noiseProtectionDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _noiseProtectionDAO = noiseProtectionDAO;
        }


        public PartialViewResult AddTaskHelideck(int id)
        {
            var task = _taskDAO.Get(id);

            var viewModel = new HelideckViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = RoleTypeEnum.Helideck.ToString(),
                NoiseLevel = task.NoiseLevelGuideline,
                NoiseProtectionId = task.NoiseProtection.Id,
                NoiseProtectionDefinitionId = task.NoiseProtection.NoiseProtectionDefinition.Id
            };

            AppendHelideckMasterData(viewModel);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateHelideckTask", viewModel);
        }

        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModel)
        {
            var task = _taskDAO.Get(viewModel.TaskId);

            var validationViewModel = ValidateInput(viewModel);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            var noiseProtection = _noiseProtectionDAO.Get(viewModel.NoiseProtectionId);
            var helicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, task.Id);
            
            var selectedTask = new SelectedTask
            {
                NoiseLevel = task.NoiseLevelGuideline,
                ButtonPressed = task.ButtonPressed,
                NoiseProtectionId = viewModel.NoiseProtectionId,
                Title = string.Format("{0} - {1}", task.Title, helicopterTask.HelicopterType.Title),
                Role = task.Role.Title,
                NoiseProtection = noiseProtection.Title,
                Percentage = (int)CalculatePercentage(helicopterTask.NoiseLevel, task.ButtonPressed, 0, noiseProtection, new TimeSpan(0, 0, task.AllowedExposureMinutes, 0)),
                Minutes = task.AllowedExposureMinutes,
                Task = task,
                HelicopterTaskId = helicopterTask.Id,
                CreatedBy = string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name,
                CreatedDate = DateTime.Now.Date,
                IsNoiseMeassured = false
            };

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }


        public virtual decimal CalculatePercentage(decimal actualNoiseLevel, int buttonPressed, int backgroundNoise, NoiseProtection noiseProtection, TimeSpan actualExposure)
        {
            var noiseProtectionDampening = noiseProtection.NoiseDampening;
            const double timeInFullShift = 720;
            if (backgroundNoise == 0)
                backgroundNoise = 80;

            // Støynivå => 10* LOG(10^(støydef/10) + 10^(bakgrunnsstøy/10)
            var noiseLevel = 10 *
                             Math.Log((Math.Pow(10, ((double)actualNoiseLevel / 10)) +
                                      Math.Pow(10, ((double)backgroundNoise / 10))), 10.0);

            // Norm verdi => 10 * LOG (10^(støynivå/10)) * knappen inne / 100)
            var normalizedValue = 10 * Math.Log((Math.Pow(10, (noiseLevel / 10)) * ((double)buttonPressed / 100)) + Math.Pow(10, ((double)backgroundNoise / 10)) * (((100 - (double)buttonPressed) / 100)), 10.0);

            // Norm verdi med hørselsvern
            var normValueWithNoiseProtection = normalizedValue - noiseProtectionDampening;

            var percentMinutes = actualExposure.TotalMinutes / timeInFullShift;

            // Eksponering i db => 10 * LOG (Time in minutes / Time in full shift * 10 ^ (Noise - noiseprotection / 10)
            var exposure = 10 *
                           Math.Log((percentMinutes *
                                    Math.Pow(10, (normValueWithNoiseProtection / 10))), 10.0);

            // % beregning => 10^(exposure/10))/(10^(80/10))*100)
            var calcPerc = (Math.Pow(10, (exposure / 10))) / (Math.Pow(10, (80 / 10))) * 100;

            return (decimal)calcPerc;
        }
        public PartialViewResult EditTaskHelideck(int id)
        {
            var selectedTask = _selectedTaskDAO.Get(id);
            
            var helicopterTask = _helicopterTaskDAO.Get(selectedTask.HelicopterTaskId);

            var viewModel = new HelideckViewModel
            {
                NoiseProtectionId = selectedTask.NoiseProtectionId,
                TaskId = selectedTask.Task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = selectedTask.Task.Title,
                Role = selectedTask.Task.Role.Title,
                RoleType = RoleTypeEnum.Helideck.ToString(),
                HelicopterId = helicopterTask.HelicopterType.Id,
                NoiseLevel = helicopterTask.NoiseLevel
            };

            viewModel.Helicopters.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var type in _helicopterTypeDAO.GetAll())
            {
                var selectListItem = new SelectListItem { Text = type.Title, Value = type.Id.ToString() };
                if (viewModel.HelicopterId == type.Id)
                    selectListItem.Selected = true;

                viewModel.Helicopters.Add(selectListItem);
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                var selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() };
                if (viewModel.NoiseProtectionId == noiseProtection.Id)
                    selectListItem.Selected = true;

                viewModel.NoiseProtection.Add(selectListItem);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditHelideckTask", viewModel);
        }

        [HttpPost]
        public PartialViewResult EditTaskHelideck(int id, HelideckViewModel viewModel)
        {
            var selectedTask = _selectedTaskDAO.Get(id);
            var helicopterTask = _helicopterTaskDAO.Get(selectedTask.HelicopterTaskId);

            var validationViewModel = ValidateInput(viewModel);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            var noiseProtection = _noiseProtectionDAO.Get(selectedTask.NoiseProtectionId);
            
            var taskValuesHaveBeenChanged = (viewModel.HelicopterId != helicopterTask.HelicopterType.Id
                                                || (viewModel.NoiseProtectionId != noiseProtection.Id));

            if (taskValuesHaveBeenChanged)
            {
                var newNoiseProtection = _noiseProtectionDAO.Get(viewModel.NoiseProtectionId);
                var newHelicopterTask = _helicopterTaskDAO.Get(viewModel.HelicopterId, selectedTask.Task.Id);

                selectedTask.Title = string.Format("{0} - {1}", selectedTask.Task.Title, newHelicopterTask.HelicopterType.Title);
                selectedTask.NoiseProtection = newNoiseProtection.Title;
                selectedTask.Percentage =
                    (int)
                        CalculatePercentage(newHelicopterTask.NoiseLevel, selectedTask.ButtonPressed, 80,
                            newNoiseProtection, new TimeSpan(0, 0, selectedTask.Minutes, 0));
                selectedTask.Minutes = selectedTask.Minutes;
                selectedTask.HelicopterTaskId = newHelicopterTask.Id;
                selectedTask.NoiseProtectionId = newNoiseProtection.Id;

                _selectedTaskDAO.Store(selectedTask);
            }

            return PartialView("_SelectedTask", new SelectedTaskViewModel(selectedTask));
        }


        private ValidationErrorSummaryViewModel ValidateInput(HelideckViewModel viewModel)
        {
            var errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (viewModel.HelicopterId == 0)
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterTypeRequired);

            if (viewModel.NoiseProtectionId == 0)
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);

            return errorSummaryViewModel;
        }


        private void AppendHelideckMasterData(HelideckViewModel viewModel)
        {
            viewModel.Helicopters.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var type in _helicopterTypeDAO.GetAll())
            {
                var selectListItem = new SelectListItem { Text = type.Title, Value = type.Id.ToString() };
                if (viewModel.HelicopterId == type.Id)
                    selectListItem.Selected = true;

                viewModel.Helicopters.Add(selectListItem);
            }

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                var selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString() };
                if (viewModel.NoiseProtectionDefinitionId == noiseProtection.NoiseProtectionDefinition.Id)
                    selectListItem.Selected = true;

                viewModel.NoiseProtection.Add(selectListItem);
            }

        }
    }
}
