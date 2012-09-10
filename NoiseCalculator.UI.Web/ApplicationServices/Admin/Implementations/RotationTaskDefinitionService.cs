using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask;
using NoiseCalculator.UI.Web.Support;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class RotationTaskDefinitionService : IRotationTaskDefinitionService
    {
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;
        private readonly IRotationDAO _rotationDAO;

        public RotationTaskDefinitionService(ITaskDefinitionDAO taskDefinitionDAO, IRotationDAO rotationDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
            _rotationDAO = rotationDAO;
        }


        public TaskDefinitionRotationViewModel EditGenericTaskDefinitionForm(int id)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);
            
            TaskDefinitionRotationViewModel viewModel = new TaskDefinitionRotationViewModel
                    {
                        Id = definition.Id,
                        SystemName = definition.SystemName,
                    };

            IList<Rotation> rotations = _rotationDAO.GetAllByTaskDefinitionIdOrderedByTaskTitle(definition.Id);
            foreach (Rotation rotation in rotations)
            {
                RotationTaskListItemViewModel translationViewModel
                    = new RotationTaskListItemViewModel()
                        {
                            Id = rotation.Id,
                            Title = rotation.Task.Title,
                            OperatorTask = rotation.OperatorTask.Title,
                            OperatorRole = rotation.OperatorTask.Role.Title,
                            AssistantTask = rotation.AssistantTask.Title,
                            AssistantRole = rotation.AssistantTask.Role.Title,
                            Language = LanguageResolver.GetLanguageName(rotation.Task.CultureName)
                        };

                viewModel.RotationTasks.Add(translationViewModel);
            }

            return viewModel;
        }
    }
}