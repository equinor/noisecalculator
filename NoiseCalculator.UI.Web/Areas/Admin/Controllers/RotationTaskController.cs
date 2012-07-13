using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class RotationTaskController : Controller
    {
        private readonly IRotationDAO _rotationDAO;
        private readonly ITaskDAO _taskDAO;
        private readonly ITaskDefinitionDAO _taskDefinitionDAO;
        private readonly IRoleDAO _roleDAO;

        public RotationTaskController(IRotationDAO rotationDAO, 
                                      ITaskDAO taskDAO, 
                                      ITaskDefinitionDAO taskDefinitionDAO,
                                      IRoleDAO roleDAO)
        {
            _rotationDAO = rotationDAO;
            _taskDAO = taskDAO;
            _taskDefinitionDAO = taskDefinitionDAO;
            _roleDAO = roleDAO;
        }

        public ActionResult Create(int id)
        {
            RotationTaskViewModel viewModel = CreateFormViewModel(id);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateRotationTask", viewModel);
        }


        [HttpPost]
        public ActionResult Create(RotationTaskEditModel form)
        {
            Rotation rotation = new Rotation()
                {
                    OperatorTask = _taskDAO.Get(form.OperatorTaskId),
                    AssistantTask = _taskDAO.Get(form.AssistantTaskId)
                };
            
            Task task = new Task()
            {
                CultureName = form.SelectedCultureName,
                Title = form.Title,
                AllowedExposureMinutes = 0,
                NoiseLevelGuideline = 0,
                TaskDefinition = _taskDefinitionDAO.Load(form.TaskDefinitionId),
                Role = _roleDAO.Get("Rotation", Thread.CurrentThread.CurrentCulture.Name),
                NoiseProtection = rotation.OperatorTask.NoiseProtection
            };

            rotation.Task = task;

            _rotationDAO.Store(rotation);

            RotationTaskListItemViewModel viewModel = CreateTableRowViewModel(rotation);

            return PartialView("_RotationTaskTableRow", viewModel);
        }


        public ActionResult Edit(int id)
        {
            Rotation rotation = _rotationDAO.Get(id);

            RotationTaskViewModel viewModel = CreateFormViewModel(rotation);
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditRotationTask", viewModel);
        }


        [HttpPost]
        public ActionResult Edit(int id, RotationTaskEditModel form)
        {
            Rotation rotation = _rotationDAO.Get(id);
            rotation.OperatorTask = _taskDAO.Get(form.OperatorTaskId);
            rotation.AssistantTask = _taskDAO.Get(form.AssistantTaskId);
            rotation.Task.Title = form.Title;
            rotation.Task.CultureName = form.SelectedCultureName;

            _rotationDAO.Store(rotation);

            RotationTaskListItemViewModel viewModel = CreateTableRowViewModel(rotation);

            return View("_RotationTaskTableRow", viewModel);
        }

        private RotationTaskListItemViewModel CreateTableRowViewModel(Rotation rotation)
        {
            return new RotationTaskListItemViewModel
                {
                    Id = rotation.Id,
                    Language = LanguageResolver.GetLanguageName(rotation.Task.CultureName),
                    Title = rotation.Task.Title,
                    OperatorTask = rotation.OperatorTask.Title,
                    OperatorRole = rotation.OperatorTask.Role.Title,
                    AssistantTask = rotation.AssistantTask.Title,
                    AssistantRole = rotation.AssistantTask.Role.Title
                };
        }



        // -------------------------------------------------------
        // -------------------------------------------------------
        public ActionResult EditTaskDefinition(int id)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);
            
            TaskDefinitionRotationViewModel viewModel
                = new TaskDefinitionRotationViewModel
                {
                    Id = definition.Id,
                    SystemName = definition.SystemName,
                    UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("Create"), definition.Id),
                    UrlEditTranslation = Url.Action("Edit"),
                    UrlDeleteTranslationConfirmation = Url.Action("ConfirmDelete")
                };

            IList<Rotation> rotations = _rotationDAO.GetAllByTaskDefinitionIdOrderedByTaskTitle(definition.Id);
            foreach (Rotation rotation in rotations)
            {
                RotationTaskListItemViewModel translationViewModel
                    = new RotationTaskListItemViewModel()
                    {
                        Id = rotation.Id,
                        Title = rotation.Task.Title,
                        OperatorTask = rotation.OperatorTask.Title,
                        OperatorRole = rotation.OperatorTask.Role.Title,
                        AssistantTask = rotation.AssistantTask.Title,
                        AssistantRole = rotation.AssistantTask.Role.Title,
                        Language = LanguageResolver.GetLanguageName(rotation.Task.CultureName)
                    };

                viewModel.RotationTasks.Add(translationViewModel);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditTaskDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult EditTaskDefinition(int id, RotationTaskEditModel form)
        {
            return new EmptyResult();
        }
        
        public ActionResult ConfirmDelete(int id)
        {
            Rotation rotation = _rotationDAO.Get(id);

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = "trans" + rotation.Id;
            viewModel.Title = rotation.Task.Title;
            viewModel.UrlDeleteAction = Url.Action("DeleteTranslation");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
        {
            try
            {
                Rotation rotation = _rotationDAO.Load(id);
                _rotationDAO.Delete(rotation);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }



        private RotationTaskViewModel CreateFormViewModel(int taskDefinitionId)
        {
            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList(null);
            RotationTaskViewModel viewModel = new RotationTaskViewModel(languages) { TaskDefinitionId = taskDefinitionId };

            AddTaskListsToViewModel(viewModel, null);

            return viewModel;
        }

        private RotationTaskViewModel CreateFormViewModel(Rotation rotation)
        {
            IList<SelectOptionViewModel> languages = new LanguageListBuilder().CreateSelectedLanguageList(rotation.Task.CultureName);
            RotationTaskViewModel viewModel = new RotationTaskViewModel(languages)
            {
                Id = rotation.Id,
                TaskDefinitionId = rotation.Task.TaskDefinition.Id,
                Title = rotation.Task.Title
            };

            AddTaskListsToViewModel(viewModel, rotation);

            return viewModel;
        }

        private void AddTaskListsToViewModel(RotationTaskViewModel viewModel, Rotation rotation)
        {
            viewModel.OperatorTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            viewModel.AssistantTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));

            foreach (Task task in _taskDAO.GetAllOrdered().Where(x => x.Role.RoleType == RoleTypeEnum.Regular))
            {
                SelectOptionViewModel selectOption = new SelectOptionViewModel(task.Title, task.Id.ToString(CultureInfo.InvariantCulture));

                if (task.Role.SystemTitle == "Operator")
                {
                    selectOption.IsSelected = (rotation != null && task.Id == rotation.OperatorTask.Id);
                    viewModel.OperatorTasks.Add(selectOption);
                }
                else if (task.Role.SystemTitle == "Assistant")
                {
                    selectOption.IsSelected = (rotation != null && task.Id == rotation.AssistantTask.Id);
                    viewModel.AssistantTasks.Add(selectOption);
                }
            }
        }

    }
}
