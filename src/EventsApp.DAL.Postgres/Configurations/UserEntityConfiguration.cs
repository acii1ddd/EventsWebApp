using EventsApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Configurations;

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