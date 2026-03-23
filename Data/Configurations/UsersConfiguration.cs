using AuthDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDemo.Data.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Uid).IsRequired();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        }
    }
}
