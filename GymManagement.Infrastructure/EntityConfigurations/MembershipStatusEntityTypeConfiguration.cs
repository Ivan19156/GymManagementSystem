using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class MembershipStatusEntityTypeConfiguration : IEntityTypeConfiguration<MembershipStatus>
{
    public void Configure(EntityTypeBuilder<MembershipStatus> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Membersh__3213E83F3FBFB0EA");
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
    }
}