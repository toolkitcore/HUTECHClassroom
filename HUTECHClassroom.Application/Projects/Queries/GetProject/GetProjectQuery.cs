﻿using AutoMapper;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using HUTECHClassroom.Application.Common.Requests;
using HUTECHClassroom.Application.Projects.DTOs;
using HUTECHClassroom.Domain.Entities;
using System.Linq.Expressions;

namespace HUTECHClassroom.Application.Projects.Queries.GetProject
{
    public record GetProjectQuery(Guid Id) : GetQuery<ProjectDTO>;
    public class GetProjectQueryHandler : GetQueryHandler<Project, GetProjectQuery, ProjectDTO>
    {
        public GetProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
        public override Expression<Func<Project, bool>> FilterPredicate(GetProjectQuery query)
        {
            return x => x.Id == query.Id;
        }
        public override object GetNotFoundKey(GetProjectQuery query)
        {
            return query.Id;
        }
    }
}