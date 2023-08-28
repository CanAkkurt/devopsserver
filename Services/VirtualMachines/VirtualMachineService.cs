using Domain;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Data.Extensions;
using Shared.Customers;
using Shared.Tickets;
using Shared.VirtualMachines;
using System.Collections.Generic;

namespace Services.VirtualMachines;

public class VirtualMachineService : IVirtualMachineService
{
    private readonly DataContext _dbContext;
    private readonly DbSet<VirtualMachine> _virtualMachines;
    private readonly DbSet<Member> _members;
    private readonly DbSet<Customer> _customers;
    private readonly DbSet<Project> _projects;
    private readonly DbSet<Ticket> _tickets;

    public VirtualMachineService(DataContext dbContext)
    {
        this._dbContext = dbContext;
        this._virtualMachines = dbContext.VirtualMachines;
        this._members = dbContext.Members;
        this._customers = dbContext.Customers;
        this._projects = dbContext.Projects;
        this._tickets = dbContext.Tickets;
    }

    private IQueryable<VirtualMachine> GetVirtualMachineById(int id) => _virtualMachines
       .AsNoTracking()
       .Where(a => a.Id == id).Include(a => a.Tickets);

    private IQueryable<Customer> GetCustomerById(int id) => _customers
       .AsNoTracking()
       .Where(a => a.Id == id).Include(a => a.Projects).Include(a => a.VirtualMachines);

    private IQueryable<Member> GetMemberById(int id) => _members
       .AsNoTracking()
       .Where(a => a.Id == id).Include(a => a.VirtualMachines);

    private IQueryable<Project> GetProjectById(int id) => _projects
        .AsNoTracking()
        .Where(a => a.Id == id).Include(a => a.Machines);

    public async Task<VirtualMachineResult.Index> GetIndexAsync(VirtualMachineRequest.Index request)
    {
        var query = _virtualMachines.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Searchterm))
        {
            query = query.Where(x => x.Name.Contains(request.Searchterm, StringComparison.OrdinalIgnoreCase));
        }

        int totalAmount = await query.CountAsync();

        var items = await query
           .Skip((request.Page - 1) * request.PageSize)
           .Take(request.PageSize)
        .OrderBy(x => x.Id)
           .Select(x => new VirtualMachineDto.Detail
           {
               Id = x.Id,
               Name = x.Name,
               State = x.State.ToString(),
               VCPUamount = x.VCPUamount,
               MemoryAmount = x.MemoryAmount,
               StorageAmount = x.StorageAmount,
			   VCPUInUse = x.VCPUamountInUse,
			   MemoryInUse = x.MemoryAmountInUse,
			   StorageInUse = x.StorageAmountInUse,
			   Hostname = x.Hostname,



		   }).ToListAsync();

        var result = new VirtualMachineResult.Index
        {
            VirtualMachines = items,
            TotalAmount = totalAmount
        };
        return result;
    }

    public async Task<VirtualMachineResult.Detail> GetDetailAsync(VirtualMachineRequest.Detail request)
    {
        VirtualMachineResult.Detail result = new();

        result.VirtualMachine = await GetVirtualMachineById(request.Id)
            .Select(x => new VirtualMachineDto.Detail
            {
                Id = x.Id,
                Name = x.Name,
                State = x.State.ToString(),
                Modus = x.Modus.ToString(),
                Hostname = x.Hostname,
                FQDN = x.FQDN,
                VCPUamount = x.VCPUamount,
                MemoryAmount = x.MemoryAmount,
                StorageAmount = x.StorageAmount,
                RequestDate = x.RequestDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                BackupFrequency = x.BackupFrequency,
                Availability = x.Availability,
                HighAvailability = x.HighAvailability,
                FysiekeServer = x.FysiekeServer,
                RelationCustomerDescription = x.RelationCustomerDescription,
                Properties = x.Properties
            }).SingleOrDefaultAsync();

        return result;
    }

    public async Task<TicketResult.Index> GetTicketsFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
    {
        TicketResult.Index result = new();
        var tickets = await GetVirtualMachineById(request.Id).Select(vm => vm.Tickets).SingleOrDefaultAsync();
        result.Tickets = tickets.Select(t => new TicketDto.Index()
        {
            Id = t.Id,
            Title = t.Title,
            CreatedAt = t.CreatedAt,
            LastUpdatedAt = t.LastUpdatedAt,
            Severity = t.Severity.ToString(),
            State = t.State.ToString(),
            Description = t.Description,
            //VirtualMachineId = t.VirtualMachineId,
        });
        result.TotalAmount = result.Tickets.Count();
        return result;
    }

    public async Task<CustomerResult.Detail> GetCustomerFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
    {
        CustomerResult.Detail result = new();
        var customer = await GetVirtualMachineById(request.Id).Select(vm => vm.Customer).SingleOrDefaultAsync();
        result.Customer = new CustomerDto.Detail()
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Department = customer.Department.ToString(),
            ExternType = customer.ExternType,
            Education = customer.Education,
            BackupContactId = customer.BackupContact != null ? customer.BackupContact.Id : 0
        };
        return result;
    }

    public async Task<VirtualMachineResult.Create> CreateAsync(VirtualMachineRequest.Create request)
    {
        VirtualMachineResult.Create result = new();
        var model = request.VirtualMachine;

        if (await _virtualMachines.AnyAsync(x => x.Name == model.Name))
            throw new EntityAlreadyExistsException(nameof(VirtualMachines), nameof(VirtualMachine.Name), model.Name);

        var customer = await GetCustomerById(model.Customer.Id).SingleOrDefaultAsync();
        var project = await GetProjectById(model.Project.Id).SingleOrDefaultAsync();
        var creator = await GetMemberById(model.Creator.Id).SingleOrDefaultAsync();

        Modus modus;
        Enum.TryParse(model.Modus, out modus);

        VirtualMachine vm = new(
            model.Name!,
            VirtualMachineState.Requested,
            project,
            creator,
            customer,
            modus,
            model.Hostname!,
            model.FQDN!,
            model.VCPUamount,
            model.MemoryAmount,
            model.StorageAmount,
            model.RequestDate,
            model.StartDate,
            model.EndDate,
            model.BackupFrequency!,
            model.Availability!,
            model.HighAvailability,
            model.FysiekeServer!,
            model.RelationCustomerDescription,
            model.Properties
        );

        _virtualMachines.Add(vm);
        await _dbContext.SaveChangesAsync();
        result.Id = vm.Id;

        return result;
    }
    public async Task<VirtualMachineResult.Edit> EditAsync(VirtualMachineRequest.Edit request)
    {
        VirtualMachineResult.Edit result = new();
        var model = request.VirtualMachine;

        VirtualMachine? vm = await GetVirtualMachineById(request.Id).SingleOrDefaultAsync();

        if (vm is null)
            throw new EntityNotFoundException(nameof(VirtualMachines), request.Id);

        var customer = await _dbContext.Customers.SingleAsync(c => c.Id == model.Customer!.Id);
        var project = await _dbContext.Projects.SingleAsync(p => p.Id == model.Project!.Id);
        var creator = await _dbContext.Members.SingleAsync(m => m.Id == model.Creator!.Id);

        Modus modus;
        VirtualMachineState state;
        Enum.TryParse(model.Modus, out modus);
        Enum.TryParse(model.State, out state);

        vm.Name = model.Name!;
        vm.State = state;
        vm.Project= project;
        vm.Creator = creator;
        vm.Customer = customer;
        vm.Modus = modus;
        vm.Hostname = model.Hostname!;
        vm.FQDN = model.FQDN!;
        vm.VCPUamount = model.VCPUamount;
        vm.MemoryAmount = model.MemoryAmount;
        vm.StorageAmount = model.StorageAmount;
        vm.RequestDate = model.RequestDate;
        vm.StartDate = model.StartDate;
        vm.EndDate = model.EndDate;
        vm.BackupFrequency = model.BackupFrequency!;
        vm.Availability = model.Availability!;
        vm.HighAvailability = model.HighAvailability;
        vm.FysiekeServer = model.FysiekeServer!;
        vm.RelationCustomerDescription = model.RelationCustomerDescription;
        vm.Properties = model.Properties;

        await _dbContext.SaveChangesAsync();
        result.Id = vm.Id;

        return result;
    }

    public async Task DeleteAsync(VirtualMachineRequest.Delete request)
    {
        var vm = GetVirtualMachineById(request.Id).First();
        foreach(var ticket in vm.Tickets)
        {
            _tickets.RemoveIf(t => t.Id == ticket.Id);
        }
        _virtualMachines.RemoveIf(a => a.Id == request.Id);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<VirtualMachineResult.Usage> GetUsageAsync()
    {
        List<VirtualMachineDto.Usage> vms = new();

        foreach (VirtualMachine vm in await _virtualMachines.ToListAsync())
        {
            VirtualMachineDto.Usage v = new()
            {
                Id = vm.Id,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                VCPUamount = vm.VCPUamount,
                MemoryAmount = vm.MemoryAmount,
                StorageAmount = vm.StorageAmount,

            };
            vms.Add(v);
        }

        var result = new VirtualMachineResult.Usage
        {
            VirtualMachines = vms,
        };

        return result;
    }


}
