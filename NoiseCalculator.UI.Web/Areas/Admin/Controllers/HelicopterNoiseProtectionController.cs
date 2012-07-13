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
    public class HelicopterNoiseProtectionController : Controller
    {
        private readonly IDAO<HelicopterNoiseProtection, int> _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;

        public HelicopterNoiseProtectionController(IDAO<HelicopterNoiseProtection, int> helicopterNoiseProtectionDAO, IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO)
        {
            _helicopterNoiseProtectionDAO = helicopterNoiseProtectionDAO;
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
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
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(form.DefinitionId);
            
            HelicopterNoiseProtection helicopterNoiseProtection = new HelicopterNoiseProtection();
            helicopterNoiseProtection.HelicopterNoiseProtectionDefinition = definition;
            helicopterNoiseProtection.Title = form.Title;
            helicopterNoiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED
            definition.HelicopterNoiseProtections.Add(helicopterNoiseProtection);

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);
            
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName);
            viewModel.DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id;
            viewModel.Id = helicopterNoiseProtection.Id;
            viewModel.Title = helicopterNoiseProtection.Title;

            return PartialView("_GenericTranslationTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName);
            viewModel.Id = helicopterNoiseProtection.Id;
            viewModel.DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id;
            viewModel.Title = helicopterNoiseProtection.Title;
            viewModel.FormActionUrl = Url.Action("Edit");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(GenericTranslationEditModel form)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(form.Id);
            helicopterNoiseProtection.Title = form.Title;
            helicopterNoiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED

            _helicopterNoiseProtectionDAO.Store(helicopterNoiseProtection);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName);
            viewModel.DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id;
            viewModel.Id = helicopterNoiseProtection.Id;
            viewModel.Title = helicopterNoiseProtection.Title;

            return PartialView("_GenericTranslationTableRow", viewModel);
        }

        public ActionResult ConfirmDelete(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = "trans" + helicopterNoiseProtection.Id;
            viewModel.Title = helicopterNoiseProtection.Title;
            viewModel.UrlDeleteAction = Url.Action("DeleteTranslation");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
        {
            try
            {
                HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);
                _helicopterNoiseProtectionDAO.Delete(helicopterNoiseProtection);
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
