using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;
using KALS.Domain.Enums;

namespace KALS.Domain.Entities;

public class Order: BaseEntity
{
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public string CreatedAt { get; set; }
    public string ModifiedAt { get; set; }
    public Guid MemberId { get; set; }
    [ForeignKey(nameof(MemberId))]
    public Member Member { get; set; }
    
    public Payment Payment { get; set; }
}