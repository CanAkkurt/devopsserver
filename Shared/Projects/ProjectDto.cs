using FluentValidation;
using Shared.Customers;
using Shared.VirtualMachines;

namespace Shared.Projects
{
    public abstract class ProjectDto
    {
        public class Index
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? CustomerName { get; set; }
            public string? State { get; set; }
            public int VmAmount { get; set; }
            public int TotalCPUs { get; set; }
            public double TotalMemory { get; set; }
            public double TotalStorage { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public class Detail
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? CustomerName { get; set; }
            public int CustomerId { get; set; }
            public string? State { get; set; }
            public int VmAmount { get; set; }
            public int TotalCPUs { get; set; }
            public double TotalMemory { get; set; }
            public double TotalStorage { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class Create
        {
            public string Name { get; set; }
            public int CustomerId { get; set; }

            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                }
            }
        }

        public class Update
        {
            public string? Name { get; set; }
            public string? State { get; set; }

            public class Validator : AbstractValidator<Update>
            {
                public Validator()
                {
                    RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                }
            }
        }
    }
}
