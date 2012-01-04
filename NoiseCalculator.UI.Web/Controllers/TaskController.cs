using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.ViewModels;


namespace NoiseCalculator.Controllers
{
    // Can we replace this with a selectedTask int field in the relevant controller?????
    public class TaskFormRequestModel
    {
        public int selectedTask { get; set; }
    }

    public class TaskController : Controller
    {
        private readonly IDAO<Task, int> _taskDAO;

        public TaskController(IDAO<Task, int> taskDAO)
        {
            _taskDAO = taskDAO;
        }
        
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAll();
            return PartialView("_TaskFormCommon", tasks);
        }

        [HttpPost]
        //public PartialViewResult GetTaskForm(TaskFormRequestModel taskFormRequestModel)
        public ActionResult GetTaskForm(TaskFormRequestModel taskFormRequestModel)
        {
            Task task = _taskDAO.Get(taskFormRequestModel.selectedTask);

            switch (task.Role.Title)
            {
                case "Helideck":
                    {
                        return AddTaskHelideck(task);
                        break;
                    }
                case "Rotation":
                    {
                        return AddTaskRotation(task);
                        break;
                    }
                default:
                    {
                        return AddTaskRegular(task);
                        break;
                    }
            }
        }
        
        // ------------------------------------------------
        public PartialViewResult AddTaskRegular(Task task)
        {
            return PartialView("_TaskFormRegular");
        }

        public PartialViewResult AddTaskHelideck(Task task)
        {
            return PartialView("_TaskFormHelideck", task);
        }

        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModel)
        {
            if(viewModel.HelicopterIdSelected > 1)
            {
                Response.StatusCode = 500;
                return Json("EN FEIL HAR OPPSTÅTT");
            }

            return PartialView("_SelectedTask");
        }


        public PartialViewResult AddTaskRotation(Task task)
        {
            return PartialView("_TaskFormRotation");
        }
    }
}
