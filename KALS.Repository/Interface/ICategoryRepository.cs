using KALS.Domain.Entities;
using KALS.Domain.Paginate;

namespace KALS.Repository.Interface;

public interface ICategoryRepository: IGenericRepository<Category>
{
    Task<IPaginate<Category>> GetCategoriesPaginateAsync(int page, int size);
    Task<Category> GetCategoryByIdAsync(Guid id);
}