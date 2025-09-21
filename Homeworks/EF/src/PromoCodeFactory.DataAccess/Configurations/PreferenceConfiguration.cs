using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class PreferenceConfiguration 
        : IEntityTypeConfiguration<Preference>
    {
        public void Configure(EntityTypeBuilder<Preference> builder)
        {
            builder.ToTable("Preferences");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
