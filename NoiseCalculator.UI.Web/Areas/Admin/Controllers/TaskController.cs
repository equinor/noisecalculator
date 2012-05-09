using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Task;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class TaskController : Controller
    {
        private IDAO<TaskDefinition, int> _taskDefinitionDAO;

        public TaskController(IDAO<TaskDefinition, int> taskDefinitionDAO)
        {
            _taskDefinitionDAO = taskDefinitionDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<TaskDefinition> definitions = _taskDefinitionDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var definition in definitions)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel { Id = definition.Id, SystemName = definition.SystemName });
            }

            viewModel.PageTitle = "Tasks"; // <---- TRANSLATIION!
            viewModel.UrlCreate = Url.Action("Create");
            viewModel.UrlEdit = Url.Action("Edit");
            viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            TaskDefinitionViewModel viewModel = new TaskDefinitionViewModel();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_CreateTaskDefinition", viewModel);
        }

        [HttpPost]
        public ActionResult Create(GenericDefinitionEditModel form)
        {
            if (string.IsNullOrEmpty(form.Title))
            {
                Response.StatusCode = 500;
                return Json("FAIL!");
            }

            TaskDefinition definition = new TaskDefinition();
            definition.SystemName = form.Title;

            _taskDefinitionDAO.Store(definition);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel();
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;

            return PartialView("_GenericDefinitionTableRow", viewModel);
        }

        public ActionResult Edit(int id)
        {
            TaskDefinition definition = _taskDefinitionDAO.Get(id);

            TaskDefinitionViewModel viewModel 
                = new TaskDefinitionViewModel();
            
            viewModel.Id = definition.Id;
            viewModel.SystemName = definition.SystemName;
            viewModel.UrlCreateTranslation = string.Format("{0}/{1}", Url.Action("CreateTranslation"), definition.Id);
            viewModel.UrlEditTranslation = Url.Action("EditTranslation");
            viewModel.UrlDeleteTranslationConfirmation = Url.Action("ConfirmDeleteTranslation");

            foreach (Task task in definition.Tasks)
            {
                TaskListItemViewModel translationViewModel
                    = new TaskListItemViewModel(task.CultureName)
                    {
                        Id = task.Id,
                        Title = task.Title
                    };

                viewModel.Tasks.Add(translationViewModel);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            return PartialView("_EditGenericDefinition", viewModel);
        }

        public ActionResult ConfirmDelete(int id)
        {
            return new EmptyResult();
        }
        
        // ------------------------------------------------------------
        public ActionResult CreateTranslation(int id)
        {
            //GenericTranslationViewModel viewModel = new GenericTranslationViewModel(Thread.CurrentThread.CurrentCulture.Name);
            //viewModel.DefinitionId = id;

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //return PartialView("_CreateGenericTranslation", viewModel);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CreateTranslation(GenericTranslationEditModel form)
        {
            //NoiseProtectionDefinition definition = _noiseProtectionDefinitionDAO.Get(form.DefinitionId);

            //NoiseProtection noiseProtection = new NoiseProtection();
            //noiseProtection.NoiseProtectionDefinition = definition;
            //noiseProtection.Title = form.Title;
            //noiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED
            //definition.NoiseProtections.Add(noiseProtection);

            //_noiseProtectionDefinitionDAO.Store(definition);

            //GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            //viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            //viewModel.Id = noiseProtection.Id;
            //viewModel.Title = noiseProtection.Title;

            //return PartialView("_GenericTranslationTableRow", viewModel);
            return new EmptyResult();
        }

        public ActionResult EditTranslation(int id)
        {
            //NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);

            //GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            //viewModel.Id = noiseProtection.Id;
            //viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            //viewModel.Title = noiseProtection.Title;

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //return PartialView("_EditGenericTranslation", viewModel);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EditTranslation(GenericTranslationEditModel form)
        {
            //NoiseProtection noiseProtection = _noiseProtectionDAO.Get(form.Id);
            //noiseProtection.Title = form.Title;
            //noiseProtection.CultureName = form.SelectedCultureName; // Add validation - REQUIRED

            //_noiseProtectionDAO.Store(noiseProtection);

            //GenericTranslationViewModel viewModel = new GenericTranslationViewModel(noiseProtection.CultureName);
            //viewModel.DefinitionId = noiseProtection.NoiseProtectionDefinition.Id;
            //viewModel.Id = noiseProtection.Id;
            //viewModel.Title = noiseProtection.Title;

            //return PartialView("_GenericTranslationTableRow", viewModel);
            return new EmptyResult();
        }

        // Refactor... Common CreateTranslationViewModel method

        public ActionResult ConfirmDeleteTranslation(int id)
        {
            //NoiseProtection noiseProtection = _noiseProtectionDAO.Get(id);
            //DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel();
            //viewModel.Id = "trans" + noiseProtection.Id;
            //viewModel.Title = noiseProtection.Title;
            //viewModel.UrlDeleteAction = Url.Action("DeleteTranslation");

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //return PartialView("_DeleteConfirmation", viewModel);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
        {
            //try
            //{
            //    NoiseProtection noiseProtection = _noiseProtectionDAO.Load(id);
            //    _noiseProtectionDAO.Delete(noiseProtection);
            //    return new EmptyResult();
            //}
            //catch (Exception ex)
            //{
            //    Response.StatusCode = 500;
            //    return Json(ex.ToString());
            //}
            return new EmptyResult();
        }

    }
}
