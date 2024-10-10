using System.Linq.Expressions;
using KALS.Domain.Entities;
using KALS.Domain.Filter;

namespace KALS.Domain.Filter.FilterModel;

public class MemberFilter: IFilter<Member>
{
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public Expression<Func<Member, bool>> ToExpression()
    {
        return member =>
            (string.IsNullOrEmpty(Username) || member.User.Username.Contains(Username)) &&
            (string.IsNullOrEmpty(PhoneNumber) || member.User.PhoneNumber.Contains(PhoneNumber));
    }
}