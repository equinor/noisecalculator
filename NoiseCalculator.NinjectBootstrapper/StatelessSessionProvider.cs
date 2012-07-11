using System;
using NHibernate;
using Ninject.Activation;
using NoiseCalculator.Infrastructure.NHibernate;

namespace NoiseCalculator.NinjectBootstrapper
{
    public class StatelessSessionProvider : IProvider
    {
        private readonly ISessionFactoryManager _sessionFactoryManager;

        public StatelessSessionProvider(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }
        
        public object Create(IContext context)
        {
            return _sessionFactoryManager.OpenStatelessSession();
        }

        public Type Type
        {
            get { return typeof (IStatelessSession); }
        }
    }
}
