using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;
using KALS.Domain.Enums;

namespace KALS.Domain.Entities;

public class SupportRequest: BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public SupportRequestStatus Status { get; set; }
    public Guid MemberId { get; set; }
    [ForeignKey(nameof(MemberId))]
    public Member Member { get; set; }
    
    [ForeignKey("LabId,MemberId")]
    public LabMember LabMember { get; set; }
    public Guid? StaffId { get; set; }
    [ForeignKey(nameof(StaffId))]
    public Staff? Staff { get; set; }
    public Guid LabId { get; set; }
    [ForeignKey(nameof(LabId))]
    public Lab Lab { get; set; }
    
    public virtual ICollection<SupportMessage>? SupportMessages { get; set; }
}