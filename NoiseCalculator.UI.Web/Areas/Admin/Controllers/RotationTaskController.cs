using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class RotationTaskController : Controller
    {
        private readonly IRotationTaskService _rotationTaskService;
        private readonly IRotationTaskDefinitionService _rotationTaskDefinitionService;

        public RotationTaskController(IRotationTaskService rotationTaskService, IRotationTaskDefinitionService rotationTaskDefinitionService)
        {
            _rotationTaskService = rotationTaskService;
            _rotationTaskDefinitionService = rotationTaskDefinitionService;
        }


        [NoCache]
        public ActionResult Create(int id)
        {
            RotationTaskViewModel viewModel = _rotationTaskService.CreateRotationTaskForm(id);
            return PartialView("_CreateRotationTask", viewModel);
        }


        [HttpPost]
        public ActionResult Create(RotationTaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }
            
            RotationTaskListItemViewModel viewModel = _rotationTaskService.Create(editModel);
            return PartialView("_RotationTaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            RotationTaskViewModel viewModel = _rotationTaskService.EditRotationTaskForm(id);
            return PartialView("_EditRotationTask", viewModel);
        }


        [HttpPost]
        public ActionResult Edit(int id, RotationTaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }
            
            RotationTaskListItemViewModel viewModel = _rotationTaskService.Edit(id, editModel);
            return View("_RotationTaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _rotationTaskService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _rotationTaskService.Delete(id);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }


        [NoCache]
        public ActionResult EditRotationTaskDefinition(int id)
        {
            TaskDefinitionRotationViewModel viewModel = _rotationTaskDefinitionService.EditGenericTaskDefinitionForm(id);
            return PartialView("_EditRotationTaskDefinition", viewModel);
        }
    }
}
