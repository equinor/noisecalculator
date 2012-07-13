using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class NoiseProtectionController : Controller
    {
        private readonly IDAO<NoiseProtection,int> _noiseProtectionDAO;
        private readonly IDAO<NoiseProtectionDefinition, int> _noiseProtectionDefinitionDAO;

        public NoiseProtectionController(IDAO<NoiseProtection, int> noiseProtectionDAO, IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO)
        {
            _noiseProtectionDAO = noiseProtectionDAO;
            _noiseProtectionDefinitionDAO = noiseProtectionDefinitionDAO;
        }


        public ActionResult Create(int id)
        {
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(Thread.CurrentThread.CurrentCulture.Name);
            viewModel.DefinitionId = id;
            viewModel.FormActionUrl = Url.Action("Create");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult Create(GenericTranslationEditModel form)
        {
            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(form.DefinitionId);
            
            NoiseProtection noiseProtection = new NoiseProtection();
            noiseProtection.NoiseProtectionDefinition = definition;
            noiseProtection.Title = form.Title;
            noiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED
            definition.NoiseProtections.Add(noiseProtection);

            _noiseProtectionDefinitionDAO.Store(definition);
            
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            viewModel.Id = noiseProtection.Id;
            viewModel.Title = noiseProtection.Title;

            return PartialView("_GenericTranslationTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            viewModel.Id = noiseProtection.Id;
            viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            viewModel.Title = noiseProtection.Title;
            viewModel.FormActionUrl = @Url.Action("Edit");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(GenericTranslationEditModel form)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(form.Id);
            noiseProtection.Title = form.Title;
            noiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED

            _noiseProtectionDAO.Store(noiseProtection);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            viewModel.Id = noiseProtection.Id;
            viewModel.Title = noiseProtection.Title;

            return PartialView("_GenericTranslationTableRow", viewModel);
        }

        // Refactor... Common CreateTranslationViewModel method

        public ActionResult ConfirmDelete(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = "trans" + noiseProtection.Id;
            viewModel.Title = noiseProtection.Title;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                NoiseProtection noiseProtection = _noiseProtectionDAO.Load(id);
                _noiseProtectionDAO.Delete(noiseProtection);
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
