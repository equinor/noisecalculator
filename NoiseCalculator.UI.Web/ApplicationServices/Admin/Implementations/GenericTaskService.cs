using System;
using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class GenericTaskService : IGenericTaskService
    {
        private readonly IDAO<TaskDefinition, int> _taskDefinitionDAO;
        private readonly ITaskDAO _taskDAO;
        private readonly IRoleDAO _roleDAO;
        private readonly INoiseProtectionDAO _noiseProtectionDAO;

        public GenericTaskService(IDAO<TaskDefinition, int> taskDefinitionDAO, ITaskDAO taskDAO, IRoleDAO roleDAO, INoiseProtectionDAO noiseProtectionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
            _taskDAO = taskDAO;
            _roleDAO = roleDAO;
            _noiseProtectionDAO = noiseProtectionDAO;
        }


        public TaskViewModel CreateGenericTaskForm(int definitionId)
        {
            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList("0");
            TaskViewModel viewModel = new TaskViewModel(languages)
                {
                    DefinitionId = definitionId
                };

            viewModel.Roles.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (Role role in _roleDAO.GetAllFilteredByCurrentCulture())
            {
                // We want separate handling for rotation tasks, as the view should be quite different
                if (role.RoleType != RoleTypeEnum.Rotation)
                {
                    viewModel.Roles.Add(new SelectOptionViewModel(role.Title, role.Id.ToString(CultureInfo.InvariantCulture)));
                }
            }

            viewModel.NoiseProtections.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (NoiseProtection noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                viewModel.NoiseProtections.Add(new SelectOptionViewModel(noiseProtection.Title, noiseProtection.Id.ToString(CultureInfo.InvariantCulture)));
            }

            return viewModel;
        }

        public TaskListItemViewModel Create(TaskEditModel editModel)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(editModel.DefinitionId);

            TimeSpan allowedExposureTime = new TimeSpanFactory().CreateFromStrings(editModel.Hours, editModel.Minutes);

            Task task = new Task()
            {
                Title = editModel.Title,
                Role = _roleDAO.Get(editModel.RoleId),
                NoiseProtection = _noiseProtectionDAO.Get(editModel.NoiseProtectionId),
                NoiseLevelGuideline = Int32.Parse(editModel.NoiseLevelGuideline),
                AllowedExposureMinutes = Convert.ToInt32(allowedExposureTime.TotalMinutes),
                TaskDefinition = definition,
                ButtonPressed = editModel.ButtonPressed,
                BackgroundNoise = editModel.BackgroundNoise,
                CultureName = editModel.SelectedCultureName,
            };

            definition.Tasks.Add(task);

            _taskDefinitionDAO.Store(definition);

            TaskListItemViewModel viewModel = CreateTaskListItemViewModel(task);

            return viewModel;
        }


        public TaskViewModel EditGenericTaskForm(int id)
        {
            Task task = _taskDAO.Get(id);

            IList<SelectOptionViewModel> roles = new List<SelectOptionViewModel>();
            IList<SelectOptionViewModel> noiseProtections = new List<SelectOptionViewModel>();

            roles.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (Role role in _roleDAO.GetAllFilteredByCurrentCulture())
            {
                // We want separate handling for rotation tasks, as the view should be quite different
                if (role.RoleType != RoleTypeEnum.Rotation)
                {
                    roles.Add(new SelectOptionViewModel(role.Title, role.Id.ToString(CultureInfo.InvariantCulture))
                        {
                            IsSelected = (role.RoleDefinition.Id == task.Role.RoleDefinition.Id)
                        });
                }
            }

            noiseProtections.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (NoiseProtection noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                var selectOption = new SelectOptionViewModel(noiseProtection.Title, noiseProtection.Id.ToString(CultureInfo.InvariantCulture))
                    {
                        IsSelected = (noiseProtection.NoiseProtectionDefinition.Id == task.NoiseProtection.NoiseProtectionDefinition.Id)
                    };
                noiseProtections.Add(selectOption);
            }

            TimeSpan allowedExposureTime = new TimeSpanFactory().CreateFromMinutes(task.AllowedExposureMinutes);

            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList(task.CultureName);
            TaskViewModel viewModel = new TaskViewModel(languages)
            {
                Id = task.Id,
                Title = task.Title,
                NoiseLevelGuideline = task.NoiseLevelGuideline.ToString(CultureInfo.InvariantCulture),
                Hours = allowedExposureTime.Hours.ToString(CultureInfo.InvariantCulture),
                Minutes = allowedExposureTime.Minutes.ToString(CultureInfo.InvariantCulture),
                DefinitionId = task.TaskDefinition.Id,
                Roles = roles,
                NoiseProtections = noiseProtections
            };

            return viewModel;
        }

        public TaskListItemViewModel Edit(TaskEditModel editModel)
        {
            Task task = _taskDAO.Get(editModel.Id);

            TimeSpan allowedExposureTime = new TimeSpanFactory().CreateFromStrings(editModel.Hours, editModel.Minutes);

            task.Title = editModel.Title;
            task.NoiseLevelGuideline = Int32.Parse(editModel.NoiseLevelGuideline);
            task.AllowedExposureMinutes = Convert.ToInt32(allowedExposureTime.TotalMinutes);
            task.Role = _roleDAO.Get(editModel.RoleId);
            task.NoiseProtection = _noiseProtectionDAO.Get(editModel.NoiseProtectionId);
            task.ButtonPressed = editModel.ButtonPressed;
            task.BackgroundNoise = editModel.BackgroundNoise;

            _taskDAO.Store(task);

            TaskListItemViewModel viewModel = CreateTaskListItemViewModel(task);

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            Task task = _taskDAO.Get(id);

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = "trans" + task.Id, 
                    Title = task.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            Task task = _taskDAO.Load(id);
            _taskDAO.Delete(task);
        }


        private TaskListItemViewModel CreateTaskListItemViewModel(Task task)
        {
            TaskListItemViewModel taskListItemViewModel = new TaskListItemViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                NoiseProtection = task.NoiseProtection.Title,
                NoiseLevelGuideline = task.NoiseLevelGuideline,
                AllowedExposureMinutes = task.AllowedExposureMinutes,
                ButtonPressed = task.ButtonPressed,
                BackgroundNoise = task.BackgroundNoise,
                Language = LanguageResolver.GetLanguageName(task.CultureName)
            };

            return taskListItemViewModel;
        }
    }
}