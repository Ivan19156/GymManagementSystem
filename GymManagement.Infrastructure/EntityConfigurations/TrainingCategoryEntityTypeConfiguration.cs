using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class TrainingCategoryEntityTypeConfiguration : IEntityTypeConfiguration<TrainingCategory>
{
    public void Configure(EntityTypeBuilder<TrainingCategory> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Training__3213E83F6E580E8E");
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
    }
}