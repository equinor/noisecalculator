using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Ninject.Activation;
using NoiseCalculator.Infrastructure.NHibernate;

namespace NoiseCalculator.NinjectBootstrapper
{
    public class SessionProvider : IProvider
    {
        private readonly ISessionFactoryManager _sessionFactoryManager;

        public SessionProvider(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }
        
        public object Create(IContext context)
        {
            return _sessionFactoryManager.OpenSession();
        }

        public Type Type
        {
            get { return typeof (ISession); }
        }
    }
}
