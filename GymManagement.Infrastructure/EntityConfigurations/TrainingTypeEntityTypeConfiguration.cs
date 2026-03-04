using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class TrainingTypeEntityTypeConfiguration : IEntityTypeConfiguration<TrainingType>
{
    public void Configure(EntityTypeBuilder<TrainingType> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Training__3213E83F8711FA15");
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.Description).HasColumnType("text");

        builder.HasOne(d => d.Category).WithMany(p => p.TrainingTypes)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_training_types_category");
    }
}