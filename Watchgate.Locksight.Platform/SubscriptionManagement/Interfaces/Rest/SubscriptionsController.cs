using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource)
    {
        var command = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, HttpContext.RequestAborted);
        
        if (result.IsFailure) return BadRequest(result.Error);
        
        var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
        return CreatedAtAction(nameof(GetByCompanyId), new { companyId = resource.CompanyId }, subscriptionResource);
    }

    [HttpGet("company/{companyId:int}")]
    public async Task<IActionResult> GetByCompanyId(int companyId)
    {
        var query = new GetSubscriptionByCompanyIdQuery(companyId);
        var subscription = await queryService.Handle(query, HttpContext.RequestAborted);
        
        if (subscription == null) return NotFound();
        
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }
    
    [HttpPut("{subscriptionId}/cancel")]
    public async Task<IActionResult> CancelSubscription(int subscriptionId)
    {
        var command = new CancelSubscriptionCommand(subscriptionId);
        var result = await commandService.Handle(command, HttpContext.RequestAborted);
        
        if (result.IsFailure) return BadRequest(result.Error);
        
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
    }
    
    [HttpPut("{subscriptionId:int}/change-plan")]
    public async Task<IActionResult> ChangePlan(int subscriptionId, [FromBody] ChangeSubscriptionPlanResource resource)
    {
        if (!Enum.TryParse<EPlanTier>(resource.NewTier, true, out var newTier))
        {
            return BadRequest("The plan is not valid");
        }

        var command = new ChangeSubscriptionPlanCommand(
            subscriptionId,
            newTier);

        var result = await commandService.Handle(command, HttpContext.RequestAborted);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error.ToString() });

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
    }
}