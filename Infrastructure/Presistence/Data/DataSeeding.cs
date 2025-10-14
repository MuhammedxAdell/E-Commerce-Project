using Domain.Contracts;
using Presistence.Data.Contexts;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public void SeedData()
        {
            try
            {
                //Apply any pending migrations ==> apply database
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }
                //Seed Data ==> if there is no data in the database
                if (!_dbContext.ProductBrands.Any())
                {
                    //var productBrandData = File.ReadAllText("D:\\.Net_Course\\8- Web APIs\\E-Commerce Solution\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json");
                    var productBrandsData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                              //Json ==> C# objects
                    var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandsData);
                    if (productBrands is not null && productBrands.Any())
                        _dbContext.ProductBrands.AddRange(productBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    //var productBrandData = File.ReadAllText("D:\\.Net_Course\\8- Web APIs\\E-Commerce Solution\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json");
                    var productTypesData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\types.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                            //Json ==> C# objects
                    var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                        _dbContext.ProductTypes.AddRange(productTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    //var productBrandData = File.ReadAllText("D:\\.Net_Course\\8- Web APIs\\E-Commerce Solution\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json");
                    var producsData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\products.json"); // Relative path using .. to go back to the Infrastructure folder
                                                                                                                          //Json ==> C# objects
                    var products = JsonSerializer.Deserialize<List<Product>>(producsData);
                    if (products is not null && products.Any())
                        _dbContext.Products.AddRange(products);
                }
                _dbContext.SaveChanges(); // Save changes to the database
            }
            catch (Exception ex)
            {
                //Handle ex
            }
        }
    }
}
