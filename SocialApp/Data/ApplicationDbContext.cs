﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialApp.Models;

namespace SocialApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SocialApp.Models.UserProfile> UserProfile { get; set; } = default!;
        public DbSet<SocialApp.Models.Post> Post { get; set; } = default!;
        public DbSet<SocialApp.Models.Comment> Comment { get; set; } = default!;
        public DbSet<SocialApp.Models.Story> Story { get; set; } = default!;

        public DbSet<SocialApp.Models.Follow> Follow { get; set; } = default!;
        public DbSet<SocialApp.Models.Like> Like { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between UserProfile and Post
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Posts)
                .WithOne(p => p.AuthorProfile)
                .HasForeignKey(p => p.AuthorProfileId)
                .OnDelete(DeleteBehavior.Restrict);  // Optional: configure cascade behavior

            modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserName)  // Define index on Email
            .IsUnique();              // Make the index unique

            modelBuilder.Entity<Follow>()
           .HasOne(f => f.Follower)
           .WithMany(up => up.Following)
           .HasForeignKey(f => f.FollowerId)
           .OnDelete(DeleteBehavior.Restrict);  // Adjust as necessary

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(up => up.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);  // Adjust as necessary

        }
    }
}
