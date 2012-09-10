using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterNoiseProtectionDefinitionService : IHelicopterNoiseProtectionDefinitionService
    {
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;

        public HelicopterNoiseProtectionDefinitionService(IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO)
        {
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
        }


        public GenericDefinitionIndexViewModel Index()
        {
            IEnumerable<HelicopterNoiseProtectionDefinition> definitions = _helicopterNoiseProtectionDefinitionDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            return viewModel;
        }


        public GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel)
        {
            HelicopterNoiseProtectionDefinition definition = new HelicopterNoiseProtectionDefinition
                {
                    SystemName = editModel.Title
                };

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    HasTranslationSupport = true
                };

            return viewModel;
        }

        
        public GenericDefinitionViewModel EditNoiseProtectionForm(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName, 
                    HasTranslationSupport = true
                };

            foreach (HelicopterNoiseProtection helicopterNoiseProtection in definition.HelicopterNoiseProtections)
            {
                GenericTranslationViewModel translationViewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName)
                    {
                        Id = helicopterNoiseProtection.Id,
                        Title = helicopterNoiseProtection.Title
                    };

                viewModel.Translations.Add(translationViewModel);
            }

            return viewModel;
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            definition.SystemName = editModel.Title;

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = definition.Id, 
                    SystemName = definition.SystemName
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = definition.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = definition.SystemName
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Load(id);
            _helicopterNoiseProtectionDefinitionDAO.Delete(definition);
        }
    }
}