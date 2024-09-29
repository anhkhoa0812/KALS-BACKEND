using KALS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KALS.DataAccess.Persistent.Configuration;

public class ProductRelationshipConfiguration: IEntityTypeConfiguration<ProductRelationship>
{
    public void Configure(EntityTypeBuilder<ProductRelationship> builder)
    {
        builder.HasKey(pr => new { pr.ParentProductId, pr.ChildProductId });

        // Configure the relationships
        builder
            .HasOne(pr => pr.ParentProduct)
            .WithMany(p => p.ChildProducts)
            .HasForeignKey(pr => pr.ParentProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(pr => pr.ChildProduct)
            .WithMany(p => p.ParentProducts)
            .HasForeignKey(pr => pr.ChildProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}