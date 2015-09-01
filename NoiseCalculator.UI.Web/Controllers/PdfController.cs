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
            reportInfo.CreatedBy = UserHelper.CreateUsernameWithoutDomain(string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name);
            if (reportInfo.CreatedBy != reportInfo.CreatedBy.ToUpper())
                reportInfo.CreatedBy = "";
            var selectedTasks = _selectedTaskDAO.GetAllChronologically(string.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name, DateTime.Now);

            var tasks = selectedTasks as IList<SelectedTask> ?? selectedTasks.ToList();

            if (!tasks.Any()) return RedirectToAction("Index", "Task");
            reportInfo.Footnotes = _footnotesService.CalculateFootnotesForReport(tasks);

            var memoryStream = _pdfExporter.GenerateSelectedTasksPDFPdfSharp(tasks, reportInfo);
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=MyTasks-" + DateTime.Now.Date.ToShortDateString() + ".pdf");
            return new FileStreamResult(memoryStream, "application/pdf");

            // No tasks found - Redirect to main page
        }

    }
}
