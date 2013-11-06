using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Administrator;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _administratorService;

        public AdministratorController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        [Authorize]
        public ActionResult Index()
        {
            AdministratorIndexViewModel viewModel = _administratorService.Index();
            return View(viewModel);
        }


        [NoCache]
        public ActionResult Create()
        {
            return PartialView("_CreateAdministrator", new AdministratorViewModel());
        }

        [HttpPost]
        public ActionResult Create(AdministratorEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            AdministratorListItemViewModel viewModel = _administratorService.Create(editModel);
            return PartialView("_AdministratorTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(string id) // <-- For the Administrator entity, the id is the username
        {
            DeleteConfirmationViewModel viewModel = _administratorService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(string id) // <-- For the Administrator entity, the id is the username
        {
            try
            {
                _administratorService.Delete(id);
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
