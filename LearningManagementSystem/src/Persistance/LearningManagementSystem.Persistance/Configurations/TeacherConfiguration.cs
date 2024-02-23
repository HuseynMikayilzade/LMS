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
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        void IEntityTypeConfiguration<Teacher>.Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(t => t.Surname)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.Gender)
                   .IsRequired();

            builder.Property(t => t.Image)
                   .IsRequired(false);

            builder.Property(t => t.Birthday)
                   .IsRequired();

            builder.Property(t => t.EmailAddress)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.PhoneNumber)
                   .IsRequired();
  
            builder.Property(t => t.Description)
                   .IsRequired(false)
                   .HasColumnType("text");

        }
    }
}
