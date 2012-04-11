using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.Models;

namespace NoiseCalculator.UI.Web.Areas.Admin.Controllers
{
    public class NoiseProtectionController : Controller
    {
        private INoiseProtectionDAO _noiseControllerDAO;

        public NoiseProtectionController(INoiseProtectionDAO noiseProtectionDAO)
        {
            _noiseControllerDAO = noiseProtectionDAO;
        }

        
        //
        // GET: /Admin/NoiseProtection/

        public ActionResult Index()
        {
            IEnumerable<NoiseProtection> noiseProtections = _noiseControllerDAO.GetAllByCultureName(Thread.CurrentThread.CurrentCulture.Name);
            NoiseProtectionIndexViewModel viewModel = new NoiseProtectionIndexViewModel(noiseProtections, Thread.CurrentThread.CurrentCulture.Name);
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Search(string cultureName)
        {
            // Validate empty or NULL string
            
            IEnumerable<NoiseProtection> noiseProtections = _noiseControllerDAO.GetAllByCultureName(cultureName);
            NoiseProtectionIndexViewModel viewModel = new NoiseProtectionIndexViewModel(noiseProtections, cultureName);

            return View("Index", viewModel);
        }

    }
}
