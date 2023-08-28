using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class DataInitializer
    {
        private readonly DataContext _dbContext;

        public DataInitializer(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public void InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                // Initialize Members
                var members = InitializeMembers();
                _dbContext.Members.AddRange(members);
                _dbContext.SaveChanges();

                // Initialize Customers
                var customers = InitializeCustomers();
                _dbContext.Customers.AddRange(customers);
                _dbContext.SaveChanges();

                // Initialize Projects
                var projects = InitializeProjects(customers);
                _dbContext.Projects.AddRange(projects);
                _dbContext.SaveChanges();

                // Initialize Virtual Machines
                var vms = InitializeVMs(projects, customers, members);
                _dbContext.VirtualMachines.AddRange(vms);
                _dbContext.SaveChanges();

                // Initialize Tickets
                var tickets = InitializeTickets(customers, vms);
                _dbContext.Tickets.AddRange(tickets);
                _dbContext.SaveChanges();
                var vmOne = _dbContext.VirtualMachines.Where(vm => vm.Name.Equals("VM One")).First();
                _dbContext.Update(vmOne);
                vmOne.AddTicket(tickets.First());
                vmOne.AddTicket(tickets.Skip(1).First());
                vmOne.AddTicket(tickets.Skip(2).First());
                _dbContext.SaveChanges();

                Console.WriteLine("data aangemaakt");      
            }
        }


        private static List<Member> InitializeMembers()
        {
            return new List<Member>
                {
                    new Member("Janne Van Schepdael", true, "janne.vanschepdael@student.hogent.be", "0475741904", MemberRole.Admin, Department.DIT),
                    new Member("Christiano Ronaldo", "Christiano.Ronaldo@student.hogent.be", "0486414052", MemberRole.Admin, Department.DOG),
                    new Member("Lionel Messi", "Lionel.Messi@student.hogent.be", "0486414053", MemberRole.Admin, Department.DOG),
                    new Member("Kevin De Bruyne", "kevin.de.bruyne@student.hogent.be", "0486414054", MemberRole.Admin, Department.DGZ),
                    new Member("Romelu Lukaku", "romelu-lukaku@student.hogent.be", "0475741947", MemberRole.Manager, Department.DBO),
                    new Member("David Goliath", "davidgoliath@student.hogent.be", "0475741978", MemberRole.Manager, Department.DIT),
                    new Member("John Doe", "johndoe9999@student.hogent.be", "0475741904", MemberRole.Manager, Department.DIT),
                    new Member("auth0|63a862f46b625aacfaf54747","Admin", "Admin@hogent.be", "0475741904", MemberRole.Admin, Department.DIT),
                    new Member("auth0|63a86350e2c4faec70d49060","Viewer", "viewer@hogent.be", "0475741905", MemberRole.Viewer, Department.DIT),
                    new Member(
"auth0|63a8671f44641e0a186cbb71","Manager", "Manager@hogent.be", "0475741976", MemberRole.Manager, Department.DIT),
                };
        }


        private static List<Customer> InitializeCustomers()
        {
            return new List<Customer>
            {
                new Customer("Customer One", "customer.one@gmail.com", "0474814759", Department.DIT, "", ""),
                new Customer("Customer Two", "customer.two@gmail.com", "0474814759", Department.DOG, "", ""),
                new Customer("Customer Three", "customer.three@gmail.com", "0474814759", Department.DBO, "", ""),
                new Customer("Customer Four", "customer.four@gmail.com", "0474814759", Department.DGZ, "", ""),
                new Customer("Customer Five", "customer.five@gmail.com", "0474814759", Department.DIT, "", ""),
                new Customer("Customer Six", "customer.six@gmail.com", "0474814759", Department.DIT, "", ""),
            };
        }

        private static List<Project> InitializeProjects(List<Customer> customers)
        {
            return new List<Project>
            {
                new Project("Project One", customers[0]),
                new Project("Project Two", customers[1]),
                new Project("Project Three", customers[1]),
                new Project("Project Four", customers[3]),
                new Project("Project Five", customers[4]),
                new Project("Project Six", customers[5]),
                new Project("Project Seven", customers[0]),
                new Project("Project Eight", customers[0]),
            };
        }

        private static List<VirtualMachine> InitializeVMs(List<Project> projects, List<Customer> customers, List<Member> members)
        {
            return new List<VirtualMachine>
            {   
                new VirtualMachine("VM One", projects[0], members[0],  Modus.IAAS ,customers[0], VirtualMachineState.Requested,  "vic", 3, 2, 128,2,1,40, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Two", projects[1], members[1], Modus.IAAS ,customers[0], VirtualMachineState.Accepted,  "project",   3, 4, 128,2,2,50, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Three", projects[2], members[1], Modus.IAAS ,customers[0], VirtualMachineState.Accepted, "google",  2, 4, 256,1,3,120, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Four", projects[3], members[1], Modus.IAAS ,customers[0], VirtualMachineState.Processing, "facebook", 2, 4, 256,1,3,130, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Five", projects[4], members[2], Modus.IAAS ,customers[1], VirtualMachineState.Denied, "",     2, 8, 512,1,5,320, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Six", projects[1], members[3], Modus.IAAS ,customers[1], VirtualMachineState.Processing, "",  4, 8, 512,3,7,300, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Seven", projects[1], members[4], Modus.IAAS ,customers[1], VirtualMachineState.Requested, "", 4, 16, 1024,3,8,500, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Eight", projects[0], members[0], Modus.IAAS ,customers[2], VirtualMachineState.Requested, "", 4, 16, 1024,4,16,1000, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
                new VirtualMachine("VM Nine", projects[0], members[0], Modus.IAAS ,customers[1], VirtualMachineState.Processing, "", 4, 32, 1024,3,18,500, DateTime.Now, DateTime.Now.AddDays(7), "", true, "", ""),
            };
        }

        private static List<Ticket> InitializeTickets(List<Customer> customers, List<VirtualMachine> vms)
        {
            return new List<Ticket>
            {
                new Ticket("Ticket One", "This is not good, please help", customers[0], "MAJOR", 1),
                new Ticket("Ticket Two", "Hello, my virtual machine is broken, please help", customers[0], "CRITICAL", 1),
                new Ticket("Ticket Three", "I have no clue", customers[1], "", 3),
                new Ticket("Ticket Four", "BIG error", customers[2], "", 4),
                new Ticket("Ticket Five", "VM not work", customers[3], "", 5),
                new Ticket("Ticket Six", "Hello, can you help me?", customers[4], "", 6),
                new Ticket("Ticket Seven", "Problem", customers[0], "", 1),
            };
        }
    }
}
