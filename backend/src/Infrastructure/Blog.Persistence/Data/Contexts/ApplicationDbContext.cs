using System.Reflection;
using Blog.Application.Common.Interfaces;
using Blog.Domain.Common;
using Blog.Domain.Entities;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<PostCategories> PostCategories => Set<PostCategories>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<PostTag> PostTags => Set<PostTag>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<PostMeta> PostMetas => Set<PostMeta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>(p =>
            {
                p.ToTable("Posts");
                p.HasKey(p => p.Id);
                p.Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
                p.Property(e => e.UpdatedDate).HasDefaultValueSql("getutcdate()");
                p.Property(e => e.Title).IsRequired().HasMaxLength(250);
                p.Property(e => e.Slug).IsRequired().HasMaxLength(250);
                p.HasOne(u => u.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(u => u.UserId)
                    .HasConstraintName("FK_Posts_Users")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>(p =>
            {
                p.ToTable("Comments");
                p.HasKey(x => x.Id);
                p.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .HasConstraintName("FK_Comments_Users")
                    .OnDelete(DeleteBehavior.NoAction);

                p.HasOne(c => c.ParentComment)
                    .WithMany(pc => pc.ChildComments)
                    .HasForeignKey(c => c.ParentCommentId)
                    .IsRequired(false)
                    .HasConstraintName("FK_CommentParent_CommentChild")
                    .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(c => c.ParentPost)
                    .WithMany(pc => pc.Comments)
                    .HasForeignKey(c => c.PostId)
                    .IsRequired(false)
                    .HasConstraintName("FK_Comments_Posts")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostMeta>(p =>
            {
                p.ToTable("PostMetas");
                p.HasKey(p => p.Id);
                p.Property(x => x.Key).HasMaxLength(100);
                p.Property(x => x.Content).HasMaxLength(250);
                p.HasOne(e => e.Post)
                        .WithMany(e => e.PostMetas)
                        .HasForeignKey(e => e.PostId)
                        .HasConstraintName("FK_PostMetas_Posts");
            });

            modelBuilder.Entity<Like>(p =>
            {
                p.ToTable("Likes");
                p.HasKey(l => new { l.UserId, l.PostId });
                p.HasOne(e => e.User)
                                    .WithMany(e => e.Likes)
                                    .HasForeignKey(e => e.UserId)
                                    .HasConstraintName("FK_Likes_Users")
                                    .OnDelete(DeleteBehavior.NoAction);

                p.HasOne(e => e.Post)
                                    .WithMany(e => e.Likes)
                                    .HasForeignKey(e => e.PostId)
                                    .HasConstraintName("FK_Likes_Posts")
                                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostTag>(p =>
            {
                p.ToTable("PostTags");
                p.HasKey(pt => new { pt.PostId, pt.TagId });

                p.HasOne(e => e.Post)
                                    .WithMany(e => e.PostTags)
                                    .HasForeignKey(e => e.PostId)
                                    .HasConstraintName("FK_PostTags_Posts")
                                    .OnDelete(DeleteBehavior.Cascade);

                p.HasOne(e => e.Tag)
                                    .WithMany(e => e.PostTags)
                                    .HasForeignKey(e => e.TagId)
                                    .HasConstraintName("FK_PostTags_Tags")
                                    .OnDelete(DeleteBehavior.Cascade);

            });

            // Define unique constraint for Tag Name
            modelBuilder.Entity<Tag>(p =>
            {
                p.ToTable("Tags");
                p.HasIndex(t => t.Title)
                    .IsUnique();
            });

            modelBuilder.Entity<Category>(p =>
            {
                p.ToTable("Categories");
                p.HasKey(p => p.Id);
                p.Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
                p.Property(e => e.UpdatedDate).HasDefaultValueSql("getutcdate()");
                p.Property(e => e.Title).IsRequired().HasMaxLength(150);
                p.Property(e => e.Slug).IsRequired().HasMaxLength(100);
                p.Property(e => e.Description).IsRequired().HasMaxLength(300);
            });

            // PostCategories
            modelBuilder.Entity<PostCategories>(p =>
            {
                p.ToTable("PostCategories");
                p.HasKey(pt => new { pt.PostId, pt.CategoryId });

                p.HasOne(e => e.Post)
                        .WithMany(e => e.PostCategories)
                        .HasForeignKey(e => e.PostId)
                        .HasConstraintName("FK_PostCategories_Posts");

                p.HasOne(e => e.Category)
                        .WithMany(e => e.PostCategories)
                        .HasForeignKey(e => e.CategoryId)
                        .HasConstraintName("FK_PostCategories_Categories");
            });
            // Apply audit configuration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseAuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<string>("CreatedBy").IsRequired();
                modelBuilder.Entity(entityType.ClrType).Property<string>("UpdatedBy");
                modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("CreatedDate").IsRequired();
                modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("UpdatedDate");
            }
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}