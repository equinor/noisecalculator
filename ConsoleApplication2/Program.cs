using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Security.Principal;
using DoddleReport;
using Ninject;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.NinjectBootstrapper;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());
            //ISessionFactoryManager sessionFactoryManager = kernel.Get<ISessionFactoryManager>();
            ISelectedTaskDAO selectedTaskDAO = kernel.Get<ISelectedTaskDAO>();
            IEnumerable<SelectedTask> selectedTasks = selectedTaskDAO.GetAllChronologically(WindowsIdentity.GetCurrent().Name, DateTime.Now);

            IEnumerable<ReportItem> reportItems = selectedTasks.Select(i => new ReportItem
                                {
                                    Title = i.Title,
                                    Role = i.Role,
                                    NoiseProtection = i.NoiseProtection,
                                    NoiseLevel = i.NoiseLevel.ToString(CultureInfo.InvariantCulture),
                                    WorkTime = string.Format("{0}t {1}min", i.Hours, i.Minutes),
                                    Percentage = i.Percentage.ToString(CultureInfo.InvariantCulture)
                                });

            var report = new Report(reportItems.ToReportSource());

            // Customize the Text Fields
            report.TextFields.Title = "Products Report";
            report.TextFields.SubTitle = "This is a sample report showing how Doddle Report works";
            report.TextFields.Footer = "Copyright 2011 &copy; The Doddle Project";

            // Render hints allow you to pass additional hints to the reports as they are being rendered
            report.RenderHints.BooleanCheckboxes = true;

            var writer = new DoddleReport.iTextSharp.PdfReportWriter();
            writer.WriteReport(report, new FileStream(@"C:\test\testing.pdf", FileMode.Create));
            
        }
    }

    class ReportItem
    {
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseProtection { get; set; }
        public string NoiseLevel { get; set; }
        public string WorkTime { get; set; }
        public string Percentage { get; set; }

    }
}

