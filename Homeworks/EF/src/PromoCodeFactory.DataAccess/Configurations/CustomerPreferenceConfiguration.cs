using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class CustomerPreferenceConfiguration
        : IEntityTypeConfiguration<CustomerPreference>
    {
        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder.ToTable("CustomerPreferences");

            builder.HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            builder.HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.Preference)
                .WithMany(p => p.CustomerPreferences)
                .HasForeignKey(cp => cp.PreferenceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
