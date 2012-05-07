using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class HelicopterTypeController : Controller
    {
        private IDAO<HelicopterType, int> _helicopterTypeDAO;

        public HelicopterTypeController(IDAO<HelicopterType,int> helicopterTypeDAO)
        {
            _helicopterTypeDAO = helicopterTypeDAO;
        }

        
        public ActionResult Index()
        {
            IEnumerable<HelicopterType> helicopterTypes = _helicopterTypeDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var helicopterType in helicopterTypes)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = helicopterType.Id, SystemName = helicopterType.Title });
            }

            viewModel.PageTitle = "Helicopter Type"; // <---- TRANSLATIION!
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

            HelicopterType helicopterType = new HelicopterType();
            helicopterType.Title = form.Title;

            _helicopterTypeDAO.Store(helicopterType);
            
            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterType.Id;
            viewModel.SystemName = helicopterType.Title;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult Edit(int id)
        {
            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterType.Id;
            viewModel.SystemName = helicopterType.Title;
            viewModel.HasTranslationSupport = false;

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

            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);
            helicopterType.Title = form.Title;

            _helicopterTypeDAO.Store(helicopterType);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterType.Id;
            viewModel.SystemName = helicopterType.Title;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            HelicopterType helicopterType = _helicopterTypeDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = helicopterType.Id.ToString();
            viewModel.Title = helicopterType.Title;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                HelicopterType helicopterWorkInterval = _helicopterTypeDAO.Load(id);
                _helicopterTypeDAO.Delete(helicopterWorkInterval);
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
