using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DomainEntity;

namespace Dal.Nhibernate.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.FIO).Not.Nullable().Length(70);
            References(x => x.Company).Not.Nullable().Cascade.All();
            HasMany(x => x.EmpContracts);
        }
    }
}
