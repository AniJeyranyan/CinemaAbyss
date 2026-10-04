using CinemaAbyss.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAbyss.Movies.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("movies");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(m => m.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
        builder.Property(m => m.Description).HasColumnName("description");
        builder.Property(m => m.Rating).HasColumnName("rating").HasColumnType("numeric(3,1)");
        builder.Property(m => m.Genres).HasColumnName("genres").HasColumnType("text[]");
    }
}
