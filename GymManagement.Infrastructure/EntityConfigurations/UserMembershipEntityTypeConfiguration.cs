using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class UserMembershipEntityTypeConfiguration : IEntityTypeConfiguration<UserMembership>
{
    public void Configure(EntityTypeBuilder<UserMembership> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__UserMemb__3213E83FFEE12C1D");
        builder.Property(e => e.SessionsUsed).HasDefaultValue(0);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        builder.HasOne(d => d.Client).WithMany(p => p.UserMemberships)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_user_memberships_client");

        builder.HasOne(d => d.MembershipPlan).WithMany(p => p.UserMemberships)
            .HasForeignKey(d => d.MembershipPlanId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_user_memberships_plan");

        builder.HasOne(d => d.Status).WithMany(p => p.UserMemberships)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_user_memberships_status");
    }
}