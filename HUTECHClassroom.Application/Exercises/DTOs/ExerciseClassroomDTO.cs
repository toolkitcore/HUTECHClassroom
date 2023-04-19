﻿using HUTECHClassroom.Application.Common.DTOs;

namespace HUTECHClassroom.Application.Exercises.DTOs;

public record ExerciseClassroomDTO : BaseEntityDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Room { get; set; }
    public string Topic { get; set; }
}
