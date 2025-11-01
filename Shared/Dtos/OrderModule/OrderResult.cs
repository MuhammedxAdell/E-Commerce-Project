namespace Shared.Dtos.OrderModule
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        public string UserEmail { get; init; } = string.Empty;
        public AddressDto ShippingAddress { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string PaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; } = string.Empty;
        public int? DeliveryMethodId { get; init; }
        public decimal SubTotal { get; init; } //OrderItem1.Q * OrderItem1.Price + ...
        public decimal Total { get; init; } // SubTotal + DeliveryPrice
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;
    }
}
