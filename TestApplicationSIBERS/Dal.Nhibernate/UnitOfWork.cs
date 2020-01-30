using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using NHibernate;

namespace Dal.Nhibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITransaction tx;

        public UnitOfWork(ISession orm)
        {
            this.Orm = orm;
        }

        #region IUnitOfWork Members

        public object Orm { get; private set; }

        public void Add<T>(T entity) where T : class
        {
            try
            {
                ((ISession)Orm).SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the Add method.", ex);
            }
        }

        public void Update<T>(T entity) where T : class
        {
            try
            {
                ((ISession)Orm).Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the Update method.", ex);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                ((ISession)Orm).Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the Update method.", ex);
            }
        }

        public void BeginTransaction()
        {
            try
            {
                if ((tx == null) || (tx.WasCommitted) || (tx.WasRolledBack))
                {
                    tx = ((ISession)Orm).BeginTransaction();
                }
                else
                {
                    if (tx.IsActive)
                    {
                        throw new Exception("The transaction is already open.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the Begin Tx method.", ex);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (tx == null)
                {
                    throw new Exception("The current transaction has not been initialized.");
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the Commit Tx method.", ex);
            }
        }

        public void RollbackTransaction()
        {
            if (tx == null)
            {
                throw new Exception("The current transaction has not been initialized.");
            }
            tx.Rollback();
        }

        #endregion
    }
}
