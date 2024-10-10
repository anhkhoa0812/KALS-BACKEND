using System.Linq.Expressions;

namespace KALS.Domain.Filter;

public interface IFilter<T>
{
    Expression<Func<T, bool>> ToExpression();
}