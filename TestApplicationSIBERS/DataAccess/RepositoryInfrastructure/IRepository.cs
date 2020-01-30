using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DataAccess.RepositoryInfrastructure
{
    public interface IRepository
    {
        IUnitOfWork UoW { get; }

        void AddEntity<T>(T entity) where T : class;

        T UpdateEntity<T>(T entity) where T : class;

        void DeleteEntity<T>(T entity) where T : class;

        IQueryable<T> GetList<T>() where T : class;

        IQueryable<T> GetList<T>(Expression<Func<T, bool>> query) where T : class;

        T GetEntity<T>(object primaryKey) where T : class;
    }
}
