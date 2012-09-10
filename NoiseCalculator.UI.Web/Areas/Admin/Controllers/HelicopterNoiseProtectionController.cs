using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class HelicopterNoiseProtectionController : Controller
    {
        private readonly IHelicopterNoiseProtectionService _helicopterNoiseProtectionService;

        public HelicopterNoiseProtectionController(IHelicopterNoiseProtectionService helicopterNoiseProtectionService)
        {
            _helicopterNoiseProtectionService = helicopterNoiseProtectionService;
        }


        [NoCache]
        public ActionResult Create(int id)
        {
            GenericTranslationViewModel viewModel = _helicopterNoiseProtectionService.CreateHelicopterNoiseProtectionForm(id);
            return PartialView("_CreateGenericTranslation", viewModel);
        }


        [HttpPost]
        public ActionResult Create(GenericTranslationEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            GenericTranslationViewModel viewModel = _helicopterNoiseProtectionService.Create(editModel);
            return PartialView("_GenericTranslationTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            GenericTranslationViewModel viewModel = _helicopterNoiseProtectionService.EditHelicopterNoiseProtiectionForm(id);
            return PartialView("_EditGenericTranslation", viewModel);
        }


        [HttpPost]
        public ActionResult Edit(GenericTranslationEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            GenericTranslationViewModel viewModel = _helicopterNoiseProtectionService.Edit(editModel);
            return PartialView("_GenericTranslationTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _helicopterNoiseProtectionService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _helicopterNoiseProtectionService.Delete(id);
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
