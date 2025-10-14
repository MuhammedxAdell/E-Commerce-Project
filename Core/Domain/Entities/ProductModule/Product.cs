namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;

        //1-M Product-ProductType
        public ProductType ProductType { get; set; } // Navigation Property
        public int TypeId { get; set; } // Foreign Key

        //1-M Product-ProductBrand
        public ProductBrand ProductBrand { get; set; } // Navigation Property
        public int BrandId { get; set; } // Foreign Key
    }
}