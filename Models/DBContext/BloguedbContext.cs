using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Projet_RedSocial_app.Models.DBContext;

public partial class BloguedbContext : DbContext
{
    public BloguedbContext()
    {
    }

    public BloguedbContext(DbContextOptions<BloguedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blogue> Blogues { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentDislike> CommentDislikes { get; set; }

    public virtual DbSet<CommentLike> CommentLikes { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostDislike> PostDislikes { get; set; }

    public virtual DbSet<PostLike> PostLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blogue>(entity =>
        {
            entity.HasKey(e => e.BlogueId).HasName("PK__Blogue__EBBC3655F649A1BC");

            entity.ToTable("Blogue");

            entity.Property(e => e.BlogueId).HasColumnName("Blogue_ID");
            entity.Property(e => e.Categorie)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Member).WithMany(p => p.Blogues)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade) // Suppression en cascade
                .HasConstraintName("FK__Blogue__Member_I__398D8EEE");
                
            // Configurer la relation avec Posts et appliquer la suppression en cascade
            entity.HasMany(b => b.Posts) // Un Blogue a plusieurs Posts
                .WithOne() // Relation un à plusieurs (One-to-many)
                .HasForeignKey(p => p.BlogueId) // Clé étrangère pour Post
                .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade des Posts associés
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__99FC143BA80BDC1D");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("Comment_ID");
            entity.Property(e => e.CommentUrl)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Comment_URL");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.PublishingDate).HasColumnType("date");

            entity.HasOne(d => d.Member).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comment__Member___412EB0B6");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comment__Post_ID__403A8C7D");
        });

        modelBuilder.Entity<CommentDislike>(entity =>
        {
            entity.HasKey(e => e.DisLikeId).HasName("PK__CommentD__FD8188E53F0EB2F0");

            entity.ToTable("CommentDislike");

            entity.Property(e => e.DisLikeId).HasColumnName("DisLike_ID");
            entity.Property(e => e.CommentId).HasColumnName("Comment_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentDislikes)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__CommentDi__Membe__47DBAE45");

            entity.HasOne(d => d.Member).WithMany(p => p.CommentDislikes)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__CommentDi__Membe__48CFD27E");
        });

        modelBuilder.Entity<CommentLike>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK__CommentL__A40EE7671E9F3ACD");

            entity.ToTable("CommentLike");

            entity.Property(e => e.LikeId).HasColumnName("Like_ID");
            entity.Property(e => e.CommentId).HasColumnName("Comment_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentLikes)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__CommentLi__Membe__440B1D61");

            entity.HasOne(d => d.Member).WithMany(p => p.CommentLikes)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__CommentLi__Membe__44FF419A");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Member__42A68F27E75A3127");

            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsOnline).HasDefaultValueSql("((0))");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Configuration des relations et suppression en cascade
            entity.HasMany(m => m.PostLikes)
                  .WithOne(pl => pl.Member)  // Assurez-vous que PostLike a une propriété "Member" (clé étrangère)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.PostDislikes)
                  .WithOne(pd => pd.Member)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.Comments)
                  .WithOne(c => c.Member)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.CommentDislikes)
                  .WithOne(cd => cd.Member)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.CommentLikes)
                  .WithOne(cl => cl.Member)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__5875F74DFF133AF6");

            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.BlogueId).HasColumnName("Blogue_ID");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.PostUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Post_URL");
            entity.Property(e => e.PublishingDate).HasColumnType("date");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Blogue).WithMany(p => p.Posts)
               .HasForeignKey(d => d.BlogueId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK__Post__Blogue_ID__3C69FB99");

            entity.HasOne(d => d.Member).WithMany(p => p.Posts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Post__Member_ID__3D5E1FD2");
        });

        modelBuilder.Entity<PostDislike>(entity =>
        {
            entity.HasKey(e => e.DisLikeId).HasName("PK__PostDisl__FD8188E5B3AF9ED6");

            entity.ToTable("PostDislike");

            entity.Property(e => e.DisLikeId).HasColumnName("DisLike_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.PostId).HasColumnName("Post_ID");

            entity.HasOne(d => d.Member).WithMany(p => p.PostDislikes)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PostDisli__Membe__59FA5E80");

            entity.HasOne(d => d.Post).WithMany(p => p.PostDislikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PostDisli__Membe__59063A47");
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK__PostLike__A40EE767507DA327");

            entity.ToTable("PostLike");

            entity.Property(e => e.LikeId).HasColumnName("Like_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.PostId).HasColumnName("Post_ID");

            entity.HasOne(d => d.Member).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PostLike__Member__5629CD9C");

            entity.HasOne(d => d.Post).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PostLike__Member__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
