﻿using HUTECHClassroom.Application.Common.Models;
using HUTECHClassroom.Domain.Enums;

namespace HUTECHClassroom.Application.Majors;

public record MajorPaginationParams(int? PageNumber, int? PageSize, string SearchString) : PaginationParams(PageNumber, PageSize, SearchString)
{
    public SortingOrder TitleOrder { get; set; }
    public SortingOrder TotalCreditsOrder { get; set; }
    public SortingOrder NonComulativeCreditsOrder { get; set; }
}
