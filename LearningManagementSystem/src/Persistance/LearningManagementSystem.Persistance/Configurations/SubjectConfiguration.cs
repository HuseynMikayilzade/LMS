using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        void IEntityTypeConfiguration<Subject>.Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.Property(s=>s.Name).IsRequired().HasMaxLength(40); 
            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
