using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }
            if (!context.ProductTypes.Any())
            {
                var productTypeData = File.ReadAllText("../Infastructure/Data/SeedData/types.json");
                var productType = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                context.ProductTypes.AddRange(productType);
            }
            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }
            if(context.ChangeTracker.HasChanges()){
                await context.SaveChangesAsync();
            }
        }
    }
}