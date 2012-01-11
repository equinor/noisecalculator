using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using NoiseCalculator.Infrastructure.NHibernate;
using NoiseCalculator.NinjectBootstrapper;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());
            ISessionFactoryManager sessionFactoryManager = kernel.Get<ISessionFactoryManager>();

            sessionFactoryManager.ExportSchema();
        }
    }
}
