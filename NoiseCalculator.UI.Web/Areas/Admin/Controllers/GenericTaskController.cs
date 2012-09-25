using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class GenericTaskController : Controller
    {
        private readonly IGenericTaskService _genericTaskService;
        private readonly IGenericTaskDefinitionService _genericTaskDefinitionService;

        public GenericTaskController(IGenericTaskService genericTaskService, IGenericTaskDefinitionService genericTaskDefinitionService)
        {
            _genericTaskService = genericTaskService;
            _genericTaskDefinitionService = genericTaskDefinitionService;
        }


        [NoCache]
        public ActionResult Create(int id)
        {
            TaskViewModel viewModel = _genericTaskService.CreateGenericTaskForm(id);
            return PartialView("_CreateTask", viewModel);
        }

        [HttpPost]
        public ActionResult Create(TaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }
            
            TaskListItemViewModel viewModel = _genericTaskService.Create(editModel);
            return PartialView("_TaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult Edit(int id)
        {
            TaskViewModel viewModel = _genericTaskService.EditGenericTaskForm(id);
            return PartialView("_EditTask", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(TaskEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }
            
            TaskListItemViewModel viewModel = _genericTaskService.Edit(editModel);
            return PartialView("_TaskTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _genericTaskService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _genericTaskService.Delete(id);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }

        [NoCache]
        public ActionResult EditGenericTaskDefinition(int id)
        {
            TaskDefinitionGenericViewModel viewModel = _genericTaskDefinitionService.EditGenericTaskDefinitionForm(id);
            return PartialView("_EditGenericTaskDefinition", viewModel);
        }
    }
}
