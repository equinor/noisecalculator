using System.Threading;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class NoiseProtectionService : INoiseProtectionService
    {
        private readonly IDAO<NoiseProtection, int> _noiseProtectionDAO;
        private readonly IDAO<NoiseProtectionDefinition, int> _noiseProtectionDefinitionDAO;

        public NoiseProtectionService(IDAO<NoiseProtection, int> noiseProtectionDAO, IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO)
        {
            _noiseProtectionDAO = noiseProtectionDAO;
            _noiseProtectionDefinitionDAO = noiseProtectionDefinitionDAO;
        }
        
        public GenericTranslationViewModel CreateNoiseProtectionForm(int definitionId)
        {
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(Thread.CurrentThread.CurrentCulture.Name)
                {
                    DefinitionId = definitionId
                };

            return viewModel;
        }
        
        public GenericTranslationViewModel Create(GenericTranslationEditModel editModel)
        {
            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(editModel.DefinitionId);

            NoiseProtection noiseProtection = new NoiseProtection
                {
                    NoiseProtectionDefinition = definition,
                    Title = editModel.Title,
                    CultureName = editModel.SelectedCultureName
                };
            definition.NoiseProtections.Add(noiseProtection);

            _noiseProtectionDefinitionDAO.Store(definition);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName)
                {
                    DefinitionId = noiseProtection.NoiseProtectionDefinition.Id,
                    Id = noiseProtection.Id,
                    Title = noiseProtection.Title
                };

            return viewModel;
        }


        public GenericTranslationViewModel EditNoiseProtectionForm(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName)
                {
                    Id = noiseProtection.Id,
                    DefinitionId = noiseProtection.NoiseProtectionDefinition.Id,
                    Title = noiseProtection.Title
                };

            return viewModel;
        }

        public GenericTranslationViewModel Edit(GenericTranslationEditModel editModel)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(editModel.Id);
            noiseProtection.Title = editModel.Title;
            noiseProtection.CultureName = editModel.SelectedCultureName;

            _noiseProtectionDAO.Store(noiseProtection);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName)
                {
                    DefinitionId = noiseProtection.NoiseProtectionDefinition.Id,
                    Id = noiseProtection.Id,
                    Title = noiseProtection.Title
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = "trans" + noiseProtection.Id, 
                    Title = noiseProtection.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Load(id);
            _noiseProtectionDAO.Delete(noiseProtection);
        }
    }
}