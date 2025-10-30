using Domain.Entities.OrderModule;

namespace Services.Specifications
{
    internal class OrderWithIncludesSpecefications : BaseSpecifications<Order, Guid>
    {
        //Get Order By Id with Includes
        public OrderWithIncludesSpecefications(Guid id) : base(o => o.Id == id)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems);
        }

        //Get Orders By User Email with Includes
        public OrderWithIncludesSpecefications(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems);
            AddOrderBy( o => o.OrderDate);
        }

    }
}
