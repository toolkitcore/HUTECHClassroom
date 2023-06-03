﻿using EntityFrameworkCore.Repository.Collections;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace HUTECHClassroom.API.Controllers.Api;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IActionDescriptorCollectionProvider MyActionDescriptionCollectionProvider => HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();

    protected ActionResult<IEnumerable<T>> HandlePagedList<T>(IPagedList<T> pagedList)
    {
        Response.Headers.Add("pagination", JsonConvert.SerializeObject(new { currentPage = pagedList.PageIndex, itemsPerPage = pagedList.PageSize, totalItems = pagedList.TotalCount, totalPages = pagedList.TotalPages, hasNext = pagedList.HasNextPage, hasPrevious = pagedList.HasPreviousPage }));
        return Ok(pagedList.Items);
    }

    [HttpGet("endpoints")]
    public ActionResult<IEnumerable<string>> GetEndpoints()
    {
        var controllerEndpoints = new List<Endpoint>();

        var controllerActions = MyActionDescriptionCollectionProvider.ActionDescriptors.Items
            .Where(descriptor => descriptor is ControllerActionDescriptor controllerDescriptor &&
                                  controllerDescriptor.ControllerTypeInfo == GetType())
            .Cast<ControllerActionDescriptor>();

        foreach (var actionDescriptor in controllerActions)
        {
            var httpMethods = string.Join(", ", actionDescriptor.ActionConstraints.OfType<HttpMethodActionConstraint>().SelectMany(x => x.HttpMethods));
            var routeTemplate = actionDescriptor.AttributeRouteInfo?.Template;

            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();

            var endpoint = new Endpoint { Method = httpMethods, Url = routeTemplate.Replace("{version:apiVersion}", apiVersion) };

            controllerEndpoints.Add(endpoint);
        }

        return Ok(controllerEndpoints);
    }
    record Endpoint
    {
        public string Method { get; set; }
        public string Url { get; set; }
    }
}



