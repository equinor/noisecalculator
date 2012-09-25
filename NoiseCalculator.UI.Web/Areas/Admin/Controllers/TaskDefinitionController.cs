using System;
using System.Web.Mvc;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class TaskDefinitionController : Controller
    {
        private readonly ITaskDefinitionService _taskDefinitionService;
        private readonly IGenericTaskDefinitionService _genericTaskDefinitionService;

        public TaskDefinitionController(ITaskDefinitionService taskDefinitionService, IGenericTaskDefinitionService genericTaskDefinitionService)
        {
            _taskDefinitionService = taskDefinitionService;
            _genericTaskDefinitionService = genericTaskDefinitionService;
        }


        [NoCache]
        public ActionResult Index()
        {
            TaskDefinitionIndexViewModel viewModel = _taskDefinitionService.Index();
            return View(viewModel);
        }


        public ActionResult Create()
        {
            return PartialView("_CreateTaskDefinition", new NewTaskDefinitionViewModel());
        }


        [HttpPost]
        public ActionResult Create(CreateTaskDefinitionEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            TaskDefinitionListItemViewModel viewModel = _taskDefinitionService.Create(editModel);
            return PartialView("_TaskDefinitionTableRow", viewModel);
        }


        [NoCache]
        public ActionResult ConfirmDelete(int id)
        {
            DeleteConfirmationViewModel viewModel = _taskDefinitionService.DeleteConfirmationForm(id);
            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _taskDefinitionService.Delete(id);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }


        [HttpPost]
        public ActionResult EditGenericTaskDefinition(int id, GenericDefinitionEditModel editModel)
        {
            if (editModel.IsValid() == false)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", new ValidationErrorSummaryViewModel(editModel.GetValidationErrors()));
            }

            TaskDefinitionListItemViewModel viewModel = _genericTaskDefinitionService.Edit(id, editModel);
            return PartialView("_TaskDefinitionTableRow", viewModel);
        }
    }
}
