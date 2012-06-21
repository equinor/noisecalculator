using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Administrator;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Models;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorDAO _adminDAO;

        public AdministratorController(IAdministratorDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }

        public ActionResult Index()
        {
            IEnumerable<Administrator> administrators = _adminDAO.GetAll();

            AdministratorIndexViewModel viewModel = new AdministratorIndexViewModel();
            foreach (var administrator in administrators)
            {
                string username = UserHelper.CreateUsernameWithoutDomain(administrator.Username);
                viewModel.Administrators.Add(new AdministratorListItemViewModel { Username =  username});
            }

            viewModel.PageTitle = "Administrators"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            //viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            AdministratorViewModel viewModel = new AdministratorViewModel();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateAdministrator", viewModel);
        }

        [HttpPost]
        public ActionResult Create(AdministratorEditModel form)
        {
            if (string.IsNullOrEmpty(form.Username))
            {
                Response.StatusCode = 500;
                return Json("FAIL!");
            }

            Administrator administrator = new Administrator(form.Username.ToUpper());
            _adminDAO.Store(administrator);

            AdministratorListItemViewModel viewModel = new AdministratorListItemViewModel();
            viewModel.Username = form.Username.ToUpper();

            return PartialView("_AdministratorTableRow", viewModel);
        }

        public ActionResult ConfirmDelete(string id) // <-- For the Administrator entity, the id is the username
        {
            Administrator administrator = _adminDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = administrator.Username;
            viewModel.Title = administrator.Username;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(string id) // <-- For the Administrator entity, the id is the username
        {
            try
            {
                Administrator administrator = _adminDAO.Load(id);
                _adminDAO.Delete(administrator);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }
    }
}
