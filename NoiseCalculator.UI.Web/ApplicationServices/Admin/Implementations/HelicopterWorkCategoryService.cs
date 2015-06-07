using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            HelicopterWorkCategory helicopterWorkCategory = new HelicopterWorkCategory
            {
                Title = editModel.Title
            };

            _helicopterWorkCategoryDAO.Store(helicopterWorkCategory);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
            {
                Id = helicopterWorkCategory.Id,
                SystemName = helicopterWorkCategory.Title
            };

            return viewModel;
        }

        public GenericDefinitionViewModel EditHelicopterWorkCategoryForm(int id)
        {
            throw new NotImplementedException();
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            throw new NotImplementedException();
        }

        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}