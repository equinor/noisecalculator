using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;


namespace NoiseCalculator.Controllers
{
    public class TaskFormRequestModel
    {
        public int selectedTask { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly IDAO<Task, int> _taskDAO;

        public HomeController(IDAO<Task,int> taskDAO)
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
        public PartialViewResult AddTask(TaskFormRequestModel taskFormRequestModel)
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

        public PartialViewResult AddTaskRotation(Task task)
        {
            return PartialView("_TaskFormRotation");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
