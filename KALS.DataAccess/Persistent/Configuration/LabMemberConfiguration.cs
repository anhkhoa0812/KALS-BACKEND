using KALS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KALS.DataAccess.Persistent.Configuration;

public class LabMemberConfiguration: IEntityTypeConfiguration<LabMember>
{
    public void Configure(EntityTypeBuilder<LabMember> builder)
    {
        builder.HasKey(lp => new { lp.LabId, lp.MemberId });

        builder
            .HasOne(lm => lm.Lab)
            .WithMany(l => l.LabMembers)
            .HasForeignKey(lm => lm.LabId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(lm => lm.Member)
            .WithMany(m => m.LabMembers)
            .HasForeignKey(lm => lm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}