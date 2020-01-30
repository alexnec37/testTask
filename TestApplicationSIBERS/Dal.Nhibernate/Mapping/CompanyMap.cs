using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DomainEntity;

namespace Dal.Nhibernate.Mapping
{
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable().Length(50);
            HasMany(x => x.Employees).Inverse().Cascade.All();
        }
    }
}
