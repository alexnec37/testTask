using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.RepositoryInfrastructure;
using System.Collections.ObjectModel;
using TestApplicationSIBERS.DTO;
using DataAccess;
using Dal.Nhibernate;
using DomainEntity;
using System.Collections.Specialized;
using TestApplicationSIBERS.Commands;
using System.Windows.Input;

namespace TestApplicationSIBERS.ViewModels
{
    public class AllEmployeesViewModel : WorkspaceViewModel
    {
        private IRepository _repository;
        public ObservableCollection<EmployeeDTO> AllEmployees { get; private set; }
        private DelegateCommand _editCommand;

        public event EventHandler<EntityAddedEventArgs<EmployeeDTO>> EmployeeEdit;

        public AllEmployeesViewModel()
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            AllEmployees = new ObservableCollection<EmployeeDTO>(_repository.GetList<Employee>()
                .Select(x => new EmployeeDTO()
                    {
                        ID = x.ID,
                        FIO = x.FIO,
                        Company = new CompanyDTO() { ID = x.Company.ID, Name = x.Company.Name }
                    }
                ).OrderBy(x => x.FIO));
        }

        public string DisplayName
        {
            get { return "Сотрудники"; }
        }

        private EmployeeDTO _selectedEmployee;
        public EmployeeDTO SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                if (value == _selectedEmployee)
                    return;
                _selectedEmployee = value;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new DelegateCommand(EditEmployee);
                }
                return _editCommand;
            }
        }

        private void EditEmployee()
        {
            if (this.EmployeeEdit != null)
                this.EmployeeEdit(this, new EntityAddedEventArgs<EmployeeDTO>(_selectedEmployee));
        }


        public void ModifyEntity(Employee employee)
        {
            EmployeeDTO employeeDTO = AllEmployees.Where<EmployeeDTO>(x => x.ID == employee.ID).FirstOrDefault();
            if (employeeDTO == null)
            {
                employeeDTO = new EmployeeDTO()
                {
                    ID = employee.ID,
                    FIO = employee.FIO,
                    Company = new CompanyDTO { ID = employee.Company.ID, Name = employee.Company.Name }
                };
                AllEmployees.Add(employeeDTO);
            }
            else
            {
                AllEmployees.Remove(employeeDTO);
                employeeDTO = new EmployeeDTO()
                {
                    ID = employee.ID,
                    FIO = employee.FIO,
                    Company = new CompanyDTO { ID = employee.Company.ID, Name = employee.Company.Name }
                };
                AllEmployees.Add(employeeDTO);
            }
        }
    }
}
