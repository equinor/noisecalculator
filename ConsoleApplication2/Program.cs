using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Security.Principal;
using Ninject;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.NinjectBootstrapper;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());

            ISelectedTaskDAO selectedTaskDAO = kernel.Get<ISelectedTaskDAO>();
            IEnumerable<SelectedTask> selectedTasks =
                selectedTaskDAO.GetAllChronologically(WindowsIdentity.GetCurrent().Name, DateTime.Now);
            
            Console.ReadLine();
        }
    }
}



//        IEnumerable<ReportItem> reportItems = selectedTasks.Select(i => new ReportItem
//                            {
//                                //Title = i.Title,
//                                //Role = i.Role,
//                                //NoiseProtection = i.NoiseProtection,
//                                //NoiseLevel = i.NoiseLevel.ToString(CultureInfo.InvariantCulture),
//                                //WorkTime = string.Format("{0} t  {1} min", i.Hours, i.Minutes),
//                                //Percentage = i.Percentage.ToString(CultureInfo.InvariantCulture)

//                                //Title = i.Title,
//                                //Role = i.Role,
//                                //NoiseProtection = i.NoiseProtection,
//                                //NoiseLevel = i.NoiseLevel.ToString(CultureInfo.InvariantCulture),
//                                //WorkTime = string.Format("{0} t  {1} min", i.Hours, i.Minutes)

//                                Title = i.Title,
//                                Role = string.Format("{0}{1}{2}{3} t {4} min",
//                                    i.Role.PadRight(20), i.NoiseProtection.PadRight(30), i.NoiseLevel, i.Hours, i.Minutes)
//                            });

//        var report = new Report(reportItems.ToReportSource());
//        report.RenderHints.Orientation = ReportOrientation.Landscape;

//        // Customize the Text Fields
//        report.TextFields.Title = string.Format("Mine oppgaver - {0}", DateTime.Now.Date.ToShortDateString());
//        report.TextFields.SubTitle = "LOL";
//        report.TextFields.Footer = "Fotnotene er de samme som i selve applikasjonen, med eventuelle tillegg hvis ønskelig";

//        // Customize the data fields
//        report.DataFields["Title"].HeaderText = "Oppgave";
//        report.DataFields["Role"].HeaderText = "Rolle";
//        //report.DataFields["NoiseProtection"].HeaderText = "Hørselvern";
//        //report.DataFields["NoiseLevel"].HeaderText = "Støynivå";
//        //report.DataFields["WorkTime"].HeaderText = "Arbeidstid";
//        //report.DataFields["Percentage"].HeaderText = "%";


//        // Render hints allow you to pass additional hints to the reports as they are being rendered
//        report.RenderHints.BooleanCheckboxes = true;

//        //var writer = new DoddleReport.iTextSharp.PdfReportWriter();
//        var writer = new DoddleReport.iTextSharp.PdfReportWriter();

//        const string filePath = @"C:\test\testing.pdf";
//        if(File.Exists(filePath))
//        {
//            File.Delete(filePath);
//        }
//        writer.WriteReport(report, new FileStream(filePath, FileMode.Create));
//    }
//}

//class ReportItem
//{
//    public string Title { get; set; }
//    public string Role { get; set; }
//    //public string NoiseProtection { get; set; }
//    //public string NoiseLevel { get; set; }
//    //public string WorkTime { get; set; }
//    //public string Percentage { get; set; }

//}