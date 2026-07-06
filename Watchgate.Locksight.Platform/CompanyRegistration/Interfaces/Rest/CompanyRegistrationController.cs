using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.CommandServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.QueryServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Transform;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/company-registration")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Company Registration endpoints.")]
public class CompanyRegistrationController(
    ICompanyRegistrationCommandService commandService,
    ICompanyRegistrationQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Register company account", Description = "Creates a company registration account linked to an IAM company.", OperationId = "RegisterCompanyAccount")]
    [SwaggerResponse(StatusCodes.Status201Created, "The company account was registered.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The company account already exists.")]
    public async Task<IActionResult> RegisterCompanyAccount([FromBody] RegisterCompanyAccountResource resource, CancellationToken cancellationToken)
    {
        var command = new RegisterCompanyAccountCommand(resource.CompanyId, resource.TradeName, resource.TaxId);
        var result = await commandService.Handle(command, cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => CreatedAtAction(nameof(GetCompanyAccountByCompanyId), new { companyId = account.CompanyId }, CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get company account", Description = "Gets company registration profile and status.", OperationId = "GetCompanyAccountByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The company account was retrieved.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company account was not found.")]
    public async Task<IActionResult> GetCompanyAccountByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var result = await queryService.Handle(new GetCompanyAccountByCompanyIdQuery(companyId), cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => Ok(CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }

    [HttpPut("company/{companyId:int}/profile")]
    [SwaggerOperation(Summary = "Complete company profile", Description = "Completes company profile after initial registration.", OperationId = "CompleteCompanyProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The company profile was completed.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company account was not found.")]
    public async Task<IActionResult> CompleteProfile(int companyId, [FromBody] CompleteCompanyProfileResource resource, CancellationToken cancellationToken)
    {
        var command = new CompleteCompanyProfileCommand(companyId, resource.LegalName, resource.Industry, resource.ContactPhone, resource.Address, resource.WebsiteUrl);
        var result = await commandService.Handle(command, cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => Ok(CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }

    [HttpPut("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Update company information", Description = "Updates company registration information.", OperationId = "UpdateCompanyInfo")]
    [SwaggerResponse(StatusCodes.Status200OK, "The company information was updated.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company account was not found.")]
    public async Task<IActionResult> UpdateCompanyInfo(int companyId, [FromBody] UpdateCompanyInfoResource resource, CancellationToken cancellationToken)
    {
        var command = new UpdateCompanyInfoCommand(companyId, resource.TradeName, resource.TaxId, resource.LegalName, resource.Industry, resource.ContactPhone, resource.Address, resource.WebsiteUrl);
        var result = await commandService.Handle(command, cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => Ok(CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }

    [HttpPatch("company/{companyId:int}/verify-email")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Verify administrator email", Description = "Verifies administrator email with the code generated during company registration.", OperationId = "VerifyCompanyEmail")]
    [SwaggerResponse(StatusCodes.Status200OK, "The administrator email was verified.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The verification code is invalid.")]
    public async Task<IActionResult> VerifyEmail(int companyId, [FromBody] VerifyCompanyEmailResource resource, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new VerifyCompanyEmailCommand(companyId, resource.VerificationCode), cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => Ok(CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }

    [HttpPatch("company/{companyId:int}/deactivate")]
    [SwaggerOperation(Summary = "Deactivate company account", Description = "Deactivates a company account.", OperationId = "DeactivateCompanyAccount")]
    [SwaggerResponse(StatusCodes.Status200OK, "The company account was deactivated.", typeof(CompanyAccountResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company account was not found.")]
    public async Task<IActionResult> DeactivateCompanyAccount(int companyId, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new DeactivateCompanyAccountCommand(companyId), cancellationToken);
        return CompanyRegistrationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            account => Ok(CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account)));
    }
}
