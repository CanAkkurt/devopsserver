using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Mapping;

namespace Persistence.Data
{ 
    public class DataContext : DbContext
    {
        public DbSet<VirtualMachine> VirtualMachines { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Port> Ports { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Add configurations
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new VirtualMachineConfiguration());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new MemberConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}