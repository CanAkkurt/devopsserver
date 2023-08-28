using Bogus;
using Domain;
using Shared.Customers;
using Shared.Tickets;
using Shared.VirtualMachines;

namespace Services.VirtualMachines;

public class BogusVMService : IVirtualMachineService
{
    private readonly List<VirtualMachineDto.Detail> _vms = new();

    public BogusVMService()
    {
        var hostnames = new[] { "ThomasServer", "BobServer", "GidoSerer" };
        var availability = new[] { "Business hours", "Always", "On request" };
        var fqdn = new[] { "http://server2-aws.com", "http://server3-aws.com", "http://server1-aws.com", "http://server4-aws.com" };
        var names = new[] { "HogentVM1", "HogentVM2", "HogentVM3", "HogentVM4", "HogentVM5" };
        var templates = new[] { "AI", "MYSQL", "MSSQLServer", "Data engineer", "Cybersecurity" };
        var memory = new[] { 4, 8, 16, 32, 64 };
        var storage = new[] { 128, 256, 512, 1000 };
        var fysiekeServers = new[] { "Server B019", "Server T120", "Server C214" };
        var ports = new List<Port> { new Port("SSH", 20), new Port("HTTP", 80) };
        var states = new string[] { VirtualMachineState.Accepted.ToString(), VirtualMachineState.Processing.ToString(), VirtualMachineState.Requested.ToString(), VirtualMachineState.Denied.ToString() };
        var customers = new[] { new Customer("Jonas Verschueren", "jonas.ver@hotmail.com", "+32 494 58 04 73"), new Customer("Sean Van den Branden", "Sean.branden@hotmail.com", "+32 564 18 04 73") };
        
        var modus = new[] { Modus.PAAS.ToString(), Modus.SAAS.ToString(), Modus.IAAS.ToString() };
        var projects = new[] {
            new Project("Hogeschool Gent", customers[1]),
            new Project("Project Arteveldehogeschool", customers[0]),
            new Project("IT Company", customers[1])
        };
        var members = new[]
        {
            new Member("Yorben Van Driessche", "yorben.driessche@outlook.be", "04 75 36 48 19", MemberRole.Admin, Department.DBT),
            new Member("Birger Van Vynckt", "birger.vanvynckt@outlook.com", "04 45 36 93 36", MemberRole.Manager, Department.DGZ),
            new Member("Sam Naessens", "sam.naessens@gmail.com", "04 54 36 16 71", MemberRole.Viewer, Department.DOG)
        };

        var vmFaker = new Faker<VirtualMachineDto.Detail>("nl_BE")
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Name, f => f.PickRandom(names))
                .RuleFor(x => x.State, f => f.PickRandom(states))
                .RuleFor(x => x.Modus, f => f.PickRandom(modus))
                .RuleFor(x => x.Hostname, f => f.PickRandom(hostnames))
                .RuleFor(x => x.FQDN, f => f.PickRandom(fqdn))
                .RuleFor(x => x.VCPUamount, f => f.Random.Int(1, 16))
                .RuleFor(x => x.MemoryAmount, f => f.PickRandom(memory))
                .RuleFor(x => x.StorageAmount, f => f.PickRandom(storage))
                .RuleFor(x => x.RequestDate, f => f.Date.Past())
                .RuleFor(x => x.StartDate, f => f.Date.Recent())
                .RuleFor(x => x.EndDate, f => f.Date.Future())
                .RuleFor(x => x.BackupFrequency, f => f.Random.Int(min: 0, max: 30).ToString())
                .RuleFor(x => x.Availability, f => f.PickRandom(availability))
                .RuleFor(x => x.HighAvailability, f => f.PickRandom(new[] { true, false }))
                .RuleFor(x => x.FysiekeServer, f => f.PickRandom(fysiekeServers))
                .RuleFor(x => x.RelationCustomerDescription, "Gewone klant")
                .RuleFor(x => x.Properties, f =>
                {
                    var props = new Dictionary<string, string>();
                    props["cluster"] = f.Random.Int(0, 5).ToString();
                    props["template"] = f.PickRandom(templates);
                    return props;
                });

        _vms = vmFaker.Generate(30);
    }

    public async Task<VirtualMachineDto.Detail> GetDetailAsync(int vmId)
    {
        return _vms.First(vm => vm.Id == vmId);
    }

    public async Task<VirtualMachineResult.Index> GetIndexAsync(VirtualMachineRequest.Index request)
    {
        List<VirtualMachineDto.Detail> vms = new();
        foreach (VirtualMachineDto.Detail vm in _vms)
        {
            VirtualMachineDto.Detail v = new()
            {
                Id = vm.Id,
                Name = vm.Name,
                State = vm.State,
                MemoryAmount = vm.MemoryAmount,
                StorageAmount = vm.StorageAmount,
                VCPUamount = vm.VCPUamount,
                StartDate= vm.StartDate,
                EndDate= vm.EndDate,
            };
            vms.Add(v);
            vms = vms.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
        }

        var result = new VirtualMachineResult.Index
        {
            VirtualMachines = vms,
            TotalAmount = _vms.Count()
        };

        return result;
    }

    public async Task<int> CreateAsync(VirtualMachineDto.Create model)
    {
        Modus modus;
        Enum.TryParse(model.Modus, out modus);
        //VirtualMachine vm = new(model.Name!, VirtualMachineState.Requested, modus, model.Hostname!, model.FQDN, model.VCPUamount, model.MemoryAmount, model.StorageAmount, DateTime.Now, model.StartDate, model.EndDate, model.BackupFrequency!, model.Availability, model.HighAvailability, model.FysiekeServer!, model.RelationCustomerDescription, model.Properties);
        _vms.Add(new()
        {
            Modus = model.Modus,
            Hostname = model.Hostname,
            FQDN = model.FQDN,
            RequestDate = model.RequestDate,
            BackupFrequency = model.BackupFrequency,
            Availability = model.Availability,
            HighAvailability = model.HighAvailability,
            FysiekeServer = model.FysiekeServer,
            RelationCustomerDescription = model.RelationCustomerDescription,
            Id = _vms.Count,
            Name = model.Name,
            State = model.State,
            VCPUamount = model.VCPUamount,
            MemoryAmount = model.MemoryAmount,
            StorageAmount = model.StorageAmount,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Project = model.Project,
            Creator = model.Creator,
            Customer = model.Customer
        });
        return _vms.Count - 1;
    }

    public async Task EditAsync(int vmId, VirtualMachineDto.Update model)
    {
        Modus modus;
        Enum.TryParse(model.Modus, out modus);

        var vmToEdit = _vms.Where(vm => vm.Id == vmId);
        if(vmToEdit.Count() > 0)
        {
            var toEdit = vmToEdit.First();

            toEdit.Modus = model.Modus;
            toEdit.Hostname = model.Hostname;
            toEdit.FQDN = model.FQDN;
            toEdit.RequestDate = model.RequestDate;
            toEdit.BackupFrequency = model.BackupFrequency;
            toEdit.Availability = model.Availability;
            toEdit.HighAvailability = model.HighAvailability;
            toEdit.FysiekeServer = model.FysiekeServer;
            toEdit.RelationCustomerDescription = model.RelationCustomerDescription;
            toEdit.Name = model.Name;
            toEdit.State = model.State;
            toEdit.VCPUamount = model.VCPUamount;
            toEdit.MemoryAmount = model.MemoryAmount;
            toEdit.StorageAmount = model.StorageAmount;
            toEdit.StartDate = model.StartDate;
            toEdit.EndDate = model.EndDate;
            toEdit.Project = model.Project;
            toEdit.Creator = model.Creator;
            toEdit.Customer = model.Customer;

            _vms.Remove(toEdit);
            _vms.Add(toEdit);
        }
    }

    public async Task DeleteAsync(int vmId)
    {
        var vm = _vms.SingleOrDefault(x => x.Id == vmId);
        if (vm != null)
        {
            _vms.Remove(vm);
        }
    }

    public async Task<VirtualMachineResult.Usage> GetUsageAsync()
    {
        List<VirtualMachineDto.Usage> vms = new();
        foreach (VirtualMachineDto.Detail vm in _vms)
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

    public Task<VirtualMachineResult.Detail> GetDetailAsync(VirtualMachineRequest.Detail request)
    {
        throw new NotImplementedException();
    }

    public Task<VirtualMachineResult.Create> CreateAsync(VirtualMachineRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public Task<VirtualMachineResult.Edit> EditAsync(VirtualMachineRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(VirtualMachineRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public Task<TicketResult.Index> GetTicketsFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerResult.Detail> GetCustomerFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
    {
        throw new NotImplementedException();
    }
}
