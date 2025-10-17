using Domain.Contracts;
using Presistence.Data.Contexts;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public async Task SeedDataAsync()
        {
            try
            {
                //Apply any pending migrations ==> apply database
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if ( pendingMigrations.Any() )
                {
                   await _dbContext.Database.MigrateAsync();
                }
                //Seed Data ==> if there is no data in the database
                if (!_dbContext.ProductBrands.Any())
                {
                    //var productBrandData = File.ReadAllText("D:\\.Net_Course\\8- Web APIs\\E-Commerce Solution\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json");
                    var productBrandsData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                              //Json ==> C# objects
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandsData);
                    if (productBrands is not null && productBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\types.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                            //Json ==> C# objects
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    var producsData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\products.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                          //Json ==> C# objects
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(producsData);
                    if (products is not null && products.Any())
                        await _dbContext.Products.AddRangeAsync(products);
                }
                await _dbContext.SaveChangesAsync(); // Save changes to the database
            }
            catch (Exception ex)
            {
                //Handle ex
            }
        }
    }
}
