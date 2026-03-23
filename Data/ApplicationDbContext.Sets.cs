using AuthDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Data
{
    public partial class ApplicationDbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<PostTags> PostTags => Set<PostTags>();
    }
}
