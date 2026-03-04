using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class TrainerEntityTypeConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(e => e.UserId).HasName("PK__Trainers__B9BE370FEA91E35D");
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.Bio).HasColumnType("text");
        builder.Property(e => e.HourlyRate).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.IsAvailable).HasDefaultValue(true);

        builder.HasOne(d => d.User).WithOne(p => p.Trainer)
            .HasForeignKey<Trainer>(d => d.UserId)
            .HasConstraintName("fk_trainers_user");

        builder.HasOne(d => d.Specialization).WithMany(p => p.Trainers)
            .HasForeignKey(d => d.SpecializationId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_trainers_specialization");
    }
}