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
using NHibernate;
using System.Threading;
//using DoddleReport;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());

            //ISelectedTaskDAO selectedTaskDAO = kernel.Get<ISelectedTaskDAO>();
            //IEnumerable<SelectedTask> selectedTasks =
            //    selectedTaskDAO.GetAllChronologically(WindowsIdentity.GetCurrent().Name, DateTime.Now);

            ISession session = kernel.Get<ISession>();
            session.EnableFilter("CultureNameFilter");
            IFilter filter = session.GetEnabledFilter("CultureNameFilter");
            filter.SetParameter("meatballs", Thread.CurrentThread.CurrentCulture.Name);
            
            var list = session.QueryOver<Task>().List();
            //var entity = session.Get<Task>(15);
        }
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