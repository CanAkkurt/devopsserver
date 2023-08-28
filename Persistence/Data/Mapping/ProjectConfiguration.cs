using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Mapping
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Configuration for Project mapping
            builder.Property(c => c.Name).IsRequired();


            builder.HasMany(x => x.Machines)
                .WithOne(x => x.Project)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
