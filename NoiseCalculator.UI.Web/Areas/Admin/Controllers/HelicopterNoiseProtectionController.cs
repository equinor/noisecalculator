using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class HelicopterNoiseProtectionController : Controller
    {
        private readonly IDAO<HelicopterNoiseProtection, int> _helicopterNoiseProtectionDAO;
        private readonly IDAO<HelicopterNoiseProtectionDefinition, int> _helicopterNoiseProtectionDefinitionDAO;

        public HelicopterNoiseProtectionController(IDAO<HelicopterNoiseProtection, int> helicopterNoiseProtectionDAO, IDAO<HelicopterNoiseProtectionDefinition, int> helicopterNoiseProtectionDefinitionDAO)
        {
            _helicopterNoiseProtectionDAO = helicopterNoiseProtectionDAO;
            _helicopterNoiseProtectionDefinitionDAO = helicopterNoiseProtectionDefinitionDAO;
        }

        
        public ActionResult Index()
        {
            IEnumerable<HelicopterNoiseProtectionDefinition> definitions = _helicopterNoiseProtectionDefinitionDAO.GetAll();
            
            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            viewModel.PageTitle = "Helicopter Noise Protection"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            return View(viewModel);
        }

        public ActionResult Create()
        {
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateGenericDefinition", viewModel);
        }


        [HttpPost]
        public ActionResult Create(GenericDefinitionEditModel form)
        {
            if (string.IsNullOrEmpty(form.Title))
            {
                Response.StatusCode = 500;
                return Json("FAIL!");
            }

            HelicopterNoiseProtectionDefinition definition = new HelicopterNoiseProtectionDefinition();
            definition.SystemName = form.Title;

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;
            viewModel.UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("CreateTranslation"), definition.Id);
            viewModel.UrlEditTranslation = Url.Action("EditTranslation");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDeleteTranslation");
            
            foreach (HelicopterNoiseProtection helicopterNoiseProtection in definition.HelicopterNoiseProtections   )
            {
                GenericTranslationViewModel translationViewModel
                    = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName)
                          {
                              Id = helicopterNoiseProtection.Id,
                              Title = helicopterNoiseProtection.Title
                          };

                viewModel.Translations.Add(translationViewModel);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, GenericDefinitionEditModel form)
        {
            if(string.IsNullOrEmpty(form.Title))
            {
                return new EmptyResult();
            }

            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            definition.SystemName = form.Title;

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = definition.Id.ToString();
            viewModel.Title = definition.SystemName;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            return PartialView("_DeleteConfirmation", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Load(id);
                _helicopterNoiseProtectionDefinitionDAO.Delete(definition);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.ToString());
            }
        }


        public ActionResult CreateTranslation(int id)
        {
            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(Thread.CurrentThread.CurrentCulture.Name);
            viewModel.DefinitionId = id;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult CreateTranslation(GenericTranslationEditModel form)
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

        public ActionResult EditTranslation(int id)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _helicopterNoiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(helicopterNoiseProtection.CultureName);
            viewModel.Id = helicopterNoiseProtection.Id;
            viewModel.DefinitionId = helicopterNoiseProtection.HelicopterNoiseProtectionDefinition.Id;
            viewModel.Title = helicopterNoiseProtection.Title;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult EditTranslation(GenericTranslationEditModel form)
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

        public ActionResult ConfirmDeleteTranslation(int id)
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
