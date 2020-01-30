using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DomainEntity;

namespace Dal.Nhibernate.Mapping
{
    public class EmployeeProjectMap : ClassMap<EmployeeProject>
    {
        public EmployeeProjectMap()
        {
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.Post).Length(50);
            References(x => x.Employee).Not.Nullable();
            References(x => x.Project).Not.Nullable();
        }
    }
}
