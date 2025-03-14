using EventsApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<EventEntity> Events { get; set; }
    
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<EventUserEntity> EventUsers { get; set; }
    
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    
    public DbSet<ImageFileEntity> ImageFiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);   
    }
}
