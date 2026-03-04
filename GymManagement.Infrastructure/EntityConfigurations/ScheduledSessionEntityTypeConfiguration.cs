using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class ScheduledSessionEntityTypeConfiguration : IEntityTypeConfiguration<ScheduledSession>
{
    public void Configure(EntityTypeBuilder<ScheduledSession> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Schedule__3213E83F39228270");
        builder.HasIndex(e => e.ClientId).HasDatabaseName("idx_sessions_client");
        builder.HasIndex(e => e.TrainerId).HasDatabaseName("idx_sessions_trainer");
        builder.HasIndex(e => new { e.SessionDate, e.StartTime }).HasDatabaseName("idx_sessions_date_time");
        builder.Property(e => e.Notes).HasColumnType("text");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        builder.HasOne(d => d.Client).WithMany(p => p.ScheduledSessions)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_sessions_client");

        builder.HasOne(d => d.Trainer).WithMany(p => p.ScheduledSessions)
            .HasForeignKey(d => d.TrainerId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_sessions_trainer");

        builder.HasOne(d => d.Status).WithMany(p => p.ScheduledSessions)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_sessions_status");

        builder.HasOne(d => d.TrainingType).WithMany(p => p.ScheduledSessions)
            .HasForeignKey(d => d.TrainingTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_sessions_training_type");
    }
}