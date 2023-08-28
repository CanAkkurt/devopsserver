namespace Domain;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Customer Customer { get; set; }
    public List<VirtualMachine> Machines { get; set; } = new();
    public ProjectState State { get; set; }

    public Project()
    {

    }

    public Project(string name, Customer customer)
    {
        Name = name;
        Customer = customer;
        State = ProjectState.Requested;
    }

    public Project(string name, Customer customer, List<VirtualMachine> machines)
    {
        Name = name;
        Customer = customer;
        Machines = machines;
        State = ProjectState.Requested;
    }
}

public enum ProjectState
{
    Requested,
    Active,
    Terminated
}
