using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Ninject;
using NoiseCalculator.Domain;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.NHibernate;
using NoiseCalculator.NinjectBootstrapper;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());
            //ISessionFactoryManager sessionFactoryManager = kernel.Get<ISessionFactoryManager>();
            ISession session = kernel.Get<ISession>();
            
            session.Transaction.Begin();

            //sessionFactoryManager.ExportSchema();
            Role operatr = session.QueryOver<Role>().Where(x => x.Title == "Operator").SingleOrDefault();
            Role assistant = session.QueryOver<Role>().Where(x => x.Title == "Assistant").SingleOrDefault();
            Role helideck = session.QueryOver<Role>().Where(x => x.Title == "Helideck").SingleOrDefault();
            Role rotation = session.QueryOver<Role>().Where(x => x.Title == "Rotation").SingleOrDefault();

            operatr.RoleType = RoleTypeEnum.Regular;
            assistant.RoleType = RoleTypeEnum.Regular;
            helideck.RoleType = RoleTypeEnum.Helideck;
            rotation.RoleType = RoleTypeEnum.Rotation;
            
            session.Transaction.Commit();
        }
    }
}

