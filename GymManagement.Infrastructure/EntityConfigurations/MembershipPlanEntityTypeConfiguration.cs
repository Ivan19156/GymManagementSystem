using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class MembershipPlanEntityTypeConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Membersh__3213E83F3E1F5BB7");
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.Description).HasColumnType("text");
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.IsActive).HasDefaultValue(true);

        builder.HasOne(d => d.Type).WithMany(p => p.MembershipPlans)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_membership_plans_type");
    }
}