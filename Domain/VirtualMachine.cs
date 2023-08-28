
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class VirtualMachine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Project? Project { get; set; }
    public Member Creator { get; set; }
    public VirtualMachineState State { get; set; }
    public Modus Modus { get; set; }
    public Customer Customer { get; set; }
    public List<Port>? Ports { get; } = new();
    public List<Ticket>? Tickets { get; } = new();
    public string Hostname { get; set; }
    public string? FQDN { get; set; }
    public int VCPUamount { get; set; }
    public double MemoryAmount { get; set; }
    public double StorageAmount { get; set; }


	public int VCPUamountInUse { get; set; }
	public double MemoryAmountInUse { get; set; }
	public double StorageAmountInUse { get; set; }


	public DateTime RequestDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string BackupFrequency { get; set; }
    public string? Availability { get; set; }
    public bool HighAvailability { get; set; }
    public string FysiekeServer { get; set; }
    public string? RelationCustomerDescription { get; set; }

    // TODO: data types, validation, enz
    // Reden gebruik VM
    // opdracht toegewezen aan...
    [NotMapped]
    public Dictionary<string, string>? Properties { get; set; } = new();

    //Used in BOGUS vm service
    public VirtualMachine()
    {
    }

    public VirtualMachine(string name, Project? project, Member creator, Modus modus, Customer customer, VirtualMachineState state, string hostname, int vCPUamount, double memoryAmount, double storageAmount,int vCPUamountInUse, double memoryAmountInUse,double storageAmountInUse,  DateTime startDate, DateTime endDate, string backupFrequency, bool highAvailability, string fysiekeServer, string? relationCustomerDescription)
    {
        Name = name;
        Project = project;
        Creator = creator;
        State = state;
        Modus = modus;
        Customer = customer;
        Hostname = hostname;
        VCPUamount = vCPUamount;
        MemoryAmount = memoryAmount;
        StorageAmount = storageAmount;
		VCPUamountInUse = vCPUamountInUse;
		MemoryAmountInUse = memoryAmountInUse;
		StorageAmountInUse = storageAmountInUse;
		StartDate = startDate;
        EndDate = endDate;
        BackupFrequency = backupFrequency;
        HighAvailability = highAvailability;
        FysiekeServer = fysiekeServer;
        RelationCustomerDescription = relationCustomerDescription;
    }

    public VirtualMachine(string name, VirtualMachineState state, Modus modus, string hostname, string? fQDN, int vCPUamount, 
                            double memoryAmount, double storageAmount, DateTime requestDate,
                            DateTime startDate, DateTime endDate, string backupFrequency, string? availability, bool highAvailability, string fysiekeServer,
                            string? relationCustomerDescription, Dictionary<string, string>? properties)
    {
        Name = name;
        State = state;
        Modus = modus;
        Hostname = hostname;
        FQDN = fQDN ?? "";
        VCPUamount = vCPUamount;
        MemoryAmount = memoryAmount;
        StorageAmount = storageAmount;
        RequestDate = requestDate;
        StartDate = startDate;
        EndDate = endDate;
        BackupFrequency = backupFrequency;
        Availability = availability ?? "";
        HighAvailability = highAvailability;
        FysiekeServer = fysiekeServer;
        RelationCustomerDescription = relationCustomerDescription ?? "";
        Properties = properties ?? new();
    }


    public VirtualMachine(string name, VirtualMachineState state, Project project, Member creator, Customer customer,
        Modus modus, string hostname, string? fQDN, int vCPUamount, double memoryAmount, double storageAmount, DateTime requestDate,
        DateTime startDate, DateTime endDate, string backupFrequency, string? availability, bool highAvailability, string fysiekeServer,
        string? relationCustomerDescription, Dictionary<string, string>? properties)
    {
        Name = name;
        State = state;
        Project = project;
        Creator = creator;
        Customer = customer;
        Modus = modus;
        Hostname = hostname;
        FQDN = fQDN ?? "";
        VCPUamount = vCPUamount;
        MemoryAmount = memoryAmount;
        StorageAmount = storageAmount;
        RequestDate = requestDate;
        StartDate = startDate;
        EndDate = endDate;
        BackupFrequency = backupFrequency;
        Availability = availability ?? "";
        HighAvailability = highAvailability;
        FysiekeServer = fysiekeServer;
        RelationCustomerDescription = relationCustomerDescription ?? "";
        Properties = properties ?? new();
    }

    public void ChangeVMState(VirtualMachineState state)
    {
        State = state;
    }

    public void AddTicket(Ticket ticket)
    {
        Tickets?.Add(ticket);
    }
    public void AddPort(Port port)
    {
        Ports?.Add(port);
    }

}

public enum VirtualMachineState
{
    Requested,
    Processing,
    Denied,
    Accepted
}

public enum Modus
{
    PAAS,
    IAAS,
    SAAS,
    Sjabloon1,
    Sjabloon2
}