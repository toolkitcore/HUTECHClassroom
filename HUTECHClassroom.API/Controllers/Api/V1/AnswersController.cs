﻿using HUTECHClassroom.Application.Answers;
using HUTECHClassroom.Application.Answers.Commands.CreateAnswer;
using HUTECHClassroom.Application.Answers.Commands.DeleteAnswer;
using HUTECHClassroom.Application.Answers.Commands.DeleteRangeAnswer;
using HUTECHClassroom.Application.Answers.Commands.UpdateAnswer;
using HUTECHClassroom.Application.Answers.DTOs;
using HUTECHClassroom.Application.Answers.Queries.GetAnswer;
using HUTECHClassroom.Application.Answers.Queries.GetAnswersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace HUTECHClassroom.API.Controllers.Api.V1;

[ApiVersion("1.0")]
public class AnswersController : BaseEntityApiController<AnswerDTO>
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<AnswerDTO>>> Get([FromQuery] AnswerPaginationParams @params)
        => HandlePaginationQuery<GetAnswersWithPaginationQuery, AnswerPaginationParams>(new GetAnswersWithPaginationQuery(@params));
    [HttpGet("{answerId}", Name = nameof(GetAnswerDetails))]
    public Task<ActionResult<AnswerDTO>> GetAnswerDetails(Guid answerId)
        => HandleGetQuery(new GetAnswerQuery(answerId));
    [HttpPost]
    public Task<ActionResult<AnswerDTO>> Post(CreateAnswerCommand command)
        => HandleCreateCommand(command, nameof(GetAnswerDetails), answerId => new GetAnswerQuery(answerId));
    [HttpPut("{answerId}")]
    public Task<IActionResult> Put(Guid answerId, UpdateAnswerCommand request)
        => HandleUpdateCommand(answerId, request);
    [HttpDelete("{answerId}")]
    public Task<ActionResult<AnswerDTO>> Delete(Guid answerId)
        => HandleDeleteCommand(new DeleteAnswerCommand(answerId));
    [HttpDelete]
    public Task<IActionResult> DeleteRange(IList<Guid> answerIds)
        => HandleDeleteRangeCommand(new DeleteRangeAnswerCommand(answerIds));
}
