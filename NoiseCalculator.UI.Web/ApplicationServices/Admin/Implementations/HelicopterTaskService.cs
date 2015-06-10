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

        public HelicopterTaskService(IHelicopterTaskDAO helicopterTaskDAO,
                                        IDAO<HelicopterType, int> helicopterTypeDAO,
                                        IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO)
        {
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _noiseProtectionDefinitionDAO = noiseProtectionDefinitionDAO;
        }


        public HelicopterTaskIndexViewModel Index()
        {
            IEnumerable<HelicopterTask> helicopterTasks = _helicopterTaskDAO.GetAllOrderedByType();

            HelicopterTaskIndexViewModel viewModel = new HelicopterTaskIndexViewModel();
            foreach (var helicopterTask in helicopterTasks)
            {
                HelicopterTaskListItemViewModel listItemViewModel = new HelicopterTaskListItemViewModel
                    {
                        Id = helicopterTask.Id,
                        Helicopter = helicopterTask.HelicopterType.Title,
                        NoiseProtectionDefinition = helicopterTask.NoiseProtectionDefinition.SystemName,
                        ButtonPressed = helicopterTask.ButtonPressed,
                        FixedTime = helicopterTask.FixedTime
                    };
                viewModel.HelicopterTasks.Add(listItemViewModel);
            }

            return viewModel;
        }


        public HelicopterTaskViewModel CreateNoiseProtectionForm()
        {
            HelicopterTaskViewModel viewModel = new HelicopterTaskViewModel();

            viewModel.Helicopters.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectOptionViewModel(type.Title, type.Id.ToString(CultureInfo.InvariantCulture)));
            }

            viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var noiseProtectionDefinition in _noiseProtectionDefinitionDAO.GetAll())
            {
                viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(noiseProtectionDefinition.SystemName, noiseProtectionDefinition.Id.ToString()));
            }
            
            return viewModel;
        }

        public HelicopterTaskListItemViewModel Create(HelicopterTaskEditModel editModel)
        {
            HelicopterTask helicopterTask = new HelicopterTask
                {
                    HelicopterType = _helicopterTypeDAO.Get(editModel.HelicopterTypeId),
                    NoiseProtectionDefinition = _noiseProtectionDefinitionDAO.Get(editModel.NoiseProtectionDefinitionId),
                    ButtonPressed = editModel.ButtonPressed,
                    FixedTime = editModel.FixedTime
                };

            _helicopterTaskDAO.Store(helicopterTask);

            HelicopterTaskListItemViewModel viewModel = new HelicopterTaskListItemViewModel
                {
                    Id = helicopterTask.Id,
                    Helicopter = helicopterTask.HelicopterType.Title,
                    NoiseProtectionDefinition = helicopterTask.NoiseProtectionDefinition.SystemName,
                    ButtonPressed = helicopterTask.ButtonPressed,
                    FixedTime = helicopterTask.FixedTime
                };

            return viewModel;
        }


        public HelicopterTaskViewModel EditNoiseProtectionForm(int id)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);

            HelicopterTaskViewModel viewModel = new HelicopterTaskViewModel
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

            viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var noiseProtectionDefinition in _noiseProtectionDefinitionDAO.GetAll())
            {
                viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(noiseProtectionDefinition.SystemName, noiseProtectionDefinition.Id.ToString())
                {
                    IsSelected = (noiseProtectionDefinition.Id == helicopterTask.NoiseProtectionDefinition.Id)
                });
            }
            
            viewModel.ButtonPressed = helicopterTask.ButtonPressed;

            viewModel.FixedTime = helicopterTask.FixedTime;

            return viewModel;
        }

        public HelicopterTaskListItemViewModel Edit(int id, HelicopterTaskEditModel editModel)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);

            helicopterTask.HelicopterType = _helicopterTypeDAO.Get(editModel.HelicopterTypeId);
            helicopterTask.NoiseProtectionDefinition = _noiseProtectionDefinitionDAO.Get(editModel.NoiseProtectionDefinitionId);
            helicopterTask.ButtonPressed = editModel.ButtonPressed;
            helicopterTask.FixedTime = editModel.FixedTime;

            _helicopterTaskDAO.Store(helicopterTask);

            HelicopterTaskListItemViewModel viewModel = new HelicopterTaskListItemViewModel
                {
                    Id = helicopterTask.Id,
                    Helicopter = helicopterTask.HelicopterType.Title,
                    NoiseProtectionDefinition = helicopterTask.NoiseProtectionDefinition.SystemName,
                    ButtonPressed = helicopterTask.ButtonPressed,
                    FixedTime = helicopterTask.FixedTime
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = helicopterTask.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = helicopterTask.ToString()
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Load(id);
            _helicopterTaskDAO.Delete(helicopterTask);
        }
    }
}