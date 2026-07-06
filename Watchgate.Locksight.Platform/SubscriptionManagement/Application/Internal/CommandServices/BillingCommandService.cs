using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.CommandServices;

public class BillingCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRepository paymentRepository,
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : IBillingCommandService
{
    public async Task<Result<Invoice>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subscription = await subscriptionRepository.FindByIdWithPlanAsync(command.SubscriptionId, cancellationToken);
            if (subscription is null)
                return Result<Invoice>.Failure(SubscriptionManagementError.SubscriptionNotFound, $"Subscription with id {command.SubscriptionId} was not found.");

            if (subscription.Plan is null)
                return Result<Invoice>.Failure(SubscriptionManagementError.PlanNotFound, "The subscription plan was not found.");

            var reference = string.IsNullOrWhiteSpace(command.ProviderReference)
                ? $"sim-{Guid.NewGuid():N}"
                : command.ProviderReference;

            var payment = new Payment(subscription.Id, subscription.CompanyId, subscription.Plan.MonthlyPrice, command.Currency, reference);
            if (command.SimulateFailure)
                payment.MarkFailed();
            else
                payment.MarkProcessed();

            await paymentRepository.AddAsync(payment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            if (payment.Status == "FAILED")
                return Result<Invoice>.Failure(SubscriptionManagementError.PaymentFailed, "The simulated payment failed.");

            var invoice = new Invoice(payment.Id, subscription.Id, subscription.CompanyId, payment.Amount, payment.Currency);
            await invoiceRepository.AddAsync(invoice, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Invoice>.Success(invoice);
        }
        catch (OperationCanceledException)
        {
            return Result<Invoice>.Failure(SubscriptionManagementError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Invoice>.Failure(SubscriptionManagementError.DatabaseError, "A database error occurred while processing the payment.");
        }
        catch (Exception)
        {
            return Result<Invoice>.Failure(SubscriptionManagementError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
