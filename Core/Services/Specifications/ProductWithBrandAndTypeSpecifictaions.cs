using Domain.Entities.ProductModule;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifictaions : BaseSpecifications<Product, int>
    {
        // Get All Products ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions() : base(null)
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
        }

        // Get Product By Id ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions(int id) : base(p => p.Id == id)
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
        }
    }
}
