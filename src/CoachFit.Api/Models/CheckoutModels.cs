namespace CoachFit.Api.Models
{
    public record PriceResponse(int Amount, string Currency);

    public class CheckoutVerifyRequest
    {
        public string PaymentToken { get; set; } = ""; // e.g., provider reference id
        public string? Provider { get; set; }          // e.g., "paypal", "stripe"
        public string? PlanId { get; set; }            // optional: link purchase to a plan
    }

    public class CheckoutVerifyResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
