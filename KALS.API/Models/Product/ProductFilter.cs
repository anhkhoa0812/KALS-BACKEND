using System.Linq.Expressions;
using KALS.Repository.Interface;

namespace KALS.API.Models.Product;

public class ProductFilter: IFilter<Domain.Entities.Product>
{
    public string? Name { get; set; }
    public DateTime? CreateAt { get; set; }
    public Guid? CategoryId { get; set; }
    public Expression<Func<Domain.Entities.Product, bool>> ToExpression()
    {
        return product => 
            (string.IsNullOrEmpty(Name) || product.Name.Contains(Name)) &&
            (!CreateAt.HasValue || product.CreatedAt == CreateAt) &&
            (CategoryId == null || product.ProductCategories.Where(pc => pc.CategoryId == CategoryId).Any());
    }
}