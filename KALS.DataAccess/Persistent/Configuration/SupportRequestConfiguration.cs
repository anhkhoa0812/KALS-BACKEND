using KALS.Domain.Entities;
using KALS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KALS.DataAccess.Persistent.Configuration;

public class SupportRequestConfiguration: IEntityTypeConfiguration<SupportRequest>
{
    public void Configure(EntityTypeBuilder<SupportRequest> builder)
    {
        builder
            .HasOne(sr => sr.Member)
            .WithMany()
            .HasForeignKey(sr => sr.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(sr => sr.Staff)
            .WithMany()
            .HasForeignKey(sr => sr.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(sr => sr.Lab)
            .WithMany()
            .HasForeignKey(sr => sr.LabId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(sr => sr.Status)
            .HasConversion(
                v => v.ToString(),
                v => (SupportRequestStatus)Enum.Parse(typeof(SupportRequestStatus), v)
            );
        builder
            .HasOne(sr => sr.LabMember)
            .WithMany()
            .HasForeignKey(sr => new { sr.LabId, sr.MemberId })
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}