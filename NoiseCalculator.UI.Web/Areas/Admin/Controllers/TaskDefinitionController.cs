using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.TaskDefinition;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class TaskDefinitionController : Controller
    {
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;

        public TaskDefinitionController(ITaskDefinitionDAO taskDefinitionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<TaskDefinition> definitions = _taskDefinitionDAO.GetAllOrdered();

            TaskDefinitionIndexViewModel viewModel = new TaskDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                TaskDefinitionListItemViewModel taskDefinitionListItemView = new TaskDefinitionListItemViewModel()
                    {
                        Id = definition.Id,
                        SystemName = definition.SystemName,
                        RoleType = definition.RoleType.ToString()
                    };
                viewModel.Definitions.Add(taskDefinitionListItemView);
            }

            viewModel.PageTitle = "Tasks Definitions"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            //viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlEditGeneric = Url.Action("Edit", "GenericTask");
            viewModel.UrlEditRotation = Url.Action("Edit", "RotationTask");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return View(viewModel);
        }


        public ActionResult Create()
        {
            return PartialView("_CreateTaskDefinition", new NewTaskDefinitionViewModel());
        }


        [HttpPost]
        public ActionResult Create(CreateTaskDefinitionEditModel form)
        {
            if (string.IsNullOrEmpty(form.Title))
            {
                Response.StatusCode = 500;
                return Json("FAIL!");
            }

            TaskDefinition definition = new TaskDefinition();
            definition.SystemName = form.Title.ToUpper();
            definition.RoleType = (RoleTypeEnum) Enum.Parse(typeof (RoleTypeEnum), form.RoleType);
            _taskDefinitionDAO.Store(definition);

            TaskDefinitionListItemViewModel viewModel = new TaskDefinitionListItemViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;
            viewModel.RoleType = definition.RoleType.ToString();
            
            return PartialView("_TaskDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            TaskDefinition definintion = _taskDefinitionDAO.Get(id);
            
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = definintion.Id.ToString();
            viewModel.Title = definintion.SystemName;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                TaskDefinition noiseProtectionDefinition = _taskDefinitionDAO.Load(id);
                _taskDefinitionDAO.Delete(noiseProtectionDefinition);
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
