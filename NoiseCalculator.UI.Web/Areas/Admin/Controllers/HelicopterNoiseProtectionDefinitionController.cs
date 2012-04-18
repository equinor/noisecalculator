using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class HelicopterNoiseProtectionDefinitionController : Controller
    {
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;

        public HelicopterNoiseProtectionDefinitionController(IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO)
        {
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
        }

        
        public ActionResult Index()
        {
            IEnumerable<HelicopterNoiseProtectionDefinition> helicopterNoiseProtectionDefinitions = _helicopterNoiseProtectionDefinitionDAO.GetAll();
            
            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in helicopterNoiseProtectionDefinitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            viewModel.PageTitle = "Helicopter Noise Protection Definitions"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");
            viewModel.UrlDeleteDefinition = Url.Action("Delete");

            return View(viewModel);
        }

        public ActionResult Create()
        {
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            return PartialView("_CreateNoiseProtectionDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult Create(GenericDefinitionEditModel form)
        {
            if(string.IsNullOrEmpty(form.SystemName))
            {
                Response.StatusCode = 500;
                return Json("FAIL!");
            }

            NoiseProtectionDefinition definition = new NoiseProtectionDefinition();
            definition.SystemName = form.SystemName;

            _noiseProtectionDefinitionDAO.Store(definition);
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(id);
            
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditNoiseProtectionDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, GenericDefinitionEditModel form)
        {
            if(string.IsNullOrEmpty(form.SystemName))
            {
                return new EmptyResult();
            }

            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(id);
            definition.SystemName = form.SystemName;

            _noiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            NoiseProtectionDefinition noiseProtectionDefinition = _noiseProtectionDefinitionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel(noiseProtectionDefinition);
            
            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                NoiseProtectionDefinition noiseProtectionDefinition = _noiseProtectionDefinitionDAO.Load(id);
                _noiseProtectionDefinitionDAO.Delete(noiseProtectionDefinition);
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
