using Catalog.API.Data;
using Catalog.API.Entites;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repostories
{
    public class ProductRespostory:IProductRespostory
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRespostory(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult=await _catalogContext.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount> 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {

            FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _catalogContext.Products
                .Find(filters)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _catalogContext.Products
                .Find(p=>p.Id == id)
                .FirstOrDefaultAsync();
            
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filters = Builders<Product>.Filter.ElemMatch(p=>p.Name,name);
            return await _catalogContext.Products
                .Find(filters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult=await _catalogContext.Products
                .ReplaceOneAsync(filter: g=>g.Id==product.Id,replacement:product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount> 0;
        }
    }
}
