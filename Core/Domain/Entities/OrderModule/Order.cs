using ShippingAddress = Domain.Entities.OrderModule.Address;
namespace Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = string.Empty;
        public ShippingAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal SubTotal { get; set; } //OrderItem1.Q * OrderItem1.Price + ...
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; set; } = string.Empty;

    }
}
