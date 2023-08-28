using Domain;
using FluentValidation;
using Shared.Customers;
using Shared.VirtualMachines;

namespace Shared.Tickets
{
    public abstract class TicketDto
    {
        public class Index
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastUpdatedAt { get; set; }
            public string? Severity { get; set; }
            public string? State { get; set; }
            public string? Description { get; set; }
            public int CustomerId { get; set; }

        }

        public class Detail
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastUpdatedAt { get; set; }
            public string? Severity { get; set; }
            public string? State { get; set; }
            public string? Description { get; set; }
            public int CustomerId { get; set; }


        }

        public class Create
        {
            public int CustomerId { get; set; }
            public string Title { get; set; }
            public string Description { get;  set; }
            public string? Severity { get; set; }
            public DateTime CreatedAt { get; set; }

            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
                }
            }
        }

        public class Update
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string? Severity { get; set; }
            public string? State { get; set; }
            public DateTime LastUpdatedAt { get; set; }


            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
                }
            }
        }
    }
}
