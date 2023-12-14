using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(int categoryId);

        Task<List<Product>> GetProductsInCategoryAsync(int categoryId);

        Task<int> InsertAsync(Category category);

        Task<bool> UpdateAsync(Category category);

        Task<bool> DeleteByIdAsync(int categoryId);
    }
}