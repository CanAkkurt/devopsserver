using Domain.Exceptions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Tickets;
using Persistence.Data.Extensions;
using Shared.VirtualMachines;

namespace Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<Project> _projects;
        private readonly DbSet<Customer> _customers;
        private readonly DbSet<Ticket> _tickets;
        private readonly DbSet<VirtualMachine> _virtualMachines;

        public ProjectService(DataContext dbContext)
        {
            this._dbContext = dbContext;
            this._projects = dbContext.Projects;
            this._customers = dbContext.Customers;
            this._tickets = dbContext.Tickets;
            this._virtualMachines = dbContext.VirtualMachines;
        }

        private IQueryable<Project> GetProjectById(int id) => _projects
        .AsNoTracking()
        .Where(a => a.Id == id).Include(a => a.Machines);

        private IQueryable<VirtualMachine> GetVMById(int id) => _virtualMachines
        .AsNoTracking()
        .Where(vm => vm.Id == id).Include(vm => vm.Tickets);

        private IQueryable<Customer> GetCustomerById(int id) => _customers
        .AsNoTracking()
        .Where(a => a.Id == id).Include(a => a.Projects).Include(a => a.VirtualMachines);

        public async Task<ProjectResult.Index> GetIndexAsync(ProjectRequest.Index request)
        {
            var query = _projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Searchterm))
            {
                query = query.Where(x => x.Name.Contains(request.Searchterm, StringComparison.OrdinalIgnoreCase));
            }

            int totalAmount = await query.CountAsync();

            var items = await query
               .Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
                .OrderBy(x => x.Id)
               .Select(x => new ProjectDto.Index
               {
                   Id = x.Id,
                   Name = x.Name,
                   CustomerName = x.Customer.Name,
                   State = x.State.ToString(),
                   VmAmount = x.Machines.Count(),
                   TotalCPUs = x.Machines.Sum(vm => vm.VCPUamount),
                   TotalMemory = x.Machines.Sum(vm => vm.MemoryAmount),
                   TotalStorage = x.Machines.Sum(vm => vm.StorageAmount),
                   //TO DO: Fix dates to have a null value if there are no machines
                   //StartDate = x.Machines.Min(vm => vm.StartDate),
                   //EndDate = x.Machines.Max(vm => vm.EndDate),
            }).ToListAsync();

            var result = new ProjectResult.Index
            {
                Projects = items,
                TotalAmount = totalAmount
            };
            return result;
        }

        public async Task<ProjectResult.Detail> GetDetailAsync(ProjectRequest.Detail request)
        {
            ProjectResult.Detail result = new();

            if (GetProjectById(request.Id).First().Machines.Count == 0)
            {
                var project = await GetProjectById(request.Id)
                .Select(x => new ProjectDto.Detail
                {
                    Id = x.Id,
                    Name = x.Name,
                    CustomerName = x.Customer.Name,
                    CustomerId = x.Customer.Id,
                    State = x.State.ToString(),
                    VmAmount = 0,
                    TotalCPUs = 0,
                    TotalMemory = 0,
                    TotalStorage = 0,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                }).SingleOrDefaultAsync();
                result.Project = project;
                return result;
            }
            else
            {
                var project = await GetProjectById(request.Id)
                .Select(x => new ProjectDto.Detail
                {
                    Id = x.Id,
                    Name = x.Name,
                    CustomerName = x.Customer.Name,
                    CustomerId = x.Customer.Id,
                    State = x.State.ToString(),
                    VmAmount = x.Machines.Count(),
                    TotalCPUs = x.Machines.Sum(vm => vm.VCPUamount),
                    TotalMemory = x.Machines.Sum(vm => vm.MemoryAmount),
                    TotalStorage = x.Machines.Sum(vm => vm.StorageAmount),
                    StartDate = x.Machines.Min(vm => vm.StartDate),
                    EndDate = x.Machines.Max(vm => vm.EndDate),
                }).SingleOrDefaultAsync();
                result.Project = project;
                return result;
            }
        }

        public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromProjectAsync(ProjectRequest.Detail request)
        {
            VirtualMachineResult.Index result = new();
            var virtualMachines = await GetProjectById(request.Id).Select(p => p.Machines).SingleOrDefaultAsync();
            result.VirtualMachines = virtualMachines.Select(vm => new VirtualMachineDto.Detail()
            {
                Id = vm.Id,
                Name = vm.Name,
                State = vm.State.ToString(),
                VCPUamount = vm.VCPUamount,
                MemoryAmount = vm.MemoryAmount,
                StorageAmount = vm.StorageAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
            });
            result.TotalAmount = result.VirtualMachines.Count();
            return result;
        }

        public async Task<ProjectResult.Create> CreateAsync(ProjectRequest.Create request)
        {
            ProjectResult.Create result = new();
            var model = request.Project;

            var customer = await GetCustomerById(request.Project.CustomerId).SingleOrDefaultAsync(); ;

            Project project = new(
                model.Name,
                customer
            );

            _dbContext.Attach(project.Customer);
            foreach(var vm in project.Machines)
            {
                _dbContext.Attach(vm);
            }
            //_dbContext.Attach(project.Machines);

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
            result.Id = project.Id;

            return result;
        }

        public async Task<ProjectResult.Edit> EditAsync(ProjectRequest.Edit request)
        {
            ProjectResult.Edit result = new();
            var model = request.Project;

            Project project = await GetProjectById(request.Id).SingleOrDefaultAsync();

            if (project is null)
                throw new EntityNotFoundException(nameof(Project), request.Id);

            ProjectState state;
            Enum.TryParse(model.State, out state);

            project.Name = model.Name;
            project.State = state;

            _dbContext.Update(project);
            await _dbContext.SaveChangesAsync();
            result.Id = project.Id;

            return result;
        }

        public async Task DeleteAsync(ProjectRequest.Delete request)
        {
            var project = GetProjectById(request.Id).First();
            foreach(var virtualMachine in project.Machines)
            {
                var vm = GetVMById(virtualMachine.Id).First();
                foreach(var ticket in vm.Tickets)
                {
                     _tickets.RemoveIf(t => t.Id == ticket.Id);
                }
                
                _virtualMachines.RemoveIf(v => v.Id == vm.Id);
            }

            _projects.RemoveIf(a => a.Id == request.Id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
