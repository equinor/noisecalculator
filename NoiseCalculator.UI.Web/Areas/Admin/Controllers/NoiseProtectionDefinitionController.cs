using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class NoiseProtectionDefinitionController : Controller
    {
        private readonly INoiseProtectionDefinitionService _noiseProtectionDefinitionService;

        public NoiseProtectionDefinitionController(INoiseProtectionDefinitionService noiseProtectionDefinitionService)
        {
            _noiseProtectionDefinitionService = noiseProtectionDefinitionService;
        }


        [NoCache]
        public ActionResult Index()
        {
            GenericDefinitionIndexViewModel viewModel = _noiseProtectionDefinitionService.Index();
            return View(viewModel);
        }


        [NoCache]
        public ActionResult Create()
        {
            return PartialView("_CreateGenericDefinition", new GenericDefinitionViewModel());
        }


        [HttpPost]
        public ActionResult Create(GenericDefinitionEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            GenericDefinitionViewModel viewModel = _noiseProtectionDefinitionService.Create(editModel);
            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            GenericDefinitionViewModel viewModel = _noiseProtectionDefinitionService.EditNoiseProtectionForm(id);
            viewModel.UrlCreateTranslation = Url.Action("Create", "NoiseProtection");
            viewModel.UrlEditTranslation = Url.Action("Edit", "NoiseProtection");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDelete", "NoiseProtection");
            
            return PartialView("_EditGenericDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, GenericDefinitionEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            GenericDefinitionViewModel viewModel = _noiseProtectionDefinitionService.Edit(id, editModel);
            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _noiseProtectionDefinitionService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _noiseProtectionDefinitionService.Delete(id);
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
