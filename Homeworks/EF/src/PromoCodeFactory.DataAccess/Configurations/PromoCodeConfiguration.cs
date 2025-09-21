using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class PromoCodeConfiguration 
        : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.ToTable("PromoCodes");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ServiceInfo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.PartnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.BeginDate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            builder.HasOne(p => p.PartnerManager)
                .WithMany(e => e.PromoCodes)
                .HasForeignKey(p => p.PartnerManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Preference)
                .WithMany()
                .HasForeignKey(p => p.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Customer)
                .WithMany(c => c.PromoCodes)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
