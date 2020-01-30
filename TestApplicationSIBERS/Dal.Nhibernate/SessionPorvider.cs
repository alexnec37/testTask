using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Dal.Nhibernate.Mapping;

namespace Dal.Nhibernate
{
    public class SessionProvider : ISessionProvider
    {
        private IUnitOfWork _uow;
        private static NHibernate.ISessionFactory _sessionFactory;

        static SessionProvider()
        {
            _sessionFactory = Fluently.Configure()
                       .Database(
                            SQLiteConfiguration.Standard
                            .UsingFile(NhHelper.Instance.DbFile))
                                .Mappings(x =>
                                    x.FluentMappings.AddFromAssemblyOf<ProjectMap>())
                        .BuildSessionFactory();
        }

        public IUnitOfWork CurrentUoW
        {
            get
            {
                if (_uow == null)
                {
                    _uow = GetUnitOfWork();
                }

                return _uow;
            }
        }

        private IUnitOfWork GetUnitOfWork()
        {
            ISession session = _sessionFactory.OpenSession();
            return new UnitOfWork(session);
        }
    }
}
