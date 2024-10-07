using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;

namespace KALS.Domain.Entities;

public class LabMember
{
    public Guid LabId { get; set; }
    [ForeignKey(nameof(LabId))]
    public Lab Lab { get; set; }
    public Guid MemberId { get; set; }
    [ForeignKey(nameof(MemberId))]
    public Member Member { get; set; }
}