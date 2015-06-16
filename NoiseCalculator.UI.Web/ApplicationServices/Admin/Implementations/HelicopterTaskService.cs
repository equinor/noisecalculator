using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterTaskService : IHelicopterTaskService
    {
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly IDAO<NoiseProtectionDefinition, int> _noiseProtectionDefinitionDAO;
        private readonly IDAO<Task, int> _taskDAO;

        public HelicopterTaskService(IHelicopterTaskDAO helicopterTaskDAO,
                                        IDAO<HelicopterType, int> helicopterTypeDAO,
                                        IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO, IDAO<Task, int> taskDAO)
        {
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _noiseProtectionDefinitionDAO = noiseProtectionDefinitionDAO;
            _taskDAO = taskDAO;
        }


        public HelicopterTaskIndexViewModel Index()
        {
            IEnumerable<HelicopterTask> helicopterTasks = _helicopterTaskDAO.GetAllOrderedByType();

            var viewModel = new HelicopterTaskIndexViewModel();
            foreach (var helicopterTask in helicopterTasks)
            {
                var listItemViewModel = new HelicopterTaskListItemViewModel
                    {
                        Id = helicopterTask.Id,
                        Helicopter = helicopterTask.HelicopterType.Title,
                        Task = helicopterTask.Task != null ? helicopterTask.Task.Title : helicopterTask.HelicopterType.Title,
                        NoiseLevel = helicopterTask.NoiseLevel
                    };
                viewModel.HelicopterTasks.Add(listItemViewModel);
            }

            return viewModel;
        }


        public HelicopterTaskViewModel CreateNoiseProtectionForm()
        {
            var viewModel = new HelicopterTaskViewModel();

            viewModel.Helicopters.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectOptionViewModel(type.Title, type.Id.ToString(CultureInfo.InvariantCulture)));
            }
            
            viewModel.Tasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var task in _taskDAO.GetAll())
            {
                viewModel.Tasks.Add(new SelectOptionViewModel(task.Title, task.Id.ToString(CultureInfo.InvariantCulture)));
            }
            
            return viewModel;
        }

        public HelicopterTaskListItemViewModel Create(HelicopterTaskEditModel editModel)
        {
            var helicopterTask = new HelicopterTask
                {
                    HelicopterType = _helicopterTypeDAO.Get(editModel.HelicopterTypeId),
                    Task = _taskDAO.Get(editModel.TaskId),
                    NoiseLevel = editModel.NoiseLevel
                };

            _helicopterTaskDAO.Store(helicopterTask);

            var viewModel = new HelicopterTaskListItemViewModel
                {
                    Id = helicopterTask.Id,
                    Helicopter = helicopterTask.HelicopterType.Title,
                    Task = helicopterTask.Task.Title,
                    NoiseLevel = helicopterTask.NoiseLevel
                };

            return viewModel;
        }


        public HelicopterTaskViewModel EditNoiseProtectionForm(int id)
        {
            var helicopterTask = _helicopterTaskDAO.Get(id);

            var viewModel = new HelicopterTaskViewModel
                {
                    Id = helicopterTask.Id
                };

            viewModel.Helicopters.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectOptionViewModel(type.Title, type.Id.ToString(CultureInfo.InvariantCulture))
                {
                    IsSelected = (type.Id == helicopterTask.HelicopterType.Id)
                });
            }
            
            viewModel.Tasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var task in _taskDAO.GetAll())
            {
                viewModel.Tasks.Add(new SelectOptionViewModel(task.Title, task.Id.ToString(CultureInfo.InvariantCulture))
                {
                    IsSelected = (task.Id == helicopterTask.Task.Id)
                });
            }

            viewModel.NoiseLevel = helicopterTask.NoiseLevel;
            
            return viewModel;
        }

        public HelicopterTaskListItemViewModel Edit(int id, HelicopterTaskEditModel editModel)
        {
            var helicopterTask = _helicopterTaskDAO.Get(id);

            helicopterTask.HelicopterType = _helicopterTypeDAO.Get(editModel.HelicopterTypeId);
            helicopterTask.Task = _taskDAO.Get(editModel.TaskId);
            helicopterTask.NoiseLevel = editModel.NoiseLevel;

            _helicopterTaskDAO.Store(helicopterTask);

            var viewModel = new HelicopterTaskListItemViewModel
                {
                    Id = helicopterTask.Id,
                    Helicopter = helicopterTask.HelicopterType.Title,
                    Task = helicopterTask.Task.Title,
                    NoiseLevel = helicopterTask.NoiseLevel
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            var helicopterTask = _helicopterTaskDAO.Get(id);
            var viewModel = new DeleteConfirmationViewModel
                {
                    Id = helicopterTask.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = helicopterTask.ToString()
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            var helicopterTask = _helicopterTaskDAO.Load(id);
            _helicopterTaskDAO.Delete(helicopterTask);
        }
    }
}