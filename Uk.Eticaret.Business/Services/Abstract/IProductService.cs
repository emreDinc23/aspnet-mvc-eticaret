using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services.Abstract
{
    public interface IProductService
    {
        IQueryable<Product> GetAllQueryable();

        Task<List<Product>> GetAllProductAsync();

        void CreateProductAsync(Product createProduct);

        void UpdateProductAsync(Product updateProduct);

        void DeleteProductAsync(int id);

        Task<Product> GetProduct(string slug);
    }
}