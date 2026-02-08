using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Presistence.Data.Contexts;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext , RoleManager<IdentityRole> _roleManager,
        UserManager<User> _userManager) : IDataSeeding
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

        public async Task SeedIdentityDataAsync()
        {
            try
            {
                //1- Seed roles [Admin , SuperAdmin]
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                //2- Seed users [AdminUser , SuperAdminUser]

                if (!_userManager.Users.Any())
                {
                    var adminUser = new User
                    {
                        DisplayName = "Admin",
                        UserName = "Admin",
                        Email = "Admin@gmail.com",
                        PhoneNumber = "1234567890",
                    };
                    var superAdminUser = new User
                    {
                        DisplayName = "SuperAdmin",
                        UserName = "SuperAdmin",
                        Email = "SuperAdmin@gmail.com",
                        PhoneNumber = "0123456789",
                    };
                    await _userManager.CreateAsync(adminUser, "P@ssw0rd");
                    await _userManager.CreateAsync(superAdminUser, "Pa$$w0rd");

                    //3- Assign users to roles
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
