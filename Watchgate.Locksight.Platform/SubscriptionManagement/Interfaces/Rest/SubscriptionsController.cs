using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Subscription Management endpoints.")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryService subscriptionQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("plans")]
    [SwaggerOperation(Summary = "Create a subscription plan", Description = "Creates a plan that can be selected from the landing page or web app.", OperationId = "CreateSubscriptionPlan")]
    [SwaggerResponse(StatusCodes.Status201Created, "The subscription plan was created.", typeof(SubscriptionPlanResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The subscription plan could not be created.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanResource resource, CancellationToken cancellationToken)
    {
        var command = CreateSubscriptionPlanCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            plan => CreatedAtAction(nameof(GetPlanById), new { planId = plan.Id }, SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity(plan)));
    }

    [HttpGet("plans")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get active subscription plans", Description = "Lists active plans for public pricing and plan selection views.", OperationId = "GetSubscriptionPlans")]
    [SwaggerResponse(StatusCodes.Status200OK, "The active plans were retrieved.", typeof(IEnumerable<SubscriptionPlanResource>))]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var result = await subscriptionQueryService.Handle(new GetAllSubscriptionPlansQuery(), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            plans => Ok(plans.Select(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("plans/{planId:int}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get subscription plan by ID", Description = "Gets a subscription plan by identifier.", OperationId = "GetSubscriptionPlanById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription plan was retrieved.", typeof(SubscriptionPlanResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription plan was not found.")]
    public async Task<IActionResult> GetPlanById(int planId, CancellationToken cancellationToken)
    {
        var result = await subscriptionQueryService.Handle(new GetSubscriptionPlanByIdQuery(planId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            plan => Ok(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity(plan)));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a company subscription", Description = "Creates an active subscription for a company after selecting a plan.", OperationId = "CreateSubscription")]
    [SwaggerResponse(StatusCodes.Status201Created, "The subscription was created.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The selected plan was not found.")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The company already has an active subscription.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateSubscriptionCommand(resource.CompanyId, resource.PlanId);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            subscription => CreatedAtAction(nameof(GetSubscriptionById), new { subscriptionId = subscription.Id }, SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)));
    }

    [HttpGet("{subscriptionId:int}")]
    [SwaggerOperation(Summary = "Get subscription by ID", Description = "Gets a subscription with its selected plan.", OperationId = "GetSubscriptionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription was retrieved.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetSubscriptionById(int subscriptionId, CancellationToken cancellationToken)
    {
        var result = await subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(subscriptionId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            subscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get company subscriptions", Description = "Lists the subscription history for a company.", OperationId = "GetSubscriptionsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscriptions were retrieved.", typeof(IEnumerable<SubscriptionResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetSubscriptionsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var result = await subscriptionQueryService.Handle(new GetSubscriptionsByCompanyIdQuery(companyId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            subscriptions => Ok(subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPatch("{subscriptionId:int}/plan")]
    [SwaggerOperation(Summary = "Change subscription plan", Description = "Changes the plan assigned to an existing subscription.", OperationId = "ChangeSubscriptionPlan")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription plan was changed.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription or plan was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ChangePlan(int subscriptionId, [FromBody] ChangeSubscriptionPlanResource resource, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(new ChangeSubscriptionPlanCommand(subscriptionId, resource.PlanId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            subscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)));
    }

    [HttpPatch("{subscriptionId:int}/cancel")]
    [SwaggerOperation(Summary = "Cancel subscription", Description = "Cancels an existing subscription.", OperationId = "CancelSubscription")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription was cancelled.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CancelSubscription(int subscriptionId, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(new CancelSubscriptionCommand(subscriptionId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            subscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)));
    }
}