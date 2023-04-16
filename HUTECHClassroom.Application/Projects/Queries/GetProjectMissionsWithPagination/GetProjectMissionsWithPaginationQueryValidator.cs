﻿using FluentValidation;
using HUTECHClassroom.Application.Common.Validators;
using HUTECHClassroom.Application.Projects.DTOs;

namespace HUTECHClassroom.Application.Projects.Queries.GetProjectMissionsWithPagination
{
    public class GetProjectMissionsWithPaginationQueryValidator : GetWithPaginationQueryValidator<GetProjectMissionsWithPaginationQuery, ProjectMissionDTO>
    {
        public GetProjectMissionsWithPaginationQueryValidator()
        {
        }
    }
}
