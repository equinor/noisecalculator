using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;

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

            HelicopterNoiseProtectionDefinition definition = new HelicopterNoiseProtectionDefinition();
            definition.SystemName = form.Title;

            _helicopterNoiseProtectionDefinitionDAO.Store(definition);
            
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;
            viewModel.HasTranslationSupport = true;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            HelicopterNoiseProtectionDefinition definition = _helicopterNoiseProtectionDefinitionDAO.Get(id);
            
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;
            viewModel.UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("Create", "HelicopterNoiseProtection"), definition.Id);
            viewModel.UrlEditTranslation = Url.Action("Edit", "HelicopterNoiseProtection");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDelete", "HelicopterNoiseProtection");
            viewModel.HasTranslationSupport = true;
            
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
    }
}
