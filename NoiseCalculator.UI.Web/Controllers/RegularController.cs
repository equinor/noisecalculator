﻿using System;
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

            var noiseLevelGuideLine = task.NoiseLevelGuideline.ToString(CultureInfo.InvariantCulture);

            if (noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal) > 0)
                noiseLevelGuideLine = noiseLevelGuideLine.Substring(0, noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal));

            if (task.SortOrder >= 1001)
                return null;

            var viewModel = new RegularViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                RoleType = task.Role.RoleType.ToString(),
                NoiseLevelGuideline = noiseLevelGuideLine,
                NoiseProtectionId = task.NoiseProtection.Id,
                RadioNoiseMeassuredNoCheckedAttr = InputChecked,
                RadioTimeCheckedAttr = InputChecked,
                ButtonPressed = task.ButtonPressed,
                NoiseProtectionDefinitionId = task.NoiseProtection.NoiseProtectionDefinition.Id
            };

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                var selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString(CultureInfo.InvariantCulture) };

                if (viewModel.NoiseProtectionDefinitionId == noiseProtection.NoiseProtectionDefinition.Id)
                    selectListItem.Selected = true;

                if (task.Role.RoleType.ToString() == "AreaNoise")
                    if (!selectListItem.Selected)
                        if (task.Title.Contains("80") || task.Title.Contains("85") || task.Title.Contains("90"))
                        {
                            if (noiseProtection.NoiseProtectionDefinition.Id != 3) // Quietpro
                                continue;
                        }
                        else
                        {
                            if (noiseProtection.NoiseProtectionDefinition.Id != 3 && noiseProtection.NoiseProtectionDefinition.Id != 2) // Quietpro and double hearing
                                continue;
                        }

                if (task.TaskDefinition.Id != 1070 && //"Ultra-High Pressure (UHP) water jetting"
                    task.TaskDefinition.Id != 1071 && //"Sponging"
                    task.TaskDefinition.Id != 1074 && //"Sandblasting"
                    task.TaskDefinition.Id != 1073) //"Slurry blasting"

                    if (noiseProtection.NoiseProtectionDefinition.Id == 6 || noiseProtection.NoiseProtectionDefinition.Id == 7)
                        // Skumpropp tas ikke med i vanlige operasjoner

                        continue;

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

            var noiseLevelGuideLine = selectedTask.Task.NoiseLevelGuideline.ToString(CultureInfo.InvariantCulture);

            if (noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal) > 0)
                noiseLevelGuideLine = noiseLevelGuideLine.Substring(0, noiseLevelGuideLine.IndexOf(".", StringComparison.Ordinal));


            var viewModel = new RegularViewModel
            {
                TaskId = selectedTask.Task.Id,
                SelectedTaskId = selectedTask.Id,
                Title = selectedTask.Task.Title,
                Role = selectedTask.Task.Role.Title,
                RoleType = selectedTask.Task.Role.RoleType.ToString(),
                NoiseLevelGuideline = noiseLevelGuideLine,
                NoiseLevelMeassured = selectedTask.NoiseLevel,
                NoiseProtectionId = selectedTask.NoiseProtectionId,
                RadioNoiseMeassuredNoCheckedAttr = selectedTask.IsNoiseMeassured ? InputNotChecked : InputChecked,
                RadioNoiseMeassuredYesCheckedAttr = selectedTask.IsNoiseMeassured ? InputChecked : InputNotChecked,
                RadioTimeCheckedAttr = selectedTask.UsePercentage ? InputNotChecked : InputChecked,
                RadioPercentageCheckedAttr = selectedTask.UsePercentage ? InputChecked : InputNotChecked,
                ButtonPressed = selectedTask.ButtonPressed,
                BackgroundNoise = selectedTask.BackgroundNoise == 0 ? "<80" : selectedTask.BackgroundNoise.ToString(CultureInfo.InvariantCulture),
                Hours = selectedTask.UsePercentage ? "" : selectedTask.Hours.ToString(CultureInfo.InvariantCulture),
                Minutes = selectedTask.UsePercentage ? "" : selectedTask.Minutes.ToString(CultureInfo.InvariantCulture),
                Percentage = selectedTask.UsePercentage ? selectedTask.Percentage.ToString(CultureInfo.InvariantCulture) : ""
            };

            viewModel.NoiseProtection.Add(new SelectListItem { Text = TaskResources.SelectOne, Value = "0" });
            foreach (var noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                var selectListItem = new SelectListItem { Text = noiseProtection.Title, Value = noiseProtection.Id.ToString(CultureInfo.InvariantCulture) };
                if (viewModel.NoiseProtectionId == noiseProtection.Id)
                {
                    selectListItem.Selected = true;
                }
                if (selectedTask.Task.Role.RoleType.ToString() == "AreaNoise")
                    if (!selectListItem.Selected)
                    {
                        if (selectedTask.Task.Title.Contains("80") || selectedTask.Task.Title.Contains("85") ||
                            selectedTask.Task.Title.Contains("90"))
                        {
                            if (noiseProtection.NoiseProtectionDefinition.Id != 3 &&
                                selectedTask.Task.NoiseProtection.Id != noiseProtection.Id) // Quietpro
                                continue;
                        }
                        else
                        {
                            if (noiseProtection.NoiseProtectionDefinition.Id != 3 &&
                                noiseProtection.NoiseProtectionDefinition.Id != 2 &&
                                selectedTask.Task.NoiseProtection.Id != noiseProtection.Id)
                                // Quietpro and double hearing
                                continue;
                        }
                    }
                viewModel.NoiseProtection.Add(selectListItem);
            }

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

            selectedTask.ButtonPressed = viewModel.ButtonPressed;

            if (viewModel.BackgroundNoise != "<80")
            {
                int bNoise;
                if (int.TryParse(viewModel.BackgroundNoise, out bNoise))
                    selectedTask.BackgroundNoise = bNoise;
                else if (string.IsNullOrEmpty(viewModel.BackgroundNoise))
                    selectedTask.BackgroundNoise = 0;
            }

            if (viewModel.NoiseLevelMeassured == 0)
            {
                selectedTask.NoiseLevel = selectedTask.Task.NoiseLevelGuideline;
                selectedTask.IsNoiseMeassured = false;
            }
            else if (viewModel.NoiseLevelMeassured != selectedTask.Task.NoiseLevelGuideline && viewModel.NoiseLevelMeassured != 0)
            {
                selectedTask.NoiseLevel = viewModel.NoiseLevelMeassured;
                selectedTask.IsNoiseMeassured = true;
            }

            var noiseProtection = _noiseProtectionDAO.Get(viewModel.NoiseProtectionId);

            if (noiseProtection != null)
            {
                selectedTask.NoiseProtection = noiseProtection.Title;
                selectedTask.NoiseProtectionId = noiseProtection.Id;
            }

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes))
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                var timeSpan = selectedTask.Task.CalculateTimeSpan(selectedTask.Task.TaskDefinition.RoleType.ToString(), selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, selectedTask.Task.Frequency, noiseProtection, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.UsePercentage = true;
            }
            else
            {
                var timeSpan = new TimeSpanFactory().CreateFromStrings(viewModel.Hours, viewModel.Minutes);

                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.Percentage = (int)Math.Round(selectedTask.Task.CalculatePercentage(selectedTask.Task.TaskDefinition.RoleType.ToString(), selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, selectedTask.Task.Frequency, noiseProtection, new TimeSpan(0, selectedTask.Hours, selectedTask.Minutes, 0)));
                selectedTask.UsePercentage = false;
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

            if (string.IsNullOrEmpty(viewModel.Hours) && string.IsNullOrEmpty(viewModel.Minutes) &&
                string.IsNullOrEmpty(viewModel.Percentage))
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorWorkTimeRequired);

            if (viewModel.NoiseProtectionId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorNoiseProtectionRequired);
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
                NoiseProtectionId = task.NoiseProtection.Id,
                ButtonPressed = task.ButtonPressed,
                Task = task,
                CreatedBy = string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name,
                CreatedDate = DateTime.Now.Date
            };

            var noiseProtection = _noiseProtectionDAO.Get(viewModel.NoiseProtectionId);

            selectedTask.NoiseProtection = noiseProtection.Title;
            selectedTask.NoiseProtectionId = noiseProtection.Id;

            selectedTask.ButtonPressed = viewModel.ButtonPressed;

            if (viewModel.BackgroundNoise != "<80")
            {
                int bNoise;
                int.TryParse(viewModel.BackgroundNoise, out bNoise);
                if (bNoise != 0)
                    selectedTask.BackgroundNoise = bNoise;
            }

            if (viewModel.NoiseLevelMeassured != task.NoiseLevelGuideline && viewModel.NoiseLevelMeassured != 0)
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
                selectedTask.Percentage = (int)Math.Round(task.CalculatePercentage(task.TaskDefinition.RoleType.ToString(), selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, selectedTask.Task.Frequency, noiseProtection, timeSpan));
                selectedTask.UsePercentage = false;
            }
            else
            {
                selectedTask.Percentage = string.IsNullOrEmpty(viewModel.Percentage) ? 0 : int.Parse(viewModel.Percentage);
                var timeSpan = task.CalculateTimeSpan(task.TaskDefinition.RoleType.ToString(), selectedTask.NoiseLevel, selectedTask.ButtonPressed, selectedTask.BackgroundNoise, selectedTask.Task.Frequency, noiseProtection, selectedTask.Percentage);
                selectedTask.Hours = timeSpan.Hours;
                selectedTask.Minutes = timeSpan.Minutes;
                selectedTask.UsePercentage = true;
            }

            return selectedTask;
        }
    }
}
