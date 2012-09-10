using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class NoiseProtectionDefinitionService : INoiseProtectionDefinitionService
    {
        private readonly IDAO<NoiseProtectionDefinition, int> _noiseProtectionDefinitionDAO;

        public NoiseProtectionDefinitionService(IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO)
        {
            _noiseProtectionDefinitionDAO = noiseProtectionDefinitionDAO;
        }


        public GenericDefinitionIndexViewModel Index()
        {
            IEnumerable<NoiseProtectionDefinition> definitions = _noiseProtectionDefinitionDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            return viewModel;
        }

        public GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel)
        {
            NoiseProtectionDefinition definition = new NoiseProtectionDefinition
                {
                    SystemName = editModel.Title
                };

            _noiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName
                };

            return viewModel;
        }

        public GenericDefinitionViewModel EditNoiseProtectionForm(int id)
        {
            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    HasTranslationSupport = true
                };

            foreach (NoiseProtection noiseProtection in definition.NoiseProtections)
            {
                GenericTranslationViewModel translationViewModel = new GenericTranslationViewModel(noiseProtection.CultureName)
                    {
                        Id = noiseProtection.Id,
                        Title = noiseProtection.Title
                    };

                viewModel.Translations.Add(translationViewModel);
            }

            return viewModel;
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(id);
            definition.SystemName = editModel.Title;

            _noiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            NoiseProtectionDefinition definintion = _noiseProtectionDefinitionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = definintion.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = definintion.SystemName
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            NoiseProtectionDefinition noiseProtectionDefinition = _noiseProtectionDefinitionDAO.Load(id);
            _noiseProtectionDefinitionDAO.Delete(noiseProtectionDefinition);
        }
    }
}