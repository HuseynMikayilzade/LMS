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
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        void IEntityTypeConfiguration<Student>.Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.Name).IsRequired().HasMaxLength(30);
            builder.Property(s => s.Surname).IsRequired().HasMaxLength(50);
            builder.Property(s => s.Gender).IsRequired();
            builder.Property(s => s.Image).IsRequired(false);
            builder.Property(s => s.Birthday).IsRequired();
            builder.Property(s => s.EmailAddress).IsRequired();
            builder.Property(s => s.PhoneNumber).IsRequired();
            builder.Property(s => s.Point).IsRequired();
            builder.Property(s => s.Avarage).IsRequired();           
            builder.Property(s => s.IsFailed).IsRequired();

        }
    }
}
