using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class HelicopterTaskController : Controller
    {
        private IHelicopterTaskDAO _helicopterTaskDAO;

        public HelicopterTaskController(IHelicopterTaskDAO helicopterTaskDAO)
        {
            _helicopterTaskDAO = helicopterTaskDAO;
        }


        public ActionResult Index()
        {
            IEnumerable<HelicopterTask> helicopterTasks = _helicopterTaskDAO.GetAll();

            return new EmptyResult();
        }

    }
}
