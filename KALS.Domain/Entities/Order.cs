using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;

namespace KALS.Domain.Entity;

public class Order: BaseEntity
{
    public decimal Total { get; set; }
    public string Status { get; set; }
    public string CreatedAt { get; set; }
    public string ModifiedAt { get; set; }
    public Guid MemberId { get; set; }
    [ForeignKey(nameof(MemberId))]
    public Member Member { get; set; }
    
    public Payment Payment { get; set; }
}