using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.HelicopterTask;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    //[CustomAuthorize]
    public class HelicopterTaskController : Controller
    {
        private readonly IHelicopterTaskDAO _helicopterTaskDAO;
        private readonly IDAO<HelicopterType, int> _helicopterTypeDAO;
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;
        private readonly IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;

        public HelicopterTaskController(IHelicopterTaskDAO helicopterTaskDAO,
                                        IDAO<HelicopterType, int> helicopterTypeDAO,
                                        IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO,
                                        IDAO<HelicopterWorkInterval, int> helicopterWorkIntervalDAO)
        {
            _helicopterTaskDAO = helicopterTaskDAO;
            _helicopterTypeDAO = helicopterTypeDAO;
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
            _helicopterWorkIntervalDAO = helicopterWorkIntervalDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<HelicopterTask> helicopterTasks = _helicopterTaskDAO.GetAll();

            HelicopterTaskIndexViewModel viewModel = new HelicopterTaskIndexViewModel();
            foreach (var helicopterTask in helicopterTasks)
            {
                HelicopterTaskListItemViewModel listItemViewModel 
                    = new HelicopterTaskListItemViewModel
                          {
                              Id = helicopterTask.Id,
                              Helicopter = helicopterTask.HelicopterType.Title,
                              NoiseProtectionDefinition = helicopterTask.HelicopterNoiseProtectionDefinition.SystemName,
                              WorkInterval = helicopterTask.HelicopterWorkInterval.Title,
                              Percentage = helicopterTask.Percentage
                          };
                viewModel.HelicopterTasks.Add(listItemViewModel);
            }

            viewModel.PageTitle = "Helicopter Tasks"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return View("Index", viewModel);
        }


        public ActionResult Create()
        {
            HelicopterTaskViewModel viewModel = new HelicopterTaskViewModel();

            viewModel.Helicopters.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectOptionViewModel(type.Title, type.Id.ToString()));
            }

            viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var noiseProtectionDefinition in _helicopterNoiseProtectionDefinitionDAO.GetAll())
            {
                viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(noiseProtectionDefinition.SystemName, noiseProtectionDefinition.Id.ToString()));
            }

            viewModel.WorkIntervals.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var workInterval in _helicopterWorkIntervalDAO.GetAll())
            {
                viewModel.WorkIntervals.Add(new SelectOptionViewModel(workInterval.Title, workInterval.Id.ToString()));
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateHelicopterTask", viewModel);
        }


        [HttpPost]
        public ActionResult Create(HelicopterTaskEditModel form)
        {
            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(form);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            HelicopterTask helicopterTask = new HelicopterTask();
            helicopterTask.HelicopterType = _helicopterTypeDAO.Get(form.HelicopterTypeId);
            helicopterTask.HelicopterNoiseProtectionDefinition = _helicopterNoiseProtectionDefinitionDAO.Get(form.HelicopterNoiseProtectionDefinitionId);
            helicopterTask.HelicopterWorkInterval = _helicopterWorkIntervalDAO.Get(form.HelicopterWorkIntervalId);
            helicopterTask.Percentage = form.Percentage;

            _helicopterTaskDAO.Store(helicopterTask);

            HelicopterTaskListItemViewModel viewModel
                = new HelicopterTaskListItemViewModel
                      {
                          Id = helicopterTask.Id,
                          Helicopter = helicopterTask.HelicopterType.Title,
                          NoiseProtectionDefinition = helicopterTask.HelicopterNoiseProtectionDefinition.SystemName,
                          WorkInterval = helicopterTask.HelicopterWorkInterval.Title,
                          Percentage = helicopterTask.Percentage
                      };

            return PartialView("_HelicopterTaskTableRow", viewModel);
        }


        public ActionResult Edit(int id)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);

            HelicopterTaskViewModel viewModel = new HelicopterTaskViewModel();
            viewModel.Id = helicopterTask.Id;

            viewModel.Helicopters.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (HelicopterType type in _helicopterTypeDAO.GetAll())
            {
                viewModel.Helicopters.Add(new SelectOptionViewModel(type.Title, type.Id.ToString())
                                              {
                                                  IsSelected = (type.Id == helicopterTask.HelicopterType.Id)
                                              });
            }

            viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var noiseProtectionDefinition in _helicopterNoiseProtectionDefinitionDAO.GetAll())
            {
                viewModel.NoiseProtectionDefinitions.Add(new SelectOptionViewModel(noiseProtectionDefinition.SystemName, noiseProtectionDefinition.Id.ToString())
                                                             {
                                                                 IsSelected = (noiseProtectionDefinition.Id == helicopterTask.HelicopterNoiseProtectionDefinition.Id)
                                                             });
            }

            viewModel.WorkIntervals.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            foreach (var workInterval in _helicopterWorkIntervalDAO.GetAll())
            {
                viewModel.WorkIntervals.Add(new SelectOptionViewModel(workInterval.Title, workInterval.Id.ToString())
                                                {
                                                    IsSelected = (workInterval.Id == helicopterTask.HelicopterWorkInterval.Id)
                                                });
            }

            viewModel.Percentage = helicopterTask.Percentage;

            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditHelicopterTask", viewModel);
        }


        [HttpPost]
        public ActionResult Edit(int id, HelicopterTaskEditModel form)
        {
            ValidationErrorSummaryViewModel validationViewModel = ValidateInput(form);
            if (validationViewModel.ValidationErrors.Count > 0)
            {
                Response.StatusCode = 500;
                return PartialView("_ValidationErrorSummary", validationViewModel);
            }

            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);

            helicopterTask.HelicopterType = _helicopterTypeDAO.Get(form.HelicopterTypeId);
            helicopterTask.HelicopterNoiseProtectionDefinition = _helicopterNoiseProtectionDefinitionDAO.Get(form.HelicopterNoiseProtectionDefinitionId);
            helicopterTask.HelicopterWorkInterval = _helicopterWorkIntervalDAO.Get(form.HelicopterWorkIntervalId);
            helicopterTask.Percentage = form.Percentage;

            _helicopterTaskDAO.Store(helicopterTask);

            HelicopterTaskListItemViewModel viewModel
                = new HelicopterTaskListItemViewModel
                {
                    Id = helicopterTask.Id,
                    Helicopter = helicopterTask.HelicopterType.Title,
                    NoiseProtectionDefinition = helicopterTask.HelicopterNoiseProtectionDefinition.SystemName,
                    WorkInterval = helicopterTask.HelicopterWorkInterval.Title,
                    Percentage = helicopterTask.Percentage
                };

            return PartialView("_HelicopterTaskTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            HelicopterTask helicopterTask = _helicopterTaskDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = helicopterTask.Id.ToString();
            viewModel.Title = helicopterTask.ToString();
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                HelicopterTask helicopterTask = _helicopterTaskDAO.Load(id);
                _helicopterTaskDAO.Delete(helicopterTask);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }



        // COPIED FROM HeliDeckController - NASTY NASTY - DRY DRY DRY
        // COPIED FROM HeliDeckController - NASTY NASTY - DRY DRY DRY
        private ValidationErrorSummaryViewModel ValidateInput(HelicopterTaskEditModel form)
        {
            ValidationErrorSummaryViewModel errorSummaryViewModel = new ValidationErrorSummaryViewModel();

            if (form.HelicopterTypeId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterTypeRequired);
            }

            if (form.HelicopterNoiseProtectionDefinitionId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);
            }

            if (form.HelicopterWorkIntervalId == 0)
            {
                errorSummaryViewModel.ValidationErrors.Add(TaskResources.ValidationErrorHelicopterWorkIntervalRequired);
            }

            if (form.Percentage <= 0)
            {
                errorSummaryViewModel.ValidationErrors.Add("Percentage must be provided."); // <---- TRANSLATE
            }

            return errorSummaryViewModel;
        }
    }
}
