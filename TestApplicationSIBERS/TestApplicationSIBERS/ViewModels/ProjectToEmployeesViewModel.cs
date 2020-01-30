using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;
using TestApplicationSIBERS.DTO;
using DataAccess.RepositoryInfrastructure;
using DataAccess;
using Dal.Nhibernate;
using TestApplicationSIBERS.Commands;
using System.Windows.Input;
using TestApplicationSIBERS.Dialog;
using BL.BusinessLayer;

namespace TestApplicationSIBERS.ViewModels
{
    public class ProjectToEmployeesViewModel : WorkspaceViewModel
    {
        private IRepository _repository;
        private ProjectDTO _projectDTO;
        private EmployeeDTO _employeeDTONoProj;
        private EmployeeDTO _employeeDTOInProj;
        private Project _project;

        private DelegateCommand _addInProject;
        private DelegateCommand _removeInProject;

        private event EventHandler _changeProject;

        public IList<ProjectDTO> ProjectsDTO { get; set; }
        public IList<EmployeeDTO> EmployeesNoProject { get; set; }
        public IList<EmployeeDTO> EmployeesInProject { get; set; }

        public ProjectToEmployeesViewModel()
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            ProjectsDTO = _repository.GetList<Project>()
                .Select(x => new ProjectDTO() { ID = x.ID, Name = x.Name })
                .OrderBy(x => x.Name).ToList<ProjectDTO>();
            _changeProject += ChangeProject;
        }

        public string DisplayName
        {
            get { return "Сотрудники проекта"; }
        }

        public ProjectDTO ProjectDTO
        {
            get { return _projectDTO; }
            set
            {
                if (_projectDTO == value)
                    return;
                _projectDTO = value;
                if (this._changeProject != null)
                    this._changeProject(this, null);
            }
        }

        public EmployeeDTO SelectedEmployeeDTONoProj
        {
            get { return _employeeDTONoProj; }
            set
            {
                if (_employeeDTONoProj == value)
                    return;
                _employeeDTONoProj = value;
            }
        }

        public EmployeeDTO SelectedEmployeeDTOInProj
        {
            get { return _employeeDTOInProj; }
            set
            {
                if (_employeeDTOInProj == value)
                    return;
                _employeeDTOInProj = value;
            }
        }

        public ICommand AddInProject
        {
            get
            {
                if (_addInProject == null)
                {
                    _addInProject = new DelegateCommand(AddEmployeeInProject, CanAddEmployeeInProject);
                }
                return _addInProject;
            }
        }


        public ICommand RemoveInProject
        {
            get
            {
                if (_removeInProject == null)
                {
                    _removeInProject = new DelegateCommand(RemoveEmployeeInProject, CanRemoveEmployeeInProject);
                }
                return _removeInProject;
            }
        }

        private void RemoveEmployeeInProject()
        {
            EmployeeProject empProj = _repository.GetList<EmployeeProject>()
                .Where(x => 
                    (x.Employee.ID == _employeeDTOInProj.ID) 
                    && (x.Project.ID == _projectDTO.ID))
                .FirstOrDefault<EmployeeProject>();
            _project = _repository.GetEntity<Project>(_projectDTO.ID);
            Employee employee = _repository.GetEntity<Employee>(_employeeDTOInProj.ID);
            _repository.UoW.BeginTransaction();
            _repository.UoW.Delete(empProj);
            _repository.UoW.CommitTransaction();
            if (this._changeProject != null)
                this._changeProject(this, null);
        }

        private bool CanRemoveEmployeeInProject()
        {
            return _employeeDTOInProj != null;
        }

        private void AddEmployeeInProject()
        {
            Employee employee = _repository.GetEntity<Employee>(_employeeDTONoProj.ID);          
            EmployeeBL canAddEmployee = new EmployeeBL(employee);
            if (!canAddEmployee.CanAddEmployeeInProject())
            {
                IDialogService dg = new DialogService();
                dg.ShowMessage("Внимание", "Нельзя назначать более 3-х \n проектов одному сотруднику");
                return;
            }

            _project = _repository.GetEntity<Project>(_projectDTO.ID);  

            EmployeeProject empProj = new EmployeeProject() 
                { 
                    Employee = employee,
                    Project = _project,
                    Post = "МЕГААА БОСС"
                };
            
            _repository.UoW.BeginTransaction();
            _repository.UoW.Add(empProj);
            _repository.UoW.CommitTransaction();
            if (this._changeProject != null)
                this._changeProject(this, null);
        }

        private bool CanAddEmployeeInProject()
        {
                return _employeeDTONoProj != null;
        }

        private void ChangeProject(object o, EventArgs e)
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            IRepository repository = new Repository(unitOfWork);
            EmployeesInProject = repository.GetEntity<Project>(_projectDTO.ID).Performers
                .Select(x => new EmployeeDTO()
                {
                    ID = x.Employee.ID,
                    FIO = x.Employee.FIO,
                    Company = new CompanyDTO()
                    {
                        ID = x.Employee.Company.ID,
                        Name = x.Employee.Company.Name
                    }
                }).OrderBy(x => x.FIO).ToList<EmployeeDTO>();
             
            OnPropertyChanged("EmployeesInProject");

            var lstEmp = repository.GetList<Employee>().ToList<Employee>();
            var lstEmpPorj = repository.GetEntity<Project>(_projectDTO.ID)
                .Performers
                .Select(x => x.Employee).ToList<Employee>();
            var lstResult = lstEmp.Except(lstEmpPorj).ToList<Employee>();
            EmployeesNoProject = lstEmp.Except(lstEmpPorj).Select(x => new EmployeeDTO()
                {
                    ID = x.ID,
                    FIO = x.FIO,
                    Company = new CompanyDTO() { ID = x.Company.ID, Name = x.Company.Name }
                }
                ).OrderBy(x => x.FIO).ToList<EmployeeDTO>();
            OnPropertyChanged("EmployeesNoProject");

            _employeeDTONoProj = null;
        }
    }
}
