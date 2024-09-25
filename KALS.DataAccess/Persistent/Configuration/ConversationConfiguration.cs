using KALS.Domain.Entity;
using KALS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KALS.DataAccess.Persistent.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Name)
            .HasConversion(
                v => v.ToString(),
                v => (RoleEnum)Enum.Parse(typeof(RoleEnum), v)
            );
    }
}