using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TestApplicationSIBERS.DTO;
using BL.Validation;
using DataAccess.RepositoryInfrastructure;
using DataAccess;
using Dal.Nhibernate;
using DomainEntity;
using TestApplicationSIBERS.Commands;
using System.Windows.Input;

namespace TestApplicationSIBERS.ViewModels
{
    public class EmployeeViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private IValidator _employeeValidator;
        private Employee _employee;
        private Repository _repository;

        public event EventHandler<EntityAddedEventArgs<Employee>> EmployeeAdded;
        
        public EmployeeViewModel()
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            _companies = _repository.GetList<Company>().
                Select(x => new CompanyDTO() { ID = x.ID, Name = x.Name }).ToList<CompanyDTO>();
            _employeeValidator = new EmployeeValidator();
            _employee = new Employee();
        }

        public EmployeeViewModel(EmployeeDTO employeeDTO)
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            _companies = _repository.GetList<Company>().
                Select(x => new CompanyDTO() { ID = x.ID, Name = x.Name }).ToList<CompanyDTO>();
            _employeeValidator = new EmployeeValidator();
        }

        public string DisplayName
        {
            get { return "Сотрудник"; }
        }

        private string _fio;
        public string FIO
        {
            get { return _fio; }
            set
            {
                if (_fio == value)
                    return;
                
                _fio = value;
                OnPropertyChanged("FIO");
            }
        }

        private IList<CompanyDTO> _companies;
        public IList<CompanyDTO> Companies
        {
            get
            {
                return _companies;
            }
        }

        private CompanyDTO _company;
        public CompanyDTO Company
        {
            get
            {
                return _company;
            }

            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
        }

        private DelegateCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new DelegateCommand(Save, CanSave);
                }
                return saveCommand;
            }
        }

        private void Save()
        {
            _employee.FIO = FIO;
            _employee.Company = _repository.GetEntity<Company>(_company.ID);

            _repository.UoW.BeginTransaction();
            _repository.AddEntity<Employee>(_employee);
            _repository.UoW.CommitTransaction();

            if (this.EmployeeAdded != null)
                this.EmployeeAdded(this, new EntityAddedEventArgs<Employee>(_employee));
            DoEmtyEmployee();
        }

        private void DoEmtyEmployee()
        {
            _employee = new Employee();
            _fio = null;
            OnPropertyChanged("FIO");
            OnPropertyChanged("Companies");
            _company = null;
            OnPropertyChanged("Company");
        }

        public void InitEmployeeProperties(string fio, CompanyDTO company)
        {
            //_employee = _repository.GetEntity<Employee>(employeeDTO.ID);
            //_company = _companies.Where<CompanyDTO>(x => x.ID == _employee.Company.ID).FirstOrDefault<CompanyDTO>();
            _fio = fio;
            OnPropertyChanged("FIO");
            _company = new CompanyDTO() { ID = company.ID, Name = company.Name };
            //Company = company;
            OnPropertyChanged("Company");
        }

        private bool CanSave()
        {
            return !_employeeValidator.HasErrors();
        }

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        private string GetValidationError(string propertyName)
        {
            string error = null;
            switch (propertyName)
            {
                case "FIO":
                    error = ValidateProject("FIO", _fio);
                    break;
                case "Company":
                    error = ValidateProject("Company", _company);
                    break;
            }
            
            CommandManager.InvalidateRequerySuggested();
            return error;
        }

        private string ValidateProject(string propertyName, object value)
        {
            if (!_employeeValidator.IsPropertyValid(propertyName, value))
                return _employeeValidator.GetErrorMessageForProperty(propertyName);
            return null;
        }
    }
}
