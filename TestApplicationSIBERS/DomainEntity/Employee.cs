using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEntity
{
    public class Employee : Entity
    {
        public virtual string FIO { get; set; }
        public virtual Company Company { get; set; }
        public virtual IList<EmployeeProject> EmpContracts { get; set; }

        public Employee()
        {
            EmpContracts = new List<EmployeeProject>();
        }

        public virtual void AddEmpContracts(EmployeeProject performer)
        {
            if (!EmpContracts.Contains(performer))
            {
                performer.Employee = this;
                EmpContracts.Add(performer);
            }
        }

        public virtual void RemoveEmpContracts(EmployeeProject performer)
        {
            if (EmpContracts.Contains(performer))
            {
                EmpContracts.Remove(performer);
            }
        }
    }
}
