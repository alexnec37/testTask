using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEntity
{
    public class Project : Entity
    {
        public virtual string Name { get; set; }
        public virtual int Priority { get; set; }
        public virtual DateTime BeginProject { get; set; }
        public virtual DateTime EndProject { get; set; }
        public virtual Company Customer { get; set; }
        public virtual Company Contractor { get; set; }
        public virtual IList<EmployeeProject> Performers { get; set; }

        public Project()
        {
            Performers = new List<EmployeeProject>();
        }

        public virtual void AddPerformers(EmployeeProject performer)
        {
            if (!Performers.Contains(performer))
            {
                performer.Project = this;
                Performers.Add(performer);
            }
        }

        public virtual void RemovePreformers(EmployeeProject performer)
        {
            if (Performers.Contains(performer))
            {
                Performers.Remove(performer);
            }
        }
    }
}
