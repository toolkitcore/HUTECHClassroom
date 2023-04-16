﻿using AutoMapper;
using HUTECHClassroom.Application.Common.DTOs;
using HUTECHClassroom.Application.Missions.Commands.AddMissionUser;
using HUTECHClassroom.Application.Missions.Commands.CreateMission;
using HUTECHClassroom.Application.Missions.Commands.UpdateMission;
using HUTECHClassroom.Application.Missions.DTOs;
using HUTECHClassroom.Application.Projects.Commands.CreateProject;
using HUTECHClassroom.Application.Projects.Commands.UpdateProject;
using HUTECHClassroom.Application.Projects.DTOs;
using HUTECHClassroom.Domain.Entities;

namespace HUTECHClassroom.Application.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region User
            CreateMap<ApplicationUser, MemberDTO>();
            #endregion

            #region Missions
            CreateMap<Mission, MissionDTO>();
            CreateMap<CreateMissionCommand, Mission>();
            CreateMap<UpdateMissionCommand, Mission>()
                .ForAllMembers(options => options.Condition((src, des, srcValue, desValue) => srcValue != null));

            CreateMap<Project, MissionProjectDTO>();

            CreateMap<MissionUser, MemberDTO>()
                .ConstructUsing(x => new MemberDTO(x.User.UserName, x.User.Email));
            #endregion

            #region Projects
            CreateMap<Project, ProjectDTO>();
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectCommand, Project>()
                .ForAllMembers(options => options.Condition((src, des, srcValue, desValue) => srcValue != null));

            CreateMap<Mission, ProjectMissionDTO>();
            #endregion
        }
    }
}
