using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEntity
{
    public class Company : Entity
    {
        public virtual string Name { get; set; }
        public virtual IList<Employee> Employees { get; set; }

        public Company()
        {
            Employees = new List<Employee>();
        }

        public virtual void AddEmployee(Employee employee)
        {
            if (!Employees.Contains(employee))
            {
                employee.Company = this;
                Employees.Add(employee);
            }
        }
    }
}
