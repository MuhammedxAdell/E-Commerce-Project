using Domain.Entities.ProductModule;
using Shared.Enums;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifictaions : BaseSpecifications<Product, int>
    {
        // Get All Products ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions(int? typeId, int? brandId , ProductSortingOptions sort) : 
            base( p =>  (!typeId.HasValue || p.TypeId == typeId) && 
                              (!brandId.HasValue || p.BrandId == brandId))
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
            //Switching on Sorting Options
            switch(sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy( p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending( p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy( p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending( p => p.Price);
                    break;
                case ProductSortingOptions.BrandAsc:
                    AddOrderBy( p => p.ProductBrand.Name);
                    break;
                case ProductSortingOptions.BrandDesc:
                    AddOrderByDescending( p => p.ProductBrand.Name);
                    break;
                case ProductSortingOptions.TypeAsc:
                    AddOrderBy( p => p.ProductType.Name);
                    break;
                case ProductSortingOptions.TypeDesc:
                    AddOrderByDescending( p => p.ProductType.Name);
                    break;
                default:
                    break;
            }
        }

        // Get Product By Id ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions(int id) : base(p => p.Id == id)
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
        }
    }
}
