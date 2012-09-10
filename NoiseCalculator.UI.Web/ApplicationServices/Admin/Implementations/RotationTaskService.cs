using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class RotationTaskService : IRotationTaskService
    {
        private readonly IRotationDAO _rotationDAO;
        private readonly ITaskDAO _taskDAO;
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;
        private readonly IRoleDAO _roleDAO;

        public RotationTaskService(IRotationDAO rotationDAO, ITaskDAO taskDAO, ITaskDefinitionDAO taskDefinitionDAO, IRoleDAO roleDAO)
        {
            _rotationDAO = rotationDAO;
            _taskDAO = taskDAO;
            _taskDefinitionDAO = taskDefinitionDAO;
            _roleDAO = roleDAO;
        }


        public RotationTaskViewModel CreateRotationTaskForm(int definitionId)
        {
            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList("0");
            RotationTaskViewModel viewModel = new RotationTaskViewModel(languages) { TaskDefinitionId = definitionId };
            
            AddTaskListsToViewModel(viewModel, null);

            return viewModel;
        }

        public RotationTaskListItemViewModel Create(RotationTaskEditModel editModel)
        {
            Rotation rotation = new Rotation()
            {
                OperatorTask = _taskDAO.Get(editModel.OperatorTaskId),
                AssistantTask = _taskDAO.Get(editModel.AssistantTaskId)
            };

            Task task = new Task()
            {
                CultureName = editModel.SelectedCultureName,
                Title = editModel.Title,
                AllowedExposureMinutes = 0,
                NoiseLevelGuideline = 0,
                TaskDefinition = _taskDefinitionDAO.Load(editModel.TaskDefinitionId),
                Role = _roleDAO.Get("Rotation", Thread.CurrentThread.CurrentCulture.Name),
                NoiseProtection = rotation.OperatorTask.NoiseProtection
            };

            rotation.Task = task;

            _rotationDAO.Store(rotation);

            RotationTaskListItemViewModel viewModel = CreateTableRowViewModel(rotation);

            return viewModel;
        }


        public RotationTaskViewModel EditRotationTaskForm(int id)
        {
            Rotation rotation = _rotationDAO.Get(id);
            
            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList(rotation.Task.CultureName);
            RotationTaskViewModel viewModel = new RotationTaskViewModel(languages)
            {
                Id = rotation.Id,
                TaskDefinitionId = rotation.Task.TaskDefinition.Id,
                Title = rotation.Task.Title
            };

            AddTaskListsToViewModel(viewModel, rotation);
            
            return viewModel;
        }

        public RotationTaskListItemViewModel Edit(int id, RotationTaskEditModel editModel)
        {
            Rotation rotation = _rotationDAO.Get(id);
            rotation.OperatorTask = _taskDAO.Get(editModel.OperatorTaskId);
            rotation.AssistantTask = _taskDAO.Get(editModel.AssistantTaskId);
            rotation.Task.Title = editModel.Title;
            rotation.Task.CultureName = editModel.SelectedCultureName;

            _rotationDAO.Store(rotation);

            RotationTaskListItemViewModel viewModel = CreateTableRowViewModel(rotation);

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            Rotation rotation = _rotationDAO.Get(id);

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = "trans" + rotation.Id, 
                    Title = rotation.Task.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            Rotation rotation = _rotationDAO.Load(id);
            _rotationDAO.Delete(rotation);
        }


        private RotationTaskListItemViewModel CreateTableRowViewModel(Rotation rotation)
        {
            return new RotationTaskListItemViewModel
            {
                Id = rotation.Id,
                Language = LanguageResolver.GetLanguageName(rotation.Task.CultureName),
                Title = rotation.Task.Title,
                OperatorTask = rotation.OperatorTask.Title,
                OperatorRole = rotation.OperatorTask.Role.Title,
                AssistantTask = rotation.AssistantTask.Title,
                AssistantRole = rotation.AssistantTask.Role.Title
            };
        }

        private void AddTaskListsToViewModel(RotationTaskViewModel viewModel, Rotation rotation)
        {
            viewModel.OperatorTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            viewModel.AssistantTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));

            foreach (Task task in _taskDAO.GetAllOrdered().Where(x => x.Role.RoleType == RoleTypeEnum.Regular))
            {
                SelectOptionViewModel selectOption = new SelectOptionViewModel(task.Title, task.Id.ToString(CultureInfo.InvariantCulture));

                if (task.Role.SystemTitle == "Operator")
                {
                    selectOption.IsSelected = (rotation != null && task.Id == rotation.OperatorTask.Id);
                    viewModel.OperatorTasks.Add(selectOption);
                }
                else if (task.Role.SystemTitle == "Assistant")
                {
                    selectOption.IsSelected = (rotation != null && task.Id == rotation.AssistantTask.Id);
                    viewModel.AssistantTasks.Add(selectOption);
                }
            }
        }
    }
}