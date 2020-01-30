using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;

namespace TestApplicationSIBERS.DTO
{
    public class ProjectDTO
    {
        public Guid ID { get; set; }
        public virtual string Name { get; set; }
        public virtual int Priority { get; set; }
        public virtual DateTime EndProject { get; set; }
        public virtual CompanyDTO Customer { get; set; }
        public virtual CompanyDTO Contractor { get; set; }
    }
}
