using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Administrator;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Support;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorDAO _adminDAO;

        public AdministratorService(IAdministratorDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }


        public AdministratorIndexViewModel Index()
        {
            IEnumerable<Administrator> administrators = _adminDAO.GetAll();

            AdministratorIndexViewModel viewModel = new AdministratorIndexViewModel();
            foreach (var administrator in administrators)
            {
                string username = UserHelper.CreateUsernameWithoutDomain(administrator.Username);
                viewModel.Administrators.Add(new AdministratorListItemViewModel { Username = username });
            }

            return viewModel;
        }


        public AdministratorListItemViewModel Create(AdministratorEditModel editModel)
        {
            Administrator administrator = new Administrator(editModel.Username.ToUpper());
            _adminDAO.Store(administrator);

            AdministratorListItemViewModel viewModel = new AdministratorListItemViewModel
                {
                    Username = editModel.Username.ToUpper()
                };

            return viewModel;
        }

        public DeleteConfirmationViewModel DeleteConfirmationForm(string id)
        {
            Administrator administrator = _adminDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = administrator.Username, 
                    Title = administrator.Username
                };

            return viewModel;
        }

        public void Delete(string id)
        {
            Administrator administrator = _adminDAO.Load(id);
            _adminDAO.Delete(administrator);
        }
    }
}