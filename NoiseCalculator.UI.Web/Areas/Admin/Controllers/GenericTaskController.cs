using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.GenericTask;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class GenericTaskController : Controller
    {
        private readonly IDAO<TaskDefinition, int> _taskDefinitionDAO;
        private readonly ITaskDAO _taskDAO;
        private readonly IRoleDAO _roleDAO;
        private readonly INoiseProtectionDAO _noiseProtectionDAO;


        public GenericTaskController(IDAO<TaskDefinition, int> taskDefinitionDAO, ITaskDAO taskDAO, IRoleDAO roleDAO, INoiseProtectionDAO noiseProtectionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
            _taskDAO = taskDAO;
            _roleDAO = roleDAO;
            _noiseProtectionDAO = noiseProtectionDAO;
        }


        public ActionResult CreateTranslation(int id)
        {
            TaskViewModel viewModel = new TaskViewModel(Thread.CurrentThread.CurrentCulture.Name);
            viewModel.DefinitionId = id;

            viewModel.Roles.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (Role role in _roleDAO.GetAllFilteredByCurrentCulture())
            {
                // We want separate handling for rotation tasks, as the view should be quite different
                if(role.RoleType != RoleTypeEnum.Rotation)
                {
                    viewModel.Roles.Add(new SelectOptionViewModel(role.Title, role.Id.ToString()));
                }
            }

            viewModel.NoiseProtections.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (NoiseProtection noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                viewModel.NoiseProtections.Add(new SelectOptionViewModel(noiseProtection.Title, noiseProtection.Id.ToString()));
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateTask", viewModel);
        }

        [HttpPost]
        public ActionResult CreateTranslation(TaskEditModel form)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(form.DefinitionId);

            Task task = new Task()
                            {
                                Title = form.Title,
                                Role = _roleDAO.Get(form.RoleId),
                                NoiseProtection = _noiseProtectionDAO.Get(form.NoiseProtectionId),
                                NoiseLevelGuideline = form.NoiseLevelGuideline,
                                AllowedExposureMinutes = form.AllowedExposureMinutes,
                                TaskDefinition = definition,
                                CultureName = form.SelectedCultureName,
                            };
            
            definition.Tasks.Add(task);

            _taskDefinitionDAO.Store(definition);

            TaskListItemViewModel viewModel = CreateTaskListItemViewModel(task);
            return PartialView("_TaskTableRow", viewModel);
        }

        public ActionResult EditTranslation(int id)
        {
            Task task = _taskDAO.Get(id);

            IList<SelectOptionViewModel> roles = new List<SelectOptionViewModel>();
            IList<SelectOptionViewModel> noiseProtections = new List<SelectOptionViewModel>();

            roles.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (Role role in _roleDAO.GetAllFilteredByCurrentCulture())
            {
                // We want separate handling for rotation tasks, as the view should be quite different
                if (role.RoleType != RoleTypeEnum.Rotation)
                {
                    roles.Add(new SelectOptionViewModel(role.Title, role.Id.ToString()) { IsSelected = (role.Id == task.Role.Id) });
                }
            }

            noiseProtections.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (NoiseProtection noiseProtection in _noiseProtectionDAO.GetAllFilteredByCurrentCulture())
            {
                noiseProtections.Add(new SelectOptionViewModel(noiseProtection.Title, noiseProtection.Id.ToString()){ IsSelected = (noiseProtection.Id == task.NoiseProtection.Id)});
            }

            TaskViewModel viewModel = new TaskViewModel(task.CultureName)
                                          {
                                              Id = task.Id,
                                              Title = task.Title,
                                              NoiseLevelGuideline = task.NoiseLevelGuideline,
                                              AllowedExposureMinutes = task.AllowedExposureMinutes,
                                              DefinitionId = task.TaskDefinition.Id,
                                              Roles = roles,
                                              NoiseProtections = noiseProtections
                                          };

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditTask", viewModel);
        }

        [HttpPost]
        public ActionResult EditTranslation(TaskEditModel form)
        {
            Task task = _taskDAO.Get(form.Id);
            
            task.Title = form.Title;
            task.NoiseLevelGuideline = form.NoiseLevelGuideline;
            task.AllowedExposureMinutes = form.AllowedExposureMinutes;
            task.Role = _roleDAO.Get(form.RoleId);
            task.NoiseProtection = _noiseProtectionDAO.Get(form.NoiseProtectionId);
            
            _taskDAO.Store(task);

            TaskListItemViewModel viewModel = CreateTaskListItemViewModel(task);
            return PartialView("_TaskTableRow", viewModel);
        }

        public ActionResult ConfirmDeleteTranslation(int id)
        {
            Task task = _taskDAO.Get(id);
            
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = "trans" + task.Id;
            viewModel.Title = task.Title;
            viewModel.UrlDeleteAction = Url.Action("DeleteTranslation");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
        {
            try
            {
                Task task = _taskDAO.Load(id);
                _taskDAO.Delete(task);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }


        private TaskListItemViewModel CreateTaskListItemViewModel(Task task)
        {
            TaskListItemViewModel taskListItemViewModel = new TaskListItemViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Role = task.Role.Title,
                NoiseProtection = task.NoiseProtection.Title,
                NoiseLevelGuideline = task.NoiseLevelGuideline,
                AllowedExposureMinutes = task.AllowedExposureMinutes,
                Language = LanguageResolver.GetLanguageName(task.CultureName)
            };

            return taskListItemViewModel;
        }


        // -----------------------------------------------------------------
        // -----------------------------------------------------------------
        public ActionResult EditTaskDefinition(int id)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);

            TaskDefinitionGenericViewModel viewModel
                = new TaskDefinitionGenericViewModel
                {
                    Id = definition.Id,
                    SystemName = definition.SystemName,
                    UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("CreateTranslation"), definition.Id),
                    UrlEditTranslation = Url.Action("EditTranslation"),
                    UrlDeleteTranslationConfirmation = Url.Action("ConfirmDeleteTranslation")
                };

            foreach (Task task in definition.Tasks)
            {
                TaskListItemViewModel translationViewModel
                    = new TaskListItemViewModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Role = task.Role.Title,
                        NoiseProtection = task.NoiseProtection.Title,
                        NoiseLevelGuideline = task.NoiseLevelGuideline,
                        AllowedExposureMinutes = task.AllowedExposureMinutes,
                        Language = LanguageResolver.GetLanguageName(task.CultureName)
                    };

                viewModel.Tasks.Add(translationViewModel);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditTaskDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult EditTaskDefinition(int id, GenericDefinitionEditModel form)
        {
            if (string.IsNullOrEmpty(form.Title))
            {
                return new EmptyResult();
            }

            TaskDefinition definition = _taskDefinitionDAO.Get(id);
            definition.SystemName = form.Title;

            _taskDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }

    }
}
