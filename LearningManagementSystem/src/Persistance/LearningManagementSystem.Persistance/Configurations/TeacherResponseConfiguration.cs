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
    public class TeacherResponseConfiguration : IEntityTypeConfiguration<TeacherResponse>
    {
        public void Configure(EntityTypeBuilder<TeacherResponse> builder)
        {
           // builder.HasOne(tr => tr.Student)
           //.WithMany()
           //.HasForeignKey(tr => tr.StudentId)
           //.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.Assignment)
          .WithMany()
          .HasForeignKey(tr => tr.AssignmentId)
          .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(tr => tr.StudentResponse)
		 .WithMany()
		 .HasForeignKey(tr => tr.StudentResponseId)
		 .OnDelete(DeleteBehavior.Restrict);

		}


    }
}
