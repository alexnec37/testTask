using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dal.Nhibernate
{
    public class EntityAddedEventArgs<T> : EventArgs
    {
        public EntityAddedEventArgs(T newEntity)
        {
            this.NewEntity = newEntity;
        }

        public T NewEntity { get; private set; }
    }
}
