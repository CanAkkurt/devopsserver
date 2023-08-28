using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Shared.Members;
public class MemberDto
{
    public class Index
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }

        public override string ToString()
        {
            return String.Concat("Naam: ", Name, " | Departement: ", Department);
        }
    }

    public class Detail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
    }

    public class Create
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }

        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Department).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
                RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
                RuleFor(x => x.Role).NotEmpty();
                RuleFor(x => x.Department).NotEmpty();
            }
        }
    }

    public class Update
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }

        public class Validator : AbstractValidator<Update>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Department).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
                RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
                RuleFor(x => x.Role).NotEmpty();
                RuleFor(x => x.Department).NotEmpty();
            }
        }
    }
}
