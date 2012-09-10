using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterTypeService : IHelicopterTypeService
    {
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;

        public HelicopterTypeService(IDAO<HelicopterType, int> helicopterTypeDAO)
        {
            _helicopterTypeDAO = helicopterTypeDAO;
        }


        public GenericDefinitionIndexViewModel Index()
        {
            IEnumerable<HelicopterType> helicopterTypes = _helicopterTypeDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var helicopterType in helicopterTypes)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = helicopterType.Id, SystemName = helicopterType.Title });
            }

            return viewModel;
        }


        public GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel)
        {
            HelicopterType helicopterType = new HelicopterType
                {
                    Title = editModel.Title
                };

            _helicopterTypeDAO.Store(helicopterType);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterType.Id, 
                    SystemName = helicopterType.Title
                };

            return viewModel;
        }

        public GenericDefinitionViewModel EditHelicopterTypeForm(int id)
        {
            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterType.Id, 
                    SystemName = helicopterType.Title, 
                    HasTranslationSupport = false
                };

            return viewModel;
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);
            helicopterType.Title = editModel.Title;

            _helicopterTypeDAO.Store(helicopterType);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterType.Id, 
                    SystemName = helicopterType.Title
                };

            return viewModel;
        }

        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = helicopterType.Id.ToString(CultureInfo.InvariantCulture), 
                    Title = helicopterType.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            HelicopterType helicopterWorkInterval = _helicopterTypeDAO.Load(id);
            _helicopterTypeDAO.Delete(helicopterWorkInterval);
        }
    }
}