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
                .WithCondition("this.CultureName = :meatballs")
                //.WithCondition("this_1_.CultureName = :meatballs")
                //.WithCondition("CultureName = :meatballs")
                .AddParameter("meatballs", NHibernateUtil.String);
        }

    }
}
