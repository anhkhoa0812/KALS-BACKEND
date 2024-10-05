// using KALS.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace KALS.DataAccess.Persistent.Configuration;
//
// public class OrderPaymentConfiguration : IEntityTypeConfiguration<Order>
// {
//     public void Configure(EntityTypeBuilder<Order> builder)
//     {
//         builder.HasOne(o => o.Payment)
//             .WithOne(p => p.Order)
//             .HasForeignKey<Order>(p => p.PaymentId);
//     }
// }