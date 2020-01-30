using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;
using DataAccess;
using DataAccess.RepositoryInfrastructure;
using Dal.Nhibernate;

namespace BL.BusinessLayer
{
    public class BaseBL<T> where T : Entity
    {
        public BaseBL(T entity)
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            Repository = new Repository(unitOfWork);
            Entity = entity;
        }

        protected IRepository Repository { get; set; }
        protected T Entity { get; private set; }
    }
}
