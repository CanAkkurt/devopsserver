using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.VirtualMachines.Ports;

public class PortDto
{
    public string? Type { get; set; }
    public int Nummer { get; set; }

    public class Validator: AbstractValidator<PortDto>
    {
        public Validator()
        {
            RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Nummer).NotEmpty();
        }
    }
}
