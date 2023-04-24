using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain;
using Proto1.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace Proto1.Persistence.Context;

public class ProtoContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public ProtoContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas {get;set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRole>(userRole => {
            userRole.HasKey(ur => new {ur.UserId, ur.RoleId});
            userRole.HasOne(ur => ur.Role).WithMany(u => u.UserRoles).HasForeignKey(u => u.RoleId).IsRequired();
            userRole.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(u => u.UserId).IsRequired();
        });
    }
}
