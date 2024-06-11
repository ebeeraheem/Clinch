using ClinchApi.Models;
using ClinchApi.Models.Results;

namespace ClinchApi.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResult> ProcessCheckoutAsync(CheckoutModel checkoutModel, string currentUserId);
    }
}