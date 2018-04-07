using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspTodo.Models;

namespace AspTodo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Sharing> Sharings { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Primary keys
            builder.Entity<TodoList>().HasKey(tl => tl.ListID);
            builder.Entity<TodoItem>().HasKey(ti => ti.ItemID);
            builder.Entity<Sharing>().HasKey(s => new { s.ListID, s.ShareeID });
            builder.Entity<Invitation>().HasKey(i => new { i.SenderID, i.ReceiverID, i.ListID });

            // Unique indices
            builder.Entity<TodoItem>()
                .HasIndex(ti => new { ti.ListID, ti.ItemOrder })
                .IsUnique();

            // Foreign Key constraints
            builder.Entity<TodoList>()
                .HasOne(tl => tl.Owner).WithMany(u => u.OwnedLists)
                .HasForeignKey(tl => tl.OwnerID); // will cascade delete
            builder.Entity<TodoItem>()
                .HasOne(ti => ti.TodoList).WithMany(tl => tl.TodoItems)
                .HasForeignKey(ti => ti.ListID); // will cascade delete

            builder.Entity<Sharing>()
                .HasOne(s => s.TodoList).WithMany(tl => tl.Sharings)
                .HasForeignKey(s => s.ListID); // will cascade delete
            builder.Entity<Sharing>()
                .HasOne(s => s.Sharee).WithMany(u => u.Sharings)
                .HasForeignKey(s => s.ShareeID); // will cascade delete

            builder.Entity<Invitation>()
                .HasOne(i => i.Sender).WithMany(u => u.SentInvitations)
                .HasForeignKey(i => i.SenderID); // will cascade delete
            builder.Entity<Invitation>()
                .HasOne(i => i.Receiver).WithMany(u => u.ReceivedInvitations)
                .HasForeignKey(i => i.ReceiverID); // will cascade delete
            builder.Entity<Invitation>()
                .HasOne(i => i.TodoList).WithMany(tl => tl.Invitations)
                .HasForeignKey(i => i.ListID); // will cascade delete

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
