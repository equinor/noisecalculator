using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.GenericTask;
using NoiseCalculator.UI.Web.Areas.Admin.Models.TaskDefinition;
using NoiseCalculator.UI.Web.Support;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class GenericTaskDefinitionService : IGenericTaskDefinitionService
    {
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;

        public GenericTaskDefinitionService(ITaskDefinitionDAO taskDefinitionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
        }

        
        public TaskDefinitionGenericViewModel EditGenericTaskDefinitionForm(int id)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);

            TaskDefinitionGenericViewModel viewModel = new TaskDefinitionGenericViewModel
                {
                    Id = definition.Id,
                    SystemName = definition.SystemName
                };

            foreach (Task task in definition.Tasks)
            {
                TaskListItemViewModel translationViewModel = new TaskListItemViewModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Role = task.Role.Title,
                        NoiseProtection = task.NoiseProtection.Title,
                        NoiseLevelGuideline = task.NoiseLevelGuideline,
                        AllowedExposureMinutes = task.AllowedExposureMinutes,
                        Language = LanguageResolver.GetLanguageName(task.CultureName)
                    };

                viewModel.Tasks.Add(translationViewModel);
            }

            return viewModel;
        }

        public TaskDefinitionListItemViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);
            definition.SystemName = editModel.Title;

            _taskDefinitionDAO.Store(definition);

            TaskDefinitionListItemViewModel viewModel = new TaskDefinitionListItemViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    RoleType = definition.RoleType.ToString()
                };

            return viewModel;
        }
    }
}