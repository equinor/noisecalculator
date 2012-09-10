using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class HelicopterTypeController : Controller
    {
        private readonly IHelicopterTypeService _helicopterTypeService;

        public HelicopterTypeController(IHelicopterTypeService helicopterTypeService)
        {
            _helicopterTypeService = helicopterTypeService;
        }


        [NoCache]
        public ActionResult Index()
        {
            GenericDefinitionIndexViewModel viewModel = _helicopterTypeService.Index();
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

            GenericDefinitionViewModel viewModel = _helicopterTypeService.Create(editModel);
            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            GenericDefinitionViewModel viewModel = _helicopterTypeService.EditHelicopterTypeForm(id);
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
            
            GenericDefinitionViewModel viewModel = _helicopterTypeService.Edit(id, editModel);
            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _helicopterTypeService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _helicopterTypeService.Delete(id);
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
