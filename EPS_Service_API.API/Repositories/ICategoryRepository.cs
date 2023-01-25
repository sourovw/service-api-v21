using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetCategories();
        Task<CategoryModel> GetCategoryById(int categoryId);
        Task<CategoryModel> GetCategoryByName(string categoryName);
        Task<int> InsertCategory(CategoryModel category);
        Task<List<CategoryModel>> GetCategoriesWithPies();
    }
}