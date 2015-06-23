using System;
using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class TaskDefinitionService : ITaskDefinitionService
    {
        private readonly IDAO<TaskDefinition,int> _taskDefinitionDAO;

        public TaskDefinitionService(IDAO<TaskDefinition, int> taskDefinitionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
        }


        public TaskDefinitionIndexViewModel Index()
        {
            IEnumerable<TaskDefinition> definitions = _taskDefinitionDAO.GetAllOrderedBy(x => x.SystemName);

            TaskDefinitionIndexViewModel viewModel = new TaskDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                TaskDefinitionListItemViewModel taskDefinitionListItemView = new TaskDefinitionListItemViewModel()
                {
                    Id = definition.Id,
                    SystemName = definition.SystemName,
                    RoleType = definition.RoleType.ToString(),
                    SystemNameEN = definition.SystemNameEN
                };
                viewModel.Definitions.Add(taskDefinitionListItemView);
            }

            return viewModel;
        }


        public TaskDefinitionListItemViewModel Create(CreateTaskDefinitionEditModel editModel)
        {
            TaskDefinition definition = new TaskDefinition
                {
                    SystemName = editModel.Title,
                    RoleType = (RoleTypeEnum) Enum.Parse(typeof (RoleTypeEnum), editModel.RoleType),
                    SystemNameEN = editModel.TitleEN
                };
            _taskDefinitionDAO.Store(definition);

            TaskDefinitionListItemViewModel viewModel = new TaskDefinitionListItemViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    RoleType = definition.RoleType.ToString(),
                    SystemNameEN = definition.SystemNameEN
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