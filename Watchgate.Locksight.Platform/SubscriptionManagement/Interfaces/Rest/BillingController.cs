using System.Net.Mime;
using System.Text;
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
[Route("api/v1/billing")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Subscription Billing endpoints.")]
public class BillingController(
    IBillingCommandService billingCommandService,
    IBillingQueryService billingQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("payments")]
    [SwaggerOperation(Summary = "Process subscription payment", Description = "Processes a simulated Stripe payment and issues an invoice when successful.", OperationId = "ProcessSubscriptionPayment")]
    [SwaggerResponse(StatusCodes.Status201Created, "The payment was processed and an invoice was issued.", typeof(InvoiceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription or plan was not found.")]
    [SwaggerResponse(StatusCodes.Status402PaymentRequired, "The payment failed.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentResource resource, CancellationToken cancellationToken)
    {
        var command = new ProcessPaymentCommand(resource.SubscriptionId, resource.Currency, resource.ProviderReference, resource.SimulateFailure);
        var result = await billingCommandService.Handle(command, cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            invoice => CreatedAtAction(nameof(GetInvoiceById), new { invoiceId = invoice.Id }, InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice)));
    }

    [HttpGet("invoices/{invoiceId:int}")]
    [SwaggerOperation(Summary = "Get invoice by ID", Description = "Gets a subscription invoice by identifier.", OperationId = "GetInvoiceById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invoice was retrieved.", typeof(InvoiceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetInvoiceById(int invoiceId, CancellationToken cancellationToken)
    {
        var result = await billingQueryService.Handle(new GetInvoiceByIdQuery(invoiceId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            invoice => Ok(InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice)));
    }

    [HttpGet("invoices/company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get company invoices", Description = "Lists billing invoices for a company.", OperationId = "GetInvoicesByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invoices were retrieved.", typeof(IEnumerable<InvoiceResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetInvoicesByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var result = await billingQueryService.Handle(new GetInvoicesByCompanyIdQuery(companyId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            invoices => Ok(invoices.Select(InvoiceResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("invoices/{invoiceId:int}/receipt")]
    [Produces(MediaTypeNames.Text.Plain)]
    [SwaggerOperation(Summary = "Download payment receipt", Description = "Downloads a text receipt for a processed subscription payment.", OperationId = "DownloadPaymentReceipt")]
    [SwaggerResponse(StatusCodes.Status200OK, "The receipt was downloaded.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> DownloadReceipt(int invoiceId, CancellationToken cancellationToken)
    {
        var result = await billingQueryService.Handle(new GetInvoiceByIdQuery(invoiceId), cancellationToken);
        return SubscriptionActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            invoice =>
            {
                var bytes = Encoding.UTF8.GetBytes(invoice.BuildReceiptText());
                return File(bytes, MediaTypeNames.Text.Plain, $"{invoice.Number}.txt");
            });
    }
}
