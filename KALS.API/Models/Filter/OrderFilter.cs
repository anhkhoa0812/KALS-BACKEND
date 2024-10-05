using System.Linq.Expressions;
using KALS.Domain.Enums;
using KALS.Repository.Interface;

namespace KALS.API.Models.Filter;

public class OrderFilter: IFilter<Domain.Entities.Order>
{
    public OrderStatus? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Username { get; set; }
    
    public Expression<Func<Domain.Entities.Order, bool>> ToExpression()
    {
        return order => 
            (!Status.HasValue || order.Status == Status) &&
            (!CreatedAt.HasValue || order.CreatedAt == CreatedAt) &&
            (string.IsNullOrEmpty(Username) || order.Member.User.Username.Contains(Username));
    }
}