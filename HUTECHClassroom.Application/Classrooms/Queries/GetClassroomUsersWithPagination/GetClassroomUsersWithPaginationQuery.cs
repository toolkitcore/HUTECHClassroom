﻿using EntityFrameworkCore.QueryBuilder.Interfaces;
using HUTECHClassroom.Application.Common.DTOs;
using HUTECHClassroom.Application.Common.Models;
using HUTECHClassroom.Application.Common.Requests;
using System.Linq.Expressions;

namespace HUTECHClassroom.Application.Classrooms.Queries.GetClassroomUsersWithPagination;

public record GetClassroomUsersWithPaginationQuery(Guid Id, PaginationParams Params) : GetWithPaginationQuery<MemberDTO, PaginationParams>(Params);
public class GetClassroomUsersWithPaginationQueryHandler : GetWithPaginationQueryHandler<ApplicationUser, GetClassroomUsersWithPaginationQuery, MemberDTO, PaginationParams>
{
    public GetClassroomUsersWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    protected override Expression<Func<ApplicationUser, bool>> FilterPredicate(GetClassroomUsersWithPaginationQuery query)
    {
        return x => x.ClassroomUsers.Any(x => x.ClassroomId == query.Id);
    }
    protected override Expression<Func<ApplicationUser, bool>> SearchStringPredicate(string searchString)
    {
        var toLowerSearchString = searchString.ToLower();
        return x => x.UserName.ToLower().Contains(toLowerSearchString) || x.Email.ToLower().Contains(toLowerSearchString);
    }
    protected override IQuery<ApplicationUser> Order(IMultipleResultQuery<ApplicationUser> query) => query.OrderBy(x => x.UserName);

}
