using System.Linq.Expressions;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Repository.Interface;

namespace KALS.API.Models.Filter;

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