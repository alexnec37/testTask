using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using TestApplicationSIBERS.Commands;
using TestApplicationSIBERS.ViewModels;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Dal.Nhibernate;
using DomainEntity;
using TestApplicationSIBERS.DTO;

namespace TestApplicationSIBERS.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        private DelegateCommand _exitCommand;
        private DelegateCommand _showAllProjects;
        private DelegateCommand _showProject;
        private DelegateCommand _showEmployee;
        private DelegateCommand _showAllEmployees;
        private DelegateCommand _showSettings;
        private DelegateCommand _showProjectToEmployees;
        private ObservableCollection<WorkspaceViewModel> _workspaces;

        public MainViewModel()
        {
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        public DelegateCommand ShowProjectToEmployees
        {
            get
            {
                if (_showProjectToEmployees == null)
                {
                    _showProjectToEmployees = new DelegateCommand(ProjectToEmployeesShow);
                }
                return _showProjectToEmployees;
            }
        }

        public ICommand ShowAllProjects
        {
            get
            {
                if (_showAllProjects == null)
                {
                    _showAllProjects = new DelegateCommand(ShowAllProjetcs);
                }
                return _showAllProjects;
            }
        }

        public ICommand ShowProject
        {
            get
            {
                if (_showProject == null)
                {
                    _showProject = new DelegateCommand(ShowAddProject);
                }
                return _showProject;            
            }
        }

        public ICommand ShowSettings
        {
            get
            {
                if (_showSettings == null)
                {
                    _showSettings = new DelegateCommand(ShowSetting);
                }

                return _showSettings;
            }
        }

        public ICommand ShowEmployee
        {
            get
            {
                if (_showEmployee == null)
                {
                    _showEmployee = new DelegateCommand(ShowAddEmployee);
                }

                return _showEmployee;
            }
        }

        public ICommand ShowAllEmployees
        {
            get
            {
                if (_showAllEmployees == null)
                {
                    _showAllEmployees = new DelegateCommand(ShowEmployess);
                }
                return _showAllEmployees;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new DelegateCommand(Exit);
                }
                return _exitCommand;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }

        private void ShowAllProjetcs()
        {
            AllProjectsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllProjectsViewModel)
                as AllProjectsViewModel;

            if (workspace == null)
            {
                workspace = new AllProjectsViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ProjectToEmployeesShow()
        { 
            ProjectToEmployeesViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is ProjectToEmployeesViewModel)
                as ProjectToEmployeesViewModel;

            if (workspace == null)
            {
                workspace = new ProjectToEmployeesViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAddProject()
        {
            ProjectViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is ProjectViewModel)
                as ProjectViewModel;

            if (workspace == null)
            {
                workspace = new ProjectViewModel();
                workspace.EntityAdded += OnProjectAdded;
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowSetting()
        {
            SettingsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is SettingsViewModel)
                as SettingsViewModel;

            if (workspace == null)
            {
                workspace = new SettingsViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAddEmployee()
        {
            EmployeeViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is EmployeeViewModel)
                as EmployeeViewModel;

            if (workspace == null)
            {
                workspace = new EmployeeViewModel();
                workspace.EmployeeAdded += OnEmployeeAdded;
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowEmployess()
        {
            AllEmployeesViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllEmployeesViewModel)
                as AllEmployeesViewModel;

            if (workspace == null)
            {
                workspace = new AllEmployeesViewModel();
                workspace.EmployeeEdit += OnEmployeeEdit;
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);            
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }


        private void OnEmployeeAdded(object sender, EntityAddedEventArgs<Employee> e)
        {
            AllEmployeesViewModel workspace =
               this.Workspaces.FirstOrDefault(vm => vm is AllEmployeesViewModel)
               as AllEmployeesViewModel;

            if (workspace != null)
            {
                workspace.ModifyEntity(e.NewEntity);
            }
        }

        private void OnEmployeeEdit(object sender, EntityAddedEventArgs<EmployeeDTO> e)
        {
            EmployeeViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is EmployeeViewModel)
                as EmployeeViewModel;

            if (workspace != null)
                this.Workspaces.Remove(workspace);
            workspace = new EmployeeViewModel(e.NewEntity);
            workspace.EmployeeAdded += OnEmployeeAdded;
            workspace.InitEmployeeProperties(e.NewEntity.FIO, e.NewEntity.Company);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);            
        }

        private void OnProjectAdded(object sender, EntityAddedEventArgs<Project> e)
        {
            AllProjectsViewModel workspace =
               this.Workspaces.FirstOrDefault(vm => vm is AllProjectsViewModel)
               as AllProjectsViewModel;

            if (workspace != null)
            {
                workspace.ModifyEntity(e.NewEntity);
            }
        }


        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
