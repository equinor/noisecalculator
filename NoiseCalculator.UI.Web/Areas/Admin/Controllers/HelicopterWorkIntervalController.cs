using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class HelicopterWorkIntervalController : Controller
    {
        private IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;

        public HelicopterWorkIntervalController(IDAO<HelicopterWorkInterval,int> helicopterWorkIntervalDAO)
        {
            _helicopterWorkIntervalDAO = helicopterWorkIntervalDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<HelicopterWorkInterval> helicopterWorkIntervals = _helicopterWorkIntervalDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var helicopterWorkInterval in helicopterWorkIntervals)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = helicopterWorkInterval.Id, SystemName = helicopterWorkInterval.Title });
            }

            viewModel.PageTitle = "Helicopter Work Interval"; // <---- TRANSLATIION!
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

            HelicopterWorkInterval helicopterWorkInterval = new HelicopterWorkInterval();
            helicopterWorkInterval.Title = form.Title;
            
            _helicopterWorkIntervalDAO.Store(helicopterWorkInterval);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterWorkInterval.Id;
            viewModel.SystemName = helicopterWorkInterval.Title;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult Edit(int id)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterWorkInterval.Id;
            viewModel.SystemName = helicopterWorkInterval.Title;
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

            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);
            helicopterWorkInterval.Title = form.Title;

            _helicopterWorkIntervalDAO.Store(helicopterWorkInterval);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = helicopterWorkInterval.Id;
            viewModel.SystemName = helicopterWorkInterval.Title;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }


        public ActionResult ConfirmDelete(int id)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);
            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            viewModel.Id = helicopterWorkInterval.Id.ToString();
            viewModel.Title = helicopterWorkInterval.Title;
            viewModel.UrlDeleteAction = Url.Action("Delete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Load(id);
                _helicopterWorkIntervalDAO.Delete(helicopterWorkInterval);
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
