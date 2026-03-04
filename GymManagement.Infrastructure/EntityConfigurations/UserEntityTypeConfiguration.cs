using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Users__3213E83F0AEA0218");
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false);
        builder.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false);
        builder.Property(e => e.Phone).HasMaxLength(20).IsUnicode(false);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
    }
}