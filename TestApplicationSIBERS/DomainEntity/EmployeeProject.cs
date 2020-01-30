using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEntity
{
    public class EmployeeProject : Entity
    {
        public virtual string Post { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
