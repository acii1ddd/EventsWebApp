using EventsApp.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Surname { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    /// <summary>
    /// События этого участника
    /// </summary>
    // public List<EventEntity> Events { get; set; } = [];
    //
    /// <summary>
    /// Refresh token участника
    /// </summary>
    public RefreshTokenEntity RefreshToken { get; set; } = null!;
    
    //public Guid RefreshTokenId { get; set; }
    
    public List<EventUserEntity> EventUsers { get; set; } = [];
}

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    private const int MaxLength = 100;
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.BirthDate).IsRequired();
        // builder.Property(x => x.EventRegistrationDate).IsRequired();
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(MaxLength);
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        // связь с таблицей событий
        // builder.HasMany(x => x.Events)
        //     .WithMany(x => x.Users);
        
        // связь с таблицей токенов
        builder.HasOne(x => x.RefreshToken)
            .WithOne(x => x.User)
            .HasForeignKey<RefreshTokenEntity>(x => x.UserId);
        
        // связь со связующей таблицей с событиями
        builder.HasMany(x => x.EventUsers)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}