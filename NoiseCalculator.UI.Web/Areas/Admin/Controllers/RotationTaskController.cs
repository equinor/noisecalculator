using System.Collections.Generic;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class RotationTaskController : Controller
    {
        private readonly IRotationDAO _rotationDAO;

        public RotationTaskController(IRotationDAO rotationDAO)
        {
            _rotationDAO = rotationDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<Rotation> rotations = _rotationDAO.GetAll();

            RotationTaskIndexViewModel viewModel = new RotationTaskIndexViewModel();
            //foreach (Task rotationTask in rotationTasks)
            //{
            //    RotationTaskListItemViewModel listItemViewModel
            //        = new RotationTaskListItemViewModel
            //        {
            //            Id = rotationTask.Id,
            //            Title = rotationTask.Title,
            //            OperatorTask = 
            //        };
            //    viewModel.HelicopterTasks.Add(listItemViewModel);
            //}

            //viewModel.PageTitle = "Helicopter Tasks"; // <---- TRANSLATIION!
            //viewModel.UrlCreate = Url.Action("Create");
            //viewModel.UrlEdit = Url.Action("Edit");
            //viewModel.UrlDeleteConfirmation = Url.Action("ConfirmDelete");

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //return View("Index", viewModel);
            return new EmptyResult();
        }

        
        public ActionResult Create()
        {
            RotationTaskViewModel viewModel = new RotationTaskViewModel();

            viewModel.OperatorTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));
            viewModel.AssistantTasks.Add(new SelectOptionViewModel(TaskResources.SelectOne, "0"));

            //foreach (Task task in _rotationDAO.GetAll())
            //{
            //    viewModel.OperatorTasks.Add(new SelectOptionViewModel(task.Title, task.Id.ToString()));
            //    viewModel.AssistantTasks.Add(new SelectOptionViewModel(task.Title, task.Id.ToString()));
            //}
            
            return PartialView("_CreateRotationTask", viewModel);
        }

    }
}
