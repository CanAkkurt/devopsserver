namespace Domain;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Department? Department { get; set; }
    public string? ExternType { get; set; }
    public string? Education { get; set; }
    public Customer? BackupContact { get; set; }
    public List<Project> Projects { get; set; } = new();
    public List<Ticket>? Tickets { get; set; } = new();
    public List<VirtualMachine>? VirtualMachines { get; set; } = new();

    public bool IsIntern => Department != null;
    public bool IsExtern => Department == null;


    public Customer()
    {

    }

    //Used in BOGUS vm service
    public Customer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    public Customer(string name, string email, string phoneNumber, Department? department, string? externType, string? education)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Department = department;
        ExternType = externType;
        Education = education;
    }



    public Customer(string name, string email, string phoneNumber, Department? department, string? externType, string? education, Customer? backupContact)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Department = department;
        ExternType = externType;
        Education = education;
        BackupContact = backupContact;
    }

    public Customer(int id, string name, string email, string phoneNumber, Department? department, string? externType, string? education, Customer? backupContact, List<Project> projects, List<VirtualMachine>? virtualMachines, List<Ticket>? tickets)
    {
        if (department == null && externType == null)
        {
            throw new ArgumentException("Expected one of department or externType, but both were null");
        }

        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Department = department;
        ExternType = externType;
        Education = education;
        BackupContact = backupContact;
        Projects = projects;
        VirtualMachines = virtualMachines;
        Tickets = tickets;
    }

    public static Customer CreateIntern(int id, string name, string email, string phoneNumber, Department department, List<Project> projects)
    {
        return new Customer(id, name, email, phoneNumber, department, null, null, null, projects, null, null);
    }

    public static Customer CreateExtern(int id, string name, string email, string phoneNumber, string externType, List<Project> projects)
    {
        return new Customer(id, name, email, phoneNumber, null, externType, null, null, projects, null, null);
    }

    public void AddVirtualMachine(VirtualMachine vm)
    {
        VirtualMachines?.Add(vm);
    }

    public void AddTicket(Ticket ticket)
    {
        Tickets?.Add(ticket);
    }
    public void AddProject(Project project)
    {
        Projects?.Add(project);
    }

}

public enum Department
{
    DBO,
    DBT,
    DGZ,
    DIT,
    DLO,
    DOG,
    DSA,
    Extern
}