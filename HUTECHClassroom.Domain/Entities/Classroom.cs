﻿using HUTECHClassroom.Domain.Common;

namespace HUTECHClassroom.Domain.Entities;

public class Classroom : BaseEntity
{
    public string Title { get; set; }
    public string Topic { get; set; }
    public string Room { get; set; }
    public string Description { get; set; }
    //public Semester Semester { get; set; } = Semester.Semester1;

    public Guid LecturerId { get; set; }
    public virtual ApplicationUser Lecturer { get; set; }
    public Guid FacultyId { get; set; }
    public virtual Faculty Faculty { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<ClassroomUser> ClassroomUsers { get; set; } = new HashSet<ClassroomUser>();
    public virtual ICollection<Exercise> Excercises { get; set; }
}
