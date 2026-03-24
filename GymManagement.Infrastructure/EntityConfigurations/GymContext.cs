using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure;

public class GymContext : DbContext
{
    public GymContext(DbContextOptions<GymContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<MembershipType> MembershipTypes => Set<MembershipType>();
    public DbSet<MembershipStatus> MembershipStatuses => Set<MembershipStatus>();
    public DbSet<ScheduledSession> ScheduledSessions => Set<ScheduledSession>();
    public DbSet<SessionStatus> SessionStatuses => Set<SessionStatus>();
    public DbSet<TrainingType> TrainingTypes => Set<TrainingType>();
    public DbSet<TrainingCategory> TrainingCategories => Set<TrainingCategory>();
    public DbSet<UserMembership> UserMemberships => Set<UserMembership>();
    public DbSet<Specialization> Specializations => Set<Specialization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()!));
            }
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()!));
            }
        }
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymContext).Assembly);
       
    }
    private static string ToSnakeCase(string name)
    {
        return string.Concat(name.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + char.ToLower(c) : char.ToLower(c).ToString()));
    }
}