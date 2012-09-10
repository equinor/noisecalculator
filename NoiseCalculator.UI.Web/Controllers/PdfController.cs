using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.Infrastructure.Pdf;
using NoiseCalculator.UI.Web.Support;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class PdfController : Controller
    {
        private readonly ISelectedTaskDAO _selectedTaskDAO;
        private readonly IFootnotesService _footnotesService;
        private readonly IPdfExporter _pdfExporter;

        public PdfController(ISelectedTaskDAO selectedTaskDAO, IFootnotesService footnotesService, IPdfExporter pdfExporter)
        {
            _selectedTaskDAO = selectedTaskDAO;
            _footnotesService = footnotesService;
            _pdfExporter = pdfExporter;
        }

        public ActionResult PdfReportCurrentTasks(ReportInfo reportInfo)
        {
            reportInfo.CreatedBy = UserHelper.CreateUsernameWithoutDomain(User.Identity.Name);
            IEnumerable<SelectedTask> selectedTasks = _selectedTaskDAO.GetAllChronologically(User.Identity.Name, DateTime.Now);

            if (selectedTasks.Count() > 0)
            {
                reportInfo.Footnotes = _footnotesService.CalculateFootnotes(selectedTasks);

                Stream memoryStream = _pdfExporter.GenerateSelectedTasksPDF(selectedTasks, reportInfo);
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=MyTasks-" + DateTime.Now.Date.ToShortDateString() + ".pdf");
                return new FileStreamResult(memoryStream, "application/pdf");
            }

            // No tasks found - Redirect to main page
            return RedirectToAction("Index", "Task");
        }

    }
}
