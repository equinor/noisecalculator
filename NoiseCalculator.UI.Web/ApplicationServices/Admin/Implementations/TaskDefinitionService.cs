using System;
using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.TaskDefinition;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class TaskDefinitionService : ITaskDefinitionService
    {
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;

        public TaskDefinitionService(ITaskDefinitionDAO taskDefinitionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
        }


        public TaskDefinitionIndexViewModel Index()
        {
            IEnumerable<TaskDefinition> definitions = _taskDefinitionDAO.GetAllOrdered();

            TaskDefinitionIndexViewModel viewModel = new TaskDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                TaskDefinitionListItemViewModel taskDefinitionListItemView = new TaskDefinitionListItemViewModel()
                {
                    Id = definition.Id,
                    SystemName = definition.SystemName,
                    RoleType = definition.RoleType.ToString()
                };
                viewModel.Definitions.Add(taskDefinitionListItemView);
            }

            return viewModel;
        }


        public TaskDefinitionListItemViewModel Create(CreateTaskDefinitionEditModel editModel)
        {
            TaskDefinition definition = new TaskDefinition
                {
                    SystemName = editModel.Title.ToUpper(),
                    RoleType = (RoleTypeEnum) Enum.Parse(typeof (RoleTypeEnum), editModel.RoleType)
                };
            _taskDefinitionDAO.Store(definition);

            TaskDefinitionListItemViewModel viewModel = new TaskDefinitionListItemViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    RoleType = definition.RoleType.ToString()
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            TaskDefinition definintion = _taskDefinitionDAO.Get(id);

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = definintion.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = definintion.SystemName
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            TaskDefinition noiseProtectionDefinition = _taskDefinitionDAO.Load(id);
            _taskDefinitionDAO.Delete(noiseProtectionDefinition);
        }
    }
}