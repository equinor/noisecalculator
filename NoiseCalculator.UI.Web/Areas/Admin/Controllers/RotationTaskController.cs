﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
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

        public RotationTaskController(IRotationDAO rotationDAO, 
                                        ITaskDAO taskDAO,
                                        ITaskDefinitionDAO taskDefinitionDAO)
        {
            _rotationDAO = rotationDAO;
            _taskDAO = taskDAO;
            _taskDefinitionDAO = taskDefinitionDAO;
        }


        public ActionResult Create(int id)
        {
            RotationTaskViewModel viewModel = new RotationTaskViewModel()
            {
                //TaskDefinitionId = id;
            };

            viewModel.OperatorTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            viewModel.AssistantTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));

            foreach (Task task in _taskDAO.GetAllOrdered().Where(x => x.Role.RoleType == RoleTypeEnum.Regular))
            {
                SelectOptionViewModel selectOption = new SelectOptionViewModel(task.Title, task.Id.ToString(CultureInfo.InvariantCulture));

                if (task.Role.SystemTitle == "Operator")
                {
                    viewModel.OperatorTasks.Add(selectOption);
                }
                else if (task.Role.SystemTitle == "Assistant")
                {
                    viewModel.AssistantTasks.Add(selectOption);
                }
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateRotationTask", viewModel);
        }

        [HttpPost]
        public ActionResult Create(RotationTaskEditModel form)
        {
            //ValidationErrorSummaryViewModel validationViewModel = ValidateInput(form);
            //if (validationViewModel.ValidationErrors.Count > 0)
            //{
            //    Response.StatusCode = 500;
            //    return PartialView("_ValidationErrorSummary", validationViewModel);
            //}


            // FOR TESTING!
            string cultureName = Thread.CurrentThread.CurrentCulture.Name;

            //Task task = new Task()
            //    {
            //        Title = form.Title,
            //        AllowedExposureMinutes = 0,
            //        NoiseLevelGuideline = 0,
            //        TaskDefinition = _taskDefinitionDAO.Load(form.TaskDefinitionId),
            //        Role = _roleDAO.Get("Rotation", cultureName),
            //        NoiseProtection = _noiseProtection
            //    };

            //Rotation rotationTask = new Rotation()
            //    {
            //        Task = task,
            //        OperatorTask = _taskDAO.Get(form.OperatorTaskId),
            //        AssistantTask = _taskDAO.Get(form.OperatorTaskId)
            //    };

            //_rotationDAO.Store(rotationTask);

            //HelicopterTaskListItemViewModel viewModel
            //    = new HelicopterTaskListItemViewModel
            //    {
            //        Id = helicopterTask.Id,
            //        Helicopter = helicopterTask.HelicopterType.Title,
            //        NoiseProtectionDefinition = helicopterTask.HelicopterNoiseProtectionDefinition.Username,
            //        WorkInterval = helicopterTask.HelicopterWorkInterval.Title,
            //        Percentage = helicopterTask.Percentage
            //    };

            //return PartialView("_HelicopterTaskTableRow", viewModel);
            return new EmptyResult();
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
                    //UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("CreateTranslation"), definition.Id),
                    //UrlEditTranslation = Url.Action("EditTranslation"),
                    //UrlDeleteTranslationConfirmation = Url.Action("ConfirmDeleteTranslation")
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


    }
}
