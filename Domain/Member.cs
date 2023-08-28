
namespace Domain;

/**
 * Staff member of VIC
 */
public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? Active { get; set; }
    public string Email { get; set; }

    public string AuthId { get; set; }

    public string PhoneNumber { get; set; }
    public MemberRole Role { get; set; }
    public Department Department { get; set; }
    public List<VirtualMachine>? VirtualMachines { get; set; } = new();

    public Member()
    {

    }

    public Member(string name, string email, string phoneNumber, MemberRole role, Department department)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        Department = department;
    }
    public Member(string name, bool active, string email, string phoneNumber, MemberRole role, Department department)
    {
        Name = name;
        Active = active;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        Department = department;
    }

    public Member(String id,string name, string email, string phoneNumber, MemberRole role, Department department)
    {
        AuthId = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        Department = department;
    }

    public void CreateVirtualMachine(string name, VirtualMachineState state, Project project, Member moderator, Customer customer,
        Modus modus, string hostname, string fQDN, int vCPUamount, double memoryAmount, double storageAmount, DateTime requestDate,
        DateTime startDate, DateTime endDate, string backupFrequency, string availability, bool highAvailability, string fysiekeServer,
        string? relationCustomerDescription, Dictionary<string, string>? properties)
    {
        if (Role.Equals(MemberRole.Manager))
        {
            VirtualMachines?.Add(new VirtualMachine(
                name, state, project, moderator, customer,  modus,  hostname, fQDN, vCPUamount,
                memoryAmount, storageAmount, requestDate, startDate, endDate, backupFrequency, availability,
                highAvailability, fysiekeServer, relationCustomerDescription ?? "", properties
            ));
        }
    }

}

public enum MemberRole
{
    Admin, // can add and remove users
    Manager, // can manage VMs and customers
    Viewer // can view all data
}
