using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterWorkCategoryService : IHelicopterWorkCategoryService
    {
        private readonly IDAO<HelicopterWorkCategory, int> _helicopterWorkCategoryDAO;

        public HelicopterWorkCategoryService(IDAO<HelicopterWorkCategory, int> helicopterWorkCategoryDAO)
        {
            _helicopterWorkCategoryDAO = helicopterWorkCategoryDAO;
        }

        public GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel)
        {
            var helicopterWorkCategory = new HelicopterWorkCategory
            {
                Title = editModel.Title
            };

            _helicopterWorkCategoryDAO.Store(helicopterWorkCategory);

            var viewModel = new GenericDefinitionViewModel
            {
                Id = helicopterWorkCategory.Id,
                SystemName = helicopterWorkCategory.Title
            };

            return viewModel;
        }

        public GenericDefinitionViewModel EditHelicopterWorkCategoryForm(int id)
        {
            var helicopterWorkCategory = _helicopterWorkCategoryDAO.Get(id);

            var viewModel = new GenericDefinitionViewModel
            {
                Id = helicopterWorkCategory.Id,
                SystemName = helicopterWorkCategory.Title,
                HasTranslationSupport = false
            };

            return viewModel;
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            var helicopterWorkCategory = _helicopterWorkCategoryDAO.Get(id);
            helicopterWorkCategory.Title = editModel.Title;

            _helicopterWorkCategoryDAO.Store(helicopterWorkCategory);

            var viewModel = new GenericDefinitionViewModel
            {
                Id = helicopterWorkCategory.Id,
                SystemName = helicopterWorkCategory.Title
            };

            return viewModel;
        }

        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            var helicopterWorkCategory = _helicopterWorkCategoryDAO.Get(id);

            var viewModel = new DeleteConfirmationViewModel
            {
                Id = helicopterWorkCategory.Id.ToString(CultureInfo.InvariantCulture),
                Title = helicopterWorkCategory.Title
            };

            return viewModel;
        }

        public void Delete(int id)
        {
            var helicopterWorkCategory = _helicopterWorkCategoryDAO.Load(id);
            _helicopterWorkCategoryDAO.Delete(helicopterWorkCategory);
        }

        public GenericDefinitionIndexViewModel Index()
        {
            var helicopterWorkCategories = _helicopterWorkCategoryDAO.GetAllOrderedBy(x => x.Title);

            var viewModel = new GenericDefinitionIndexViewModel();
            foreach (var helicopterWorkCategory in helicopterWorkCategories)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel
                {
                    Id = helicopterWorkCategory.Id,
                    SystemName = helicopterWorkCategory.Title
                });
            }

            return viewModel;
        }
    }

}