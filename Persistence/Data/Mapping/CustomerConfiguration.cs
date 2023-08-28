using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Mapping;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Configuration for Customer mapping
        builder.Property(c => c.Name).IsRequired();

        builder.HasMany(c => c.Projects)
                .WithOne(c => c.Customer)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
