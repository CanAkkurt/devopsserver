using Domain.Exceptions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Customers;
using Persistence.Data;
using Persistence.Data.Extensions;
using Shared.Projects;
using System.Linq;

namespace Services.Customers;

public class CustomerService : ICustomerService
{
    private readonly DataContext _dbContext;
    private readonly DbSet<Customer> _customers;

    public CustomerService(DataContext dbContext)
    {
        this._dbContext = dbContext;
        this._customers = dbContext.Customers;
    }

    private IQueryable<Customer> GetCustomerById(int id) => _customers
        .AsNoTracking()
        .Where(a => a.Id == id).Include(a => a.Projects).Include(a => a.VirtualMachines);

    public async Task<CustomerResult.Index> GetIndexAsync(CustomerRequest.Index request)
    {
        var query = _customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Searchterm))
        {
            query = query.Where(x => x.Name.Contains(request.Searchterm, StringComparison.OrdinalIgnoreCase));
        }

        int totalAmount = await query.CountAsync();

        var items = await query
           .Skip((request.Page - 1) * request.PageSize)
           .Take(request.PageSize)
           .OrderBy(x => x.Id)
           .Select(x => new CustomerDto.Index
           {
               Id = x.Id,
               Name = x.Name,
               Email = x.Email,
               PhoneNumber = x.PhoneNumber,
           }).ToListAsync();

        var result = new CustomerResult.Index
        {
            Customers = items,
            TotalAmount = totalAmount
        };
        return result;
    }

    public async Task<CustomerResult.Detail> GetDetailAsync(CustomerRequest.Detail request)
    {
        CustomerResult.Detail result = new();

        result.Customer = await GetCustomerById(request.Id)
            .Select(x => new CustomerDto.Detail
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Department = x.Department.ToString(),
                ExternType = x.ExternType,
                Education = x.Education,
            }).SingleOrDefaultAsync();

        return result;
    }

    public async Task<ProjectResult.Index> GetProjectsFromCustomerAsync(CustomerRequest.Detail request)
    {
        ProjectResult.Index result = new();

        var projects = await GetCustomerById(request.Id).Select(c => c.Projects).SingleOrDefaultAsync();
        foreach (var project in projects)
        {
            var customer = GetCustomerById(request.Id).First();
            project.Customer = customer;
            if (project.Machines.Count == 0)
            {
                var addProject = new ProjectDto.Index()
                {
                    Id = project.Id,
                    Name = project.Name,
                    CustomerName = project.Customer.Name,
                    State = project.State.ToString(),
                    VmAmount = 0,
                    TotalCPUs = 0,
                    TotalMemory = 0,
                    TotalStorage = 0,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                };
                if(result.Projects == null) result.Projects = new List<ProjectDto.Index>();
                result.Projects = result.Projects.Append(addProject);
            }
            else
            {
                result.Projects = result.Projects.Append(new ProjectDto.Index()
                {
                    Id = project.Id,
                    Name = project.Name,
                    CustomerName = project.Customer.Name,
                    State = project.State.ToString(),
                    VmAmount = project.Machines.Count(),
                    TotalCPUs = project.Machines.Sum(vm => vm.VCPUamount),
                    TotalMemory = project.Machines.Sum(vm => vm.MemoryAmount),
                    TotalStorage = project.Machines.Sum(vm => vm.StorageAmount),
                    StartDate = project.Machines.Min(vm => vm.StartDate),
                    EndDate = project.Machines.Max(vm => vm.EndDate),
                });
            }
        }
        result.TotalAmount = result.Projects.Count();
        return result;
    }

    public async Task<CustomerResult.Create> CreateAsync(CustomerRequest.Create request)
    {
        CustomerResult.Create result = new();
        var model = request.Customer;

        if (await _customers.AnyAsync(x => x.Email == model.Email))
            throw new EntityAlreadyExistsException(nameof(Customer), nameof(Customer.Email), model.Email);

        Department department;
        Enum.TryParse(model.Department, out department);

        //get backup contact by backupcontact ID from dto
        var BackupContact = await _customers.SingleOrDefaultAsync(x => x.Id == model.BackupContactId);

        Customer customer = new
        (
            model.Name!,
            model.Email!,
            model.PhoneNumber!,
            department,
            model.ExternType,
            model.Education,
            BackupContact
        );

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
        result.Id = customer.Id;
        return result;
    }

    public async Task<CustomerResult.Edit> EditAsync(CustomerRequest.Edit request)
    {
        CustomerResult.Edit result = new();

        Customer customer = await GetCustomerById(request.Id).SingleOrDefaultAsync();

        if (customer is null)
            throw new EntityNotFoundException(nameof(Customer), request.Id);

        var model = request.Customer;

        Department department;
        Enum.TryParse(model.Department, out department);

        customer.Name = model.Name!;
        customer.Email = model.Email!;
        customer.Department = department;
        customer.ExternType = model.ExternType!;
        customer.Education = model.Education;
        customer.PhoneNumber = model.PhoneNumber!;
        customer.BackupContact = await _customers.SingleOrDefaultAsync(x => x.Id == model.BackupContactId);

        await _dbContext.SaveChangesAsync();
        result.Id = customer.Id;

        return result;
    }

    public async Task DeleteAsync(CustomerRequest.Delete request)
    {
        _customers.RemoveIf(a => a.Id == request.Id);
        await _dbContext.SaveChangesAsync();
    }
}
