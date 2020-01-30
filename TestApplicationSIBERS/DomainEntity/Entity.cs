using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEntity
{
    public abstract class Entity
    {
        public virtual Guid ID { get; set; }
    }
}
