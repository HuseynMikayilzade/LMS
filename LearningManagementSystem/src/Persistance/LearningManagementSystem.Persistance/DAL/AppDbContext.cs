using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options /*,IHttpContextAccessor httpContextAccessor*/) : base(options)
        {
            //_httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<Room> Room { get; set; }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<GroupRoom> GroupRooms { get; set; }

        public DbSet<GroupSubject> GroupSubjects { get; set; }
        public DbSet<GroupTeacher> GroupTeachers { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentResponse> StudentResponses { get; set; }
        public DbSet<TeacherResponse> TeacherResponses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
           
        }
    

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();
            UpdateTime(entities);
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTime(IEnumerable<EntityEntry<BaseEntity>> entities)
        {
            //var currentuser =  _httpContextAccessor.HttpContext?.User.Identity?.Name;

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Modified:
                        entity.Entity.UpdateDate = DateTime.Now;
                        //if (currentuser != null )
                        //{ entity.Entity.UpdatedBy = currentuser; }         
                        break;
                    case EntityState.Added:
                        entity.Entity.CreateDate = DateTime.Now;
                        break;
                }
            }
        }
    }
}
