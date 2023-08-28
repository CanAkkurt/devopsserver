using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Mapping
{
    public class VirtualMachineConfiguration : IEntityTypeConfiguration<VirtualMachine>
    {
        public void Configure(EntityTypeBuilder<VirtualMachine> builder)
        {
            builder.Property(c => c.Name).IsRequired();


            builder.HasMany(x => x.Tickets).WithOne().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
