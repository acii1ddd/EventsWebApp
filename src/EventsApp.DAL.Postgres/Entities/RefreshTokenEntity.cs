using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата окончания срока жизни токена
    /// </summary>
    public DateTime ExpiryDate { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// Владелец токена
    /// </summary>
    public Guid ParticipantId { get; set; }

    public ParticipantEntity Participant { get; set; } = null!;
}

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired();

        builder.HasIndex(x => x.ParticipantId).IsUnique();
        
        builder.HasOne(x => x.Participant)
            .WithOne(x => x.RefreshToken)
            .HasForeignKey<RefreshTokenEntity>(x => x.ParticipantId);
    }
}