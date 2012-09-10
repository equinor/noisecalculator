using System.Threading;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterNoiseProtectionService : IHelicopterNoiseProtectionService
    {
        private readonly IDAO<HelicopterNoiseProtection, int> _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;

        public HelicopterNoiseProtectionService(IDAO<HelicopterNoiseProtection, int> helicopterNoiseProtectionDAO, 
            IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO)
        {
            _helicopterNoiseProtectionDAO = helicopterNoiseProtectionDAO;
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
        }


        public GenericTranslationViewModel CreateHelicopterNoiseProtectionForm(int id)
        {
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(Thread.CurrentThread.CurrentCulture.Name)
                {
                    DefinitionId = id
                };

            return viewModel;
        }
        
        public GenericTranslationViewModel Create(GenericTranslationEditModel editModel)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(editModel.DefinitionId);

            HelicopterNoiseProtection helicopterNoiseProtection = new HelicopterNoiseProtection
                {
                    HelicopterNoiseProtectionDefinition = definition,
                    Title = editModel.Title,
                    CultureName = editModel.SelectedCultureName
                };
            definition.HelicopterNoiseProtections.Add(helicopterNoiseProtection);

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName)
                {
                    DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id,
                    Id = helicopterNoiseProtection.Id,
                    Title = helicopterNoiseProtection.Title
                };

            return viewModel;
        }


        public GenericTranslationViewModel EditHelicopterNoiseProtiectionForm(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName)
                {
                    Id = helicopterNoiseProtection.Id,
                    DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id,
                    Title = helicopterNoiseProtection.Title
                };

            return viewModel;
        }
        
        public GenericTranslationViewModel Edit(GenericTranslationEditModel editModel)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(editModel.Id);
            helicopterNoiseProtection.Title = editModel.Title;
            helicopterNoiseProtection.CultureName = editModel.SelectedCultureName;

            _helicopterNoiseProtectionDAO.Store(helicopterNoiseProtection);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName)
                {
                    DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id,
                    Id = helicopterNoiseProtection.Id,
                    Title = helicopterNoiseProtection.Title
                };

            return viewModel;
        }


        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = "trans" + helicopterNoiseProtection.Id, 
                    Title = helicopterNoiseProtection.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);
            _helicopterNoiseProtectionDAO.Delete(helicopterNoiseProtection);
        }
    }
}
