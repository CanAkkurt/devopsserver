using FluentValidation;

namespace Shared.Customers;

public abstract class CustomerDto
{
    public class Index 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Detail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }
        public string? ExternType { get; set; }
        public string? Education { get; set; }
        public int BackupContactId { get; set; }

    }

    public class Create
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }
        public string? ExternType { get; set; }
        public string? Education { get; set; }
        public int BackupContactId { get; set; }

        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
                RuleFor(x => x.ExternType).MaximumLength(100);
                RuleFor(x => x.Education).MaximumLength(100);
            }
        }
    }

    public class Update
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }
        public string? ExternType { get; set; }
        public string? Education { get; set; }
        public int BackupContactId { get; set; }

        public class Validator : AbstractValidator<Update>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
                RuleFor(x => x.ExternType).MaximumLength(100);
                RuleFor(x => x.Education).MaximumLength(100);
            }
        }
    }
}