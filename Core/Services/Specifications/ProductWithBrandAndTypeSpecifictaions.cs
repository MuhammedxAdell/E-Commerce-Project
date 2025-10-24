using Domain.Entities.ProductModule;
using Shared;
using Shared.Enums;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifictaions : BaseSpecifications<Product, int>
    {
        // Get All Products ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions(ProductSpecificationParameters parameters) : 
            base( p =>  (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId) && 
                              (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId) && 
                              (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))                                                                      )
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
            //Switching on Sorting Options
            switch(parameters.Sort)
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
            //Pagination
            ApplyPagination(parameters.PageSize , parameters.PageIndex);
        }

        // Get Product By Id ==> Include : ProductBrand and ProductType
        public ProductWithBrandAndTypeSpecifictaions(int id) : base(p => p.Id == id)
        {
            AddIncludes( p => p.ProductBrand);
            AddIncludes( p => p.ProductType);
        }
    }
}
