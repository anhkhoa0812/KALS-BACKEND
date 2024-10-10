using System.Linq.Expressions;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Domain.Filter;

namespace KALS.Domain.Filter.FilterModel;

public class StaffFilter: IFilter<Staff>
{
    public StaffType? Type { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public Expression<Func<Staff, bool>> ToExpression()
    {
        return staff =>
        (!Type.HasValue || staff.Type == Type) &&
        (string.IsNullOrEmpty(Username) || staff.User.Username.Contains(Username)) &&
        (string.IsNullOrEmpty(PhoneNumber) || staff.User.PhoneNumber.Contains(PhoneNumber));
    }
}