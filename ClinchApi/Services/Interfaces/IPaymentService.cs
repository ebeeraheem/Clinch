using ClinchApi.Models.Results;

namespace ClinchApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(string paymentToken, decimal amount);
    }
}