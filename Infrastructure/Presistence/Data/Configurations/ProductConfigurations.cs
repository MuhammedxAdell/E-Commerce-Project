using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne( p => p.ProductBrand)
                   .WithMany() // Assuming ProductBrand does not have a collection of Products
                   .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.ProductType)
                     .WithMany() // Assuming ProductType does not have a collection of Products
                     .HasForeignKey(p => p.TypeId);

            builder.Property(p => p.Price)
                       .HasColumnType("decimal(15,2)");
        }
    }
}
