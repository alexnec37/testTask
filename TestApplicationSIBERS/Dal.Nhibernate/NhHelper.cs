using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Dal.Nhibernate.Mapping;
using DomainEntity;
using DataAccess;
using DataAccess.RepositoryInfrastructure;

namespace Dal.Nhibernate
{
    public class NhHelper
    {
        private static readonly Lazy<NhHelper> _lazyInstance =
             new Lazy<NhHelper>(() => new NhHelper());

        private IList<Company> _companyList;

        public static NhHelper Instance
        {
            get { return _lazyInstance.Value; }
        }

        public void CreateDB()
        {
            CreateSessionFactory();
            CreateCompanyList();
        }

        public string DbFile { get { return "ControlProject.db"; } }
        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                SQLiteConfiguration.Standard
                  .UsingFile(DbFile)
              )
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<ProjectMap>())
              .ExposeConfiguration(BuildSchema)
              .BuildSessionFactory();
        }

        private void BuildSchema(Configuration config)
        {
            if (File.Exists(DbFile))
                File.Delete(DbFile);
               new SchemaExport(config)
              .Create(false, true);
        }

        public void CreateData()
        {
            CreateCompanyList();
        }

        private void CreateCompanyList()
        {
            _companyList = new List<Company>();
            Company company1 = new Company() { Name = "Лапоть" };
            company1.AddEmployee(new Employee() { FIO = "Свиридов Иван Афанасьевич" });
            company1.AddEmployee(new Employee() { FIO = "Смирнов Константин Олегович" });
            company1.AddEmployee(new Employee() { FIO = "Приходько Кондрат Виниаминович" });
            _companyList.Add(company1);
            Company company2 = new Company() { Name = "Корат" };
            company2.AddEmployee(new Employee() { FIO = "Иваськова Таисия Алексеевна" });
            company2.AddEmployee(new Employee() { FIO = "Уварова Ефросинья Фёдоровна" });
            company2.AddEmployee(new Employee() { FIO = "Замулина Миланья Пафнутьевна" });
            _companyList.Add(company2);
            Company company3 = new Company() { Name = "Омега" };
            company3.AddEmployee(new Employee() { FIO = "Жлобова Авдотья Петровна" });
            company3.AddEmployee(new Employee() { FIO = "Акеньшина Пелагея Игнатова" });
            company3.AddEmployee(new Employee() { FIO = "Целиков Пётр Леонидович" });
            _companyList.Add(company3);
            Company company4 = new Company() { Name = "Железный занавес" };
            company4.AddEmployee(new Employee() { FIO = "Шмаков Артём Игоревич" });
            _companyList.Add(company4);
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            IRepository repository = new Repository(unitOfWork);
            unitOfWork.BeginTransaction();
            foreach (Company companyItem in _companyList)
                repository.AddEntity<Company>(companyItem);
            unitOfWork.CommitTransaction();
        }
    }
}
