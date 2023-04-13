﻿using AutoMapper;
using HUTECHClassroom.Application.Missions.Commands;
using HUTECHClassroom.Application.Missions.DTOs;
using HUTECHClassroom.Domain.Entities;

namespace HUTECHClassroom.Application.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Missions
            CreateMap<Mission, MissionDTO>();
            CreateMap<CreateMissionCommand, Mission>();
            CreateMap<UpdateMissionCommand, Mission>()
                .ForAllMembers(options => options.Condition((src, des, srcValue, desValue) => srcValue != null));
            #endregion
        }
    }
}
