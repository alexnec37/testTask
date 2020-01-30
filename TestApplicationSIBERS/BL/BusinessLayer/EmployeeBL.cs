using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;
using DataAccess.RepositoryInfrastructure;
using DataAccess;
using Dal.Nhibernate;

namespace BL.BusinessLayer
{
    public class EmployeeBL : BaseBL<Employee>
    {
        public EmployeeBL(Employee employee)
            : base(employee)
        {
        }

        public bool CanAddEmployeeInProject()
        {
            var listContracts = Repository.GetEntity<Employee>(Entity.ID).EmpContracts;
            if (listContracts.Count >= 3)
                return false;
            return true;
        }
    }
}
