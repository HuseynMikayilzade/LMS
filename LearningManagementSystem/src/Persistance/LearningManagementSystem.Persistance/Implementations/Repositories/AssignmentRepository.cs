using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using LearningManagementSystem.Persistance.Implementations.Repositories.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Repositories
{
    public class AssignmentRepository : Repository<Assignment>,IAssignmentRepo
    {
        public AssignmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
