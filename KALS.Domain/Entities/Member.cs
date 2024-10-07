using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;

namespace KALS.Domain.Entities;

public class Member : BaseEntity
{
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    [MaxLength(255)]
    public string? Province { get; set; }
    [MaxLength(255)]
    public string? District { get; set; }
    [MaxLength(255)]
    public string? Ward { get; set; }
    [MaxLength(500)]
    public string? Address { get; set; }
    
    public virtual ICollection<LabMember>? LabMembers { get; set; }
}