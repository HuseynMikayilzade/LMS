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
    public class GroupCofiguration : IEntityTypeConfiguration<Group>
    {
        void IEntityTypeConfiguration<Group>.Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(g => g.Name).IsRequired().HasMaxLength(20);
            builder.HasIndex(g => g.Name).IsUnique();
            builder.Property(g => g.MaxStudent).IsRequired().HasMaxLength(30);           
            builder.Property(g => g.CurrentStudent).IsRequired();
        }
    }
}
