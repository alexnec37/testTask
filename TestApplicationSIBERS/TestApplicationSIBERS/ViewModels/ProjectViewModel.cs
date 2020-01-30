using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;
using DataAccess.RepositoryInfrastructure;
using Dal.Nhibernate;
using DataAccess;
using TestApplicationSIBERS.DTO;
using System.Windows.Input;
using TestApplicationSIBERS.Commands;
using System.ComponentModel;
using BL.Validation;

namespace TestApplicationSIBERS.ViewModels
{
    public class ProjectViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private IValidator _projValidator;
        private Project _project;
        private IRepository _repository;

        public event EventHandler<EntityAddedEventArgs<Project>> EntityAdded;

        public ProjectViewModel()
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            _customers = _repository.GetList<Company>().
                Select(x => new CompanyDTO() { ID = x.ID, Name = x.Name }).ToList<CompanyDTO>();
            _contractors = _repository.GetList<Company>().
                Select(x => new CompanyDTO() { ID = x.ID, Name = x.Name }).ToList<CompanyDTO>();
            _projValidator = new ProjectValidator();
            _project = new Project();
        }

        public string DisplayName
        {
            get { return "Проект"; }
        }

        private string _nameProject;
        public string NameProject
        {
            get { return _nameProject; }
            set
            {
                if (_nameProject == value)
                {
                    return;
                }

                _nameProject = value;
                OnPropertyChanged("NameProject");
            }
        }

        private IList<CompanyDTO> _customers;
        public IList<CompanyDTO> Customers
        {
            get
            {
                return _customers;
            }
        }

        private CompanyDTO _customer;
        public CompanyDTO Customer
        {
            get
            {
                return _customer;
            }

            set
            {
                _customer = value;
            }
        }

        private IList<CompanyDTO> _contractors;
        public IList<CompanyDTO> Contractors
        {
            get
            {
                return _contractors;
            }
        }

        private CompanyDTO _contractor;
        public CompanyDTO Contractor
        {
            get
            {
                return _contractor;
            }

            set
            {
                _contractor = value;
            }
        }

        public enum Priority { frozen, low, middle, high }
        private static Dictionary<Priority, string> _priorities;
        public Dictionary<Priority, string> Priorities
        {
            get
            {
                return _priorities ?? (_priorities = new Dictionary<Priority, string>
                                               {
                                                   {Priority.frozen, "Заморожен"},
                                                   {Priority.low, "Низкий"},
                                                   {Priority.middle, "Средний"},
                                                   {Priority.high, "Высокий"}
                                               });

            }
        }

        private Priority _priorityKey;
        public Priority PriorityKey 
        { 
            get { return _priorityKey; }
            set
            {
                _priorityKey = value;
            } 
        }

        private DateTime _dateBegin = DateTime.Now.Date;
        public DateTime DateBegin
        {
            get
            {
                return _dateBegin;
            }

            set
            {
                if (_dateBegin == value)
                {
                    return;
                }

                _dateBegin = value;
                OnPropertyChanged("DateBegin");
                OnPropertyChanged("DateEnd");
            }
        }

        private DateTime _dateEnd = DateTime.Now.Date;
        public DateTime DateEnd
        {
            get
            {
                return _dateEnd;
            }

            set
            {
                if (_dateEnd == value)
                {
                    return;
                }

                _dateEnd = value;
                OnPropertyChanged("DateBegin");
                OnPropertyChanged("DateEnd");
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
            _project.Name = NameProject;
            _project.Priority = (int)PriorityKey;
            _project.BeginProject = DateBegin;
            _project.EndProject = DateEnd;
            _project.Customer = _repository.GetEntity<Company>(_customer.ID);
            _project.Contractor = _contractor == null ? null : _repository.GetEntity<Company>(_contractor.ID);

            _repository.UoW.BeginTransaction();
            _repository.AddEntity<Project>(_project);
            _repository.UoW.CommitTransaction();

            if (this.EntityAdded != null)
                this.EntityAdded(this, new EntityAddedEventArgs<Project>(_project));
            DoEmtyProject();
        }

        private void DoEmtyProject()
        {
            _project = new Project();
            _nameProject = null;
            OnPropertyChanged("NameProject");
            _customer = null;
            OnPropertyChanged("Customer");
            _contractor = null;
            OnPropertyChanged("Contractor");
            _dateBegin = DateTime.Now.Date;
            OnPropertyChanged("DateBegin");
            _dateEnd = DateTime.Now.Date;
            OnPropertyChanged("DateEnd");
            _priorityKey = Priority.frozen;
            OnPropertyChanged("PriorityKey");
        }

        private bool CanSave()
        {
            return !_projValidator.HasErrors();
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
                case "NameProject":
                    error = ValidateProject("NameProject", _nameProject);
                    break;
                case "Customer":
                    error = ValidateProject("Customer", _customer);
                    break;
            }
            
            CommandManager.InvalidateRequerySuggested();
            return error;
        }


        private string ValidateProject(string propertyName, object value)
        {
            if (!_projValidator.IsPropertyValid(propertyName, value))
                return _projValidator.GetErrorMessageForProperty(propertyName);
            return null;
        }
    }
}
