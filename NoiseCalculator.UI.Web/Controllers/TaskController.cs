using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskDAO _taskDAO;
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        

        public TaskController(ITaskDAO taskDAO, ISelectedTaskDAO selectedTaskDAO)
        {
            _taskDAO = taskDAO;
            _selectedTaskDAO = selectedTaskDAO;
        }
        

        public ActionResult Index()
        {
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            return View(selectedTasks);
        }


        public PartialViewResult AddTask()
        {
            IEnumerable<Task> tasks = _taskDAO.GetAllOrdered();
            return PartialView("_TaskFormCommon", tasks);
        }


        public ActionResult GetFormForTask(int id)
        {
            Task task = _taskDAO.Get(id);

            switch (task.Role.Title)
            {
                case "Helideck":
                        return AddTaskHelideck(task);
                case "Rotation":
                        return AddTaskRotation(task);
                default:
                        return AddTaskRegular(task);
            }
        }
        

        // ------------------------------------------------
        public PartialViewResult AddTaskRegular(Task task)
        {
            TimeSpan workTimeSpan = new TimeSpan(0, 0, task.ActualExposure, 0);
            RegularViewModel viewModel = new RegularViewModel
                    {
                    TaskId = task.Id,
                    Title = task.Title,
                    Role = task.Role.Title,
                    IsNoiseMeassured = (task.ActualExposure > 0),
                    NoiseLevelMeassured = (task.NoiseLevelMeasured > 0 ? task.NoiseLevelMeasured.ToString() : string.Empty),
                    IsWorkSpecifiedAsTime = (task.ActualExposure > 0),
                    Hours = workTimeSpan.Hours.ToString(),
                    Minutes = workTimeSpan.Minutes.ToString(),
                    IsWorkSpecifiedAsPercentage = false,
                    Percentage = string.Empty
                    };

            return PartialView("_TaskFormRegular", viewModel);
        }

        [HttpPost]
        public PartialViewResult AddTaskRegular(RegularViewModel viewModel)
        {
            return PartialView("_TaskFormRegular");
        }



        public PartialViewResult AddTaskHelideck(Task task)
        {
            HelideckViewModel viewModel = new HelideckViewModel
                                              {
                                                  TaskId = task.Id,
                                                  Title = task.Title,
                                                  Role = task.Role.Title
                                              };

            return PartialView("_TaskFormHelideck", viewModel);
        }


        [HttpPost]
        public ActionResult AddTaskHelideck(HelideckViewModel viewModelHelideck)
        {
            if(string.IsNullOrEmpty(viewModelHelideck.HelicopterId) || int.Parse(viewModelHelideck.HelicopterId) < 1)
            {
                Response.StatusCode = 500;
                return Json("EN FEIL HAR OPPSTÅTT");
            }


            Task task = _taskDAO.Get(viewModelHelideck.TaskId);

            //SelectedTaskViewModel viewModelSelectedTask = new SelectedTaskViewModel();
            //viewModelSelectedTask.Title = string.Format("Helikoptermottak: <Helikopter>");
            //viewModelSelectedTask.Role = task.Role.Title;

            SelectedTask selectedTask = new SelectedTask();
            selectedTask.Title = task.Title;
            selectedTask.Role = task.Role.Title;
            selectedTask.Hours = 13;
            selectedTask.Minutes = 66;
            selectedTask.CreatedBy = User.Identity.Name;
            selectedTask.CreatedDate = DateTime.Now.Date;

            _selectedTaskDAO.Store(selectedTask);

            return PartialView("_SelectedTask", selectedTask);
        }




        public PartialViewResult AddTaskRotation(Task task)
        {
            return PartialView("_TaskFormRotation");
        }
    }
}
