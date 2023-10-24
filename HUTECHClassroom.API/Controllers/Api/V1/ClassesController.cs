﻿using HUTECHClassroom.Application.Classes.Commands.CreateClass;
using HUTECHClassroom.Application.Classes.DTOs;
using HUTECHClassroom.Application.Classes.Queries.GetClass;
using HUTECHClassroom.Application.Classes.Queries.GetClassesWithPaginationn;
using HUTECHClassroom.Application.Classs.Commands.DeleteClass;
using HUTECHClassroom.Application.Classs.Commands.DeleteRangeClass;
using HUTECHClassroom.Application.Classs.Commands.UpdateClass;

namespace HUTECHClassroom.API.Controllers.Api.V1;

[ApiVersion("1.0")]
public class ClassesController : BaseEntityApiController<string, ClassDTO>
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<ClassDTO>>> Get([FromQuery] PaginationParams paginationParams)
        => HandlePaginationQuery<GetClassesWithPaginationQuery, PaginationParams>(new GetClassesWithPaginationQuery(paginationParams));

    [HttpGet("{classId}")]
    public Task<ActionResult<ClassDTO>> GetClassDetails(string classId)
        => HandleGetQuery(new GetClassQuery(classId));

    [Authorize(CreateClassPolicy)]
    [HttpPost]
    public Task<ActionResult<ClassDTO>> Post(CreateClassCommand command)
        => HandleCreateCommand<CreateClassCommand, GetClassQuery>(command);

    [Authorize(UpdateClassPolicy)]
    [HttpPut("{classId}")]
    public Task<IActionResult> Put(string classId, UpdateClassCommand request)
        => HandleUpdateCommand(classId, request);

    [Authorize(DeleteClassPolicy)]
    [HttpDelete("{classId}")]
    public Task<ActionResult<ClassDTO>> Delete(string classId)
        => HandleDeleteCommand(new DeleteClassCommand(classId));

    [Authorize(DeleteClassPolicy)]
    [HttpDelete]
    public Task<IActionResult> DeleteRange(IList<string> classIds)
        => HandleDeleteRangeCommand(new DeleteRangeClassCommand(classIds));
}
