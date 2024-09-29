using System.Linq.Expressions;

namespace KALS.Repository.Interface;

public interface IFilter<T>
{
    Expression<Func<T, bool>> ToExpression();
}