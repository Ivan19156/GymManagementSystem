using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class SessionStatusEntityTypeConfiguration : IEntityTypeConfiguration<SessionStatus>
{
    public void Configure(EntityTypeBuilder<SessionStatus> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__SessionS__3213E83F09D3A44E");
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
    }
}