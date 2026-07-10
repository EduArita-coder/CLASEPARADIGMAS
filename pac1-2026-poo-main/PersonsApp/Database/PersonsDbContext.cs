using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonsApp.Entities;

namespace PersonsApp.Database
{
    public class PersonsDbContext : IdentityDbContext<UserEntity, RoleEntity, string>
    {
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetIdentityTableNames(builder);
        }

        private static void SetIdentityTableNames(ModelBuilder builder)
        {
            builder.Entity<UserEntity>().ToTable("users");
            builder.Entity<RoleEntity>().ToTable("roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("users_roles")
                .HasKey(ur => new {ur.UserId, ur.RoleId});
            builder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("users_login");
            builder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");
        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
    }
}