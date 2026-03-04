using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class AdminEntityTypeConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(e => e.UserId).HasName("PK__Admins__B9BE370F5EAC4B29");
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.AccessLevel).HasDefaultValue(1);

        builder.HasOne(d => d.User).WithOne(p => p.Admin)
            .HasForeignKey<Admin>(d => d.UserId)
            .HasConstraintName("fk_admins_user");
    }
}