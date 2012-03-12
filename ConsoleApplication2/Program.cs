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

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            //IDAO<Task, int> taskDAO = kernel.Get<IDAO<Task, int>>();
            //var entity = taskDAO.Get(15);
            //string lol = "lol";

            ISelectedTaskDAO _selectedTaskDAO = kernel.Get<ISelectedTaskDAO>();
            var list = _selectedTaskDAO.GetAllChronologically

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