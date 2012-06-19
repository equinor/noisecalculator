using System;
using System.Collections.Generic;
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

        
        public ActionResult Index()
        {
            IEnumerable<NoiseProtectionDefinition> definitions = _noiseProtectionDefinitionDAO.GetAll();
            
            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            viewModel.PageTitle = "Noise Protection"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

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

            NoiseProtectionDefinition definition = new NoiseProtectionDefinition();
            definition.SystemName = form.Title;

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
            viewModel.UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("CreateTranslation"), definition.Id);
            viewModel.UrlEditTranslation = Url.Action("EditTranslation");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDeleteTranslation");
            viewModel.HasTranslationSupport = true;
            
            foreach (NoiseProtection noiseProtection in definition.NoiseProtections)
            {
                GenericTranslationViewModel translationViewModel
                    = new GenericTranslationViewModel(noiseProtection.CultureName)
                          {
                              Id = noiseProtection.Id,
                              Title = noiseProtection.Title
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

            NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(id);
            definition.SystemName = form.Title;

            _noiseProtectionDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            NoiseProtectionDefinition definintion = _noiseProtectionDefinitionDAO.Get(id);
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

        public ActionResult EditTranslation(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);

            GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            viewModel.Id = noiseProtection.Id;
            viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            viewModel.Title = noiseProtection.Title;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericTranslation", viewModel);
        }

        [HttpPost]
        public ActionResult EditTranslation(GenericTranslationEditModel form)
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

        public ActionResult ConfirmDeleteTranslation(int id)
        {
            NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = "trans" + noiseProtection.Id;
            viewModel.Title = noiseProtection.Title;
            viewModel.UrlDeleteAction = Url.Action("DeleteTranslation");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
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
