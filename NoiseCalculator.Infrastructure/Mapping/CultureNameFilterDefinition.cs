//using FluentNHibernate.Mapping;

using FluentNHibernate.Mapping;
using NHibernate;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class CultureNameFilterDefinition : FilterDefinition
    {
        public CultureNameFilterDefinition()
        {
            WithName("CultureNameFilter")
                .WithCondition("this_1_.CultureName = :meatballs")
                //.WithCondition("Id > :meatballs")
                .AddParameter("meatballs", NHibernateUtil.String);
                //.AddParameter("meatballs", NHibernateUtil.Int32);

            //WithName("CultureNameFilter").
            //    WithCondition("Id > 1");
            //.AddParameter("cultureName", NHibernateUtil.String);
        }

    }
}
