using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.EntityConfigurations;

internal class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.UserId).HasName("PK__Clients__B9BE370F76341260");
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.MedicalNotes).HasColumnType("text");

        builder.HasOne(d => d.User).WithOne(p => p.Client)
            .HasForeignKey<Client>(d => d.UserId)
            .HasConstraintName("fk_clients_user");
    }
}