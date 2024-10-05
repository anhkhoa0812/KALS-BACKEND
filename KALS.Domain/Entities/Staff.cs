using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;
using KALS.Domain.Enums;

namespace KALS.Domain.Entities;

public class Staff : BaseEntity
{
    public StaffType Type { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}