using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Domain;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public Customer Creator { get; set; }
    public int CustomerId { get; set; }
    public TicketSeverity Severity { get; set; }
    public TicketState State { get; set; }

    public Ticket()
    {

    }

    public Ticket(string title, string description, string? ticketSeverity, DateTime createdAt, int customerId)
    {
        TicketSeverity severity;
        Enum.TryParse(ticketSeverity, out severity);

        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        LastUpdatedAt = DateTime.Now;
        Severity = ticketSeverity != null ? severity : TicketSeverity.UNDEFINED;
        State = TicketState.Started;
        CreatedAt = createdAt;
        CustomerId = customerId;
    }

    public Ticket(string title, string description, Customer creator, string? ticketSeverity, int customerId)
    {
        TicketSeverity severity;
        Enum.TryParse(ticketSeverity, out severity);

        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        LastUpdatedAt = DateTime.Now;
        Creator = creator;
        Severity = ticketSeverity != null ? severity : TicketSeverity.UNDEFINED;
        State = TicketState.Started;
        CustomerId = customerId;
    }

    public Ticket(int id, string title, string description, DateTime createdAt, DateTime lastUpdatedAt, Customer creator, TicketSeverity severity, TicketState state)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        LastUpdatedAt = lastUpdatedAt;
        Creator = creator;
        Severity = severity;
        State = state;
    }
}

public enum TicketState
{
    Started,
    InProgress,
    Done
}

public enum TicketSeverity
{
    UNDEFINED,
    MINOR,
    MAJOR,
    CRITICAL
}
