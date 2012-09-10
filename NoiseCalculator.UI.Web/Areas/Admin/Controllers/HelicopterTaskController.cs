using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.HelicopterTask;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class HelicopterTaskController : Controller
    {
        private readonly IHelicopterTaskService _helicopterTaskService;

        public HelicopterTaskController(IHelicopterTaskService helicopterTaskService)
        {
            _helicopterTaskService = helicopterTaskService;
        }


        [NoCache]
        public ActionResult Index()
        {
            HelicopterTaskIndexViewModel viewModel = _helicopterTaskService.Index();
            return View("Index", viewModel);
        }


        [NoCache]
        public ActionResult Create()
        {
            HelicopterTaskViewModel viewModel = _helicopterTaskService.CreateNoiseProtectionForm();
            return PartialView("_CreateHelicopterTask", viewModel);
        }


        [HttpPost]
        public ActionResult Create(HelicopterTaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            HelicopterTaskListItemViewModel viewModel = _helicopterTaskService.Create(editModel);
            return PartialView("_HelicopterTaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            HelicopterTaskViewModel viewModel = _helicopterTaskService.EditNoiseProtectionForm(id);
            return PartialView("_EditHelicopterTask", viewModel);
        }


        [HttpPost]
        public ActionResult Edit(int id, HelicopterTaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            HelicopterTaskListItemViewModel viewModel = _helicopterTaskService.Edit(id, editModel);
            return PartialView("_HelicopterTaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _helicopterTaskService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _helicopterTaskService.Delete(id);
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
