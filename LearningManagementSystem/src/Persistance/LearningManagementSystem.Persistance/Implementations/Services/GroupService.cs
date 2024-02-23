using AutoMapper;
using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class GroupService : IGroupService
    {

        private readonly IGroupRepo _repo;
        private readonly ISubjectRepo _subjectRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IAssignmentRepo _assignmentRepo;
        private readonly IStudentResponseRepo _studentResponseRepo;
		private readonly IAttendanceRepo _attendanceRepo;

		public GroupService(IGroupRepo repo, ISubjectRepo subjectRepo
            , IRoomRepo roomRepo, IStudentRepo studentRepo
            , IAssignmentRepo assignmentRepo,IStudentResponseRepo studentResponseRepo,IAttendanceRepo attendanceRepo)
        {
            _repo = repo;
            _subjectRepo = subjectRepo;
            _roomRepo = roomRepo;
            _studentRepo = studentRepo;
            _assignmentRepo = assignmentRepo;
            _studentResponseRepo = studentResponseRepo;
			_attendanceRepo = attendanceRepo;
		}

        public async Task<bool> CreateAsync(CreateGroupVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (await _repo.IsExist(l => l.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This group is already exist");
                return false;
            }
            Group group = new Group
            {
                CreateDate = DateTime.UtcNow,
                Name = vm.Name,
                MaxStudent = vm.MaxStudent,
                CurrentStudent = 0,
                GroupSubjects = new List<GroupSubject>(),
                GroupRooms = new List<GroupRoom>(),
            };
            if (vm.SubjectIds != null)
            {
                foreach (var item in vm.SubjectIds)
                {
                    if (!await _subjectRepo.IsExist(t => t.Id == item))
                    {
                        modelstate.AddModelError("SubjectIds", "This subject isn't aviable");
                        return false;
                    }
                    group.GroupSubjects.Add(new GroupSubject
                    {
                        SubjectId = item
                    }); ;
                }
            }
            if (vm.RoomIds != null)
            {

                foreach (var item in vm.RoomIds)
                {
                    if (!await _roomRepo.IsExist(t => t.Id == item))
                    {
                        modelstate.AddModelError("RoomIds", "This room isn't aviable");
                        return false;
                    }
                    int groupcapacity = vm.MaxStudent;
                    var room = await _roomRepo.GetByIdAsync(item);
                    if (room.Capacity < groupcapacity)
                    {
                        modelstate.AddModelError("RoomIds", "The capacity of this room is not suitable");
                    }
                    group.GroupRooms.Add(new GroupRoom
                    {
                        RoomId = item
                    }); ;
                }
            }
            await _repo.AddAsync(group);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CreateGroupVm> CreatedAsync(CreateGroupVm vm)
        {

            vm.Subjects = await _subjectRepo.GetAll().ToListAsync();
            vm.Rooms = await _roomRepo.GetAll().ToListAsync();
            return vm;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Group exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<PaginationVm<Group>> GetAllAsync(int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            ICollection<Group> groups = await _repo.GetAllWhere(skip: (page - 1) * take, take: take, orderexpression: x => x.Id, isDescending: true).ToListAsync();
            if (groups == null) throw new NotFoundException("Not found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Group> vm = new PaginationVm<Group>
            {
                Items = groups,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

        public async Task<bool> UpdateAsync(int id, UpdateGroupVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            Group exist = await _repo.GetByIdAsync(id, includes: new string[] { "GroupSubjects", "GroupSubjects.Subject", "GroupRooms", "GroupRooms.Room" });
            if (exist == null) throw new NotFoundException("Not found");
            if (exist.Name != vm.Name)
            {
                if (await _repo.IsExist(l => l.Name == vm.Name))
                {
                    modelstate.AddModelError("Name", "This group is already exist");
                    return false;
                }
            }
            if (vm.SubjectIds != null)
            {
                foreach (var item in vm.SubjectIds)
                {
                    if (!exist.GroupSubjects.Any(gs => gs.SubjectId == item))
                    {
                        if (!await _subjectRepo.IsExist(c => c.Id == item)) throw new NotFoundException("This subject is not available");
                        exist.GroupSubjects.Add(new GroupSubject { SubjectId = item });

                    }
                }
                exist.GroupSubjects = exist.GroupSubjects.Where(pc => vm.SubjectIds.Any(x => pc.SubjectId == x)).ToList();
            }
            else
            {
                exist.GroupSubjects = new List<GroupSubject>();
            }
            if (vm.RoomIds != null)
            {
                foreach (var item in vm.RoomIds)
                {
                    if (!exist.GroupRooms.Any(gs => gs.RoomId == item))
                    {
                        if (!await _roomRepo.IsExist(c => c.Id == item)) throw new NotFoundException("This subject is not available");
                        exist.GroupRooms.Add(new GroupRoom { RoomId = item });
                    }
                }
                exist.GroupRooms = exist.GroupRooms.Where(pc => vm.RoomIds.Any(x => pc.RoomId == x)).ToList();
            }
            else
            {
                exist.GroupRooms = new List<GroupRoom>();
            }
            exist.Name = vm.Name;
            var result = await _studentRepo.GetAllWhere(st => st.GroupId == exist.Id).CountAsync();
            exist.CurrentStudent = (byte)result;
            exist.UpdateDate = DateTime.UtcNow;
            exist.MaxStudent = vm.MaxStudent;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<UpdateGroupVm> UpdatedAsync(int id, UpdateGroupVm vm)
        {
            Group exist = await _repo.GetByIdAsync(id, includes: new string[] { "GroupSubjects", "GroupSubjects.Subject", "GroupRooms", "GroupRooms.Room" });
            if (exist == null) throw new NotFoundException("Not found");
            vm.Name = exist.Name;
            vm.MaxStudent = exist.MaxStudent;

            vm.Subjects = await _subjectRepo.GetAll().ToListAsync();
            vm.SubjectIds = exist.GroupSubjects.Select(gs => gs.SubjectId).ToList();

            vm.Rooms = await _roomRepo.GetAll().ToListAsync();
            vm.RoomIds = exist.GroupRooms.Select(gs => gs.RoomId).ToList();
            return vm;
        }


        public async Task<GroupItemVm> GetGroupItems(int id,int attendancepage)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            ICollection<Student> students = await _studentRepo.GetAllWhere(s => s.GroupId == id).ToListAsync();
            if (students == null) throw new NotFoundException("Not found");

            ICollection<Assignment> assignments = await _assignmentRepo.GetAllWhere(a => a.GroupId == id, x => x.Id, isDescending: true).ToListAsync();
            if (assignments == null) throw new NotFoundException("Not found");

            Group group = await _repo.GetByIdAsync(id, includes: new string[] { "GroupSubjects", "GroupSubjects.Subject", "GroupRooms", "GroupRooms.Room" });
            if (group == null) throw new NotFoundException("Not found");   
            var assignmentcount = assignments.Select(x => x.IsActive == true).Count();

            List<Attendance> attendances = await _attendanceRepo.GetAllWhere(orderexpression: x => x.Date, isDescending: true, includes: nameof(Group)).ToListAsync();
            if (attendances == null) throw new NotFoundException("Not found");
            int count = attendances.Count();
			if (count < 0) throw new NotFoundException("Not found");
            double totalpageattendance = Math.Ceiling((double)count/7);
			PaginationVm<Attendance> AttendancepaginationVm = new PaginationVm<Attendance>
            {
                Items = attendances.Skip((attendancepage-1)*7).Take(7).ToList(),
                CurrentPage = attendancepage,
                TotalPage = totalpageattendance
			};

            GroupItemVm vm = new GroupItemVm
            {
                Group = group,
                Assignments = assignments,
                Students = students,
                AssignmentCount = assignmentcount,
                StudentCount = students.Count(),      
                PaginationAttendances = AttendancepaginationVm
            };
            return vm;
        }      
    }
}