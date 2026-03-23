using AuthDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDemo.Data.Configurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(c => new { c.ParentId, c.Name })
            .IsUnique()
            .HasFilter("[ParentId] IS NOT NULL");

            // Unique for root categories
            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasFilter("[ParentId] IS NULL");

            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
