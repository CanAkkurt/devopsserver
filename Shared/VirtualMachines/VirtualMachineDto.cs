using Domain;
using FluentValidation;
using Shared.Customers;
using Shared.Members;
using Shared.Projects;
using Shared.VirtualMachines.Ports;

namespace Shared.VirtualMachines;

public abstract class VirtualMachineDto
{
    public class Index
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public int VCPUamount { get; set; }
        public double MemoryAmount { get; set; }
        public double StorageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string ToString()
        {
            return String.Concat("Naam: ", Name);
        }
    }

    public class Detail
    {
        public string? Modus { get; set; }
        public string? Hostname { get; set; }
        public string? FQDN { get; set; }
        public DateTime RequestDate { get; set; }
        public string? BackupFrequency { get; set; }
        public string? Availability { get; set; }
        public bool HighAvailability { get; set; }
        public string? FysiekeServer { get; set; }
        public string? RelationCustomerDescription { get; set; }
        public Dictionary<string, string>? Properties { get; set; }

        public string Active { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public int VCPUamount { get; set; }
        public double MemoryAmount { get; set; }

        public double MemoryInUse { get; set; }

		public int VCPUInUse { get; set; }

		public double StorageInUse { get; set; }




		public double StorageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectDto.Index? Project { get; set; }
        public MemberDto.Index? Creator { get; set; }
        public CustomerDto.Index? Customer { get; set; }

        public override string ToString()
        {
            return String.Concat("Naam: ", Name);
        }
    }
    public class Create
    {   
        public string? Modus { get; set; }
        public string? Hostname { get; set; }
        public string? FQDN { get; set; }
        public DateTime RequestDate { get; set; }
        public string? BackupFrequency { get; set; }
        public string? Availability { get; set; }
        public bool HighAvailability { get; set; }
        public string? FysiekeServer { get; set; }
        public string? RelationCustomerDescription { get; set; }
        public Dictionary<string, string>? Properties { get; set; }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public int VCPUamount { get; set; }
        public double MemoryAmount { get; set; }
        public double StorageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectDto.Index? Project { get; set; }
        public MemberDto.Index? Creator { get; set; }
        public CustomerDto.Index? Customer { get; set; }

        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Project).NotEmpty();
                RuleFor(x => x.Customer).NotEmpty();
                RuleFor(x => x.Creator).NotEmpty();
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.State).NotEmpty();
                RuleFor(x => x.Hostname).NotEmpty().MaximumLength(50);
                RuleFor(x => x.FQDN).NotEmpty().MaximumLength(50);
                RuleFor(x => x.VCPUamount).NotEmpty();
                RuleFor(x => x.MemoryAmount).NotEmpty();
                RuleFor(x => x.StorageAmount).NotEmpty();
                RuleFor(x => x.RequestDate).NotEmpty();
                RuleFor(x => x.StartDate).NotEmpty();
                RuleFor(x => x.EndDate).NotEmpty();
                RuleFor(x => x.BackupFrequency).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Availability).NotEmpty().MaximumLength(100);
                RuleFor(x => x.FysiekeServer).NotEmpty().MaximumLength(100);
                RuleFor(x => x.RelationCustomerDescription).NotEmpty().MaximumLength(200);
            }
        }
    }

    public class Usage
    {
        public int Id { get; set; }
        public int VCPUamount { get; set; }
        public double MemoryAmount { get; set; }
        public double StorageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class Update
    {
        public string? Modus { get; set; }
        public string? Hostname { get; set; }
        public string? FQDN { get; set; }
        public DateTime RequestDate { get; set; }
        public string? BackupFrequency { get; set; }
        public string? Availability { get; set; }
        public bool HighAvailability { get; set; }
        public string? FysiekeServer { get; set; }
        public string? RelationCustomerDescription { get; set; }
        public Dictionary<string, string>? Properties { get; set; }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public int VCPUamount { get; set; }
        public double MemoryAmount { get; set; }
        public double StorageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectDto.Index? Project { get; set; }
        public MemberDto.Index? Creator { get; set; }
        public CustomerDto.Index? Customer { get; set; }

        public class Validator : AbstractValidator<Update>
        {
            public Validator()
            {
                // TO DO: Fix port validator
                //RuleForEach(x => x.Ports).NotEmpty().SetValidator(new PortDto.Validator()!);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.State).NotEmpty();
                RuleFor(x => x.Hostname).NotEmpty().MaximumLength(50);
                RuleFor(x => x.FQDN).NotEmpty().MaximumLength(50);
                RuleFor(x => x.VCPUamount).NotEmpty();
                RuleFor(x => x.MemoryAmount).NotEmpty();
                RuleFor(x => x.StorageAmount).NotEmpty();
                RuleFor(x => x.RequestDate).NotEmpty();
                RuleFor(x => x.StartDate).NotEmpty();
                RuleFor(x => x.EndDate).NotEmpty();
                RuleFor(x => x.BackupFrequency).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Availability).NotEmpty().MaximumLength(100);
                RuleFor(x => x.FysiekeServer).NotEmpty().MaximumLength(100);
                RuleFor(x => x.RelationCustomerDescription).NotEmpty().MaximumLength(200);
            }
        }
    }
}
