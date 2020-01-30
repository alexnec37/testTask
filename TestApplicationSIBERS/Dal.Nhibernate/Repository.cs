using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.Linq.Expressions;
using DataAccess.RepositoryInfrastructure;
using NHibernate;
using NHibernate.Linq;

namespace Dal.Nhibernate
{
    public class Repository : IRepository
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            UoW = unitOfWork;
        }

        public void AddEntity<T>(T entity) where T : class
        {
            this.UoW.Add(entity);
        }

        public T UpdateEntity<T>(T entity) where T : class
        {
            this.UoW.Update(entity);
            return entity;
        }

        public void DeleteEntity<T>(T entity) where T : class
        {
            this.UoW.Delete(entity);
        }

        public IQueryable<T> GetList<T>() where T : class
        {
            return GetSession().Query<T>();
        }

        public IQueryable<T> GetList<T>(Expression<Func<T, bool>> query) where T : class
        {
            return GetSession().Query<T>().Where(query);
        }

        public T GetEntity<T>(object primaryKey) where T : class
        {
            return GetSession().Get<T>(primaryKey);
        }

        public IUnitOfWork UoW { get; private set; }

        private ISession GetSession()
        {
            return (ISession)UoW.Orm;
        }
    }
} 
