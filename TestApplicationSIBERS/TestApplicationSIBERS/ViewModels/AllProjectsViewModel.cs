using System.Linq;
using DataAccess.RepositoryInfrastructure;
using Dal.Nhibernate;
using DataAccess;
using System.Collections.ObjectModel;
using TestApplicationSIBERS.DTO;
using DomainEntity;

namespace TestApplicationSIBERS.ViewModels
{
    public class AllProjectsViewModel : WorkspaceViewModel
    {
        private IRepository _repository;
        public ObservableCollection<ProjectDTO> AllProjects { get; private set; }

        public AllProjectsViewModel()
        {
            ISessionProvider factory = new SessionProvider();
            IUnitOfWork unitOfWork = factory.CurrentUoW;
            _repository = new Repository(unitOfWork);
            AllProjects = new ObservableCollection<ProjectDTO>(_repository.GetList<Project>()
                .Select(x => new ProjectDTO()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        EndProject = x.EndProject,
                        Priority = x.Priority,
                        Customer = new CompanyDTO() { ID = x.Customer.ID, Name = x.Customer.Name }
                    }
            ).OrderBy(x => x.Name));
        }

        public string DisplayName
        {
            get { return "Проекты"; }
        }

        public void ModifyEntity(Project project)
        {
            ProjectDTO projectDTO = AllProjects.Where<ProjectDTO>(x => x.ID == project.ID).FirstOrDefault();
            if (projectDTO == null)
            {
                projectDTO = new ProjectDTO()
                {
                    ID = project.ID,
                    Name = project.Name,
                    Contractor = project.Contractor == null ? null : 
                        new CompanyDTO { ID = project.Contractor.ID, Name = project.Contractor.Name },
                    Customer = new CompanyDTO { ID = project.Customer.ID, Name = project.Customer.Name },
                    Priority = project.Priority,
                    EndProject = project.EndProject
                };
                AllProjects.Add(projectDTO);
            }
            else
            {
                AllProjects.Remove(projectDTO);
                projectDTO = new ProjectDTO()
                {
                    ID = project.ID,
                    Name = project.Name,
                    Contractor = project.Contractor == null ? null : 
                        new CompanyDTO { ID = project.Contractor.ID, Name = project.Contractor.Name },
                    Customer = new CompanyDTO { ID = project.Customer.ID, Name = project.Customer.Name },
                    EndProject = project.EndProject
                };
                AllProjects.Add(projectDTO);
            }
        }
    }
}
