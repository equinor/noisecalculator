using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class NoiseProtectionDefinitionController : Controller
    {
        private readonly IDAO<NoiseProtectionDefinition, int> _noiseProtectionDefinitionDAO;

        public NoiseProtectionDefinitionController(IDAO<NoiseProtectionDefinition, int> noiseProtectionDefinitionDAO)
        {
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
            viewModel.UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("Create", "NoiseProtection"), definition.Id);
            viewModel.UrlEditTranslation = Url.Action("Edit", "NoiseProtection");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDelete", "NoiseProtection");
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
            if (string.IsNullOrEmpty(form.Title))
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

    }
}
