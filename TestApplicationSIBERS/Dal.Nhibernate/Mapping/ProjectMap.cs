using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DomainEntity;

namespace Dal.Nhibernate.Mapping
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable().Length(50);
            Map(x => x.Priority).Not.Nullable();
            Map(x => x.BeginProject);
            Map(x => x.EndProject);
            References(x => x.Contractor).Cascade.All();
            References(x => x.Customer).Not.Nullable().Cascade.All();
            HasMany(x => x.Performers);
        }
    }
}
