using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public interface IUnitOfWork
    {

        object Orm { get; }

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
