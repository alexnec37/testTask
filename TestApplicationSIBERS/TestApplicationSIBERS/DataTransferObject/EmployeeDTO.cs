using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;

namespace TestApplicationSIBERS.DTO
{
    public class EmployeeDTO
    {
        public Guid ID { get; set; }
        public string FIO { get; set; }
        public CompanyDTO Company { get; set; }
    }
}
