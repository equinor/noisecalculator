using System.Collections.Generic;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.Infrastructure.NHibernate;

namespace NoiseCalculator.Controllers
{
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
            return PartialView("_TaskForm", tasks);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
