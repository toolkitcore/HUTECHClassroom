﻿using AutoMapper;
using HUTECHClassroom.Application.Classrooms.Commands.CreateClassroom;
using HUTECHClassroom.Application.Classrooms.Commands.UpdateClassroom;
using HUTECHClassroom.Application.Classrooms.DTOs;
using HUTECHClassroom.Application.Common.DTOs;
using HUTECHClassroom.Domain.Entities;

namespace HUTECHClassroom.Application.Classrooms;

public class ClassroomMappingProfile : Profile
{
    public ClassroomMappingProfile()
    {
        CreateMap<Classroom, ClassroomDTO>();
        CreateMap<CreateClassroomCommand, Classroom>();
        CreateMap<UpdateClassroomCommand, Classroom>()
            .ForAllMembers(options => options.Condition((src, des, srcValue, desValue) => srcValue != null));

        CreateMap<ClassroomUser, MemberDTO>()
            .ConstructUsing(x => new MemberDTO(x.User.UserName, x.User.Email));
        CreateMap<Group, ClassroomGroupDTO>();
    }
}