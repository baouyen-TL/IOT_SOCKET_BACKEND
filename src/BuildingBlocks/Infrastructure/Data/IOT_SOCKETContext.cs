using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Data
{
    public partial class IOT_SOCKETContext : DbContext
    {
        public IOT_SOCKETContext()
        {
        }

        public IOT_SOCKETContext(DbContextOptions<IOT_SOCKETContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerModel> AnswerModels { get; set; }
        public virtual DbSet<BeginGameModel> BeginGameModels { get; set; }
        public virtual DbSet<QuestionModel> QuestionModels { get; set; }
        public virtual DbSet<RemoteModel> RemoteModels { get; set; }
        public virtual DbSet<SaveAnswerModel> SaveAnswerModels { get; set; }
        public virtual DbSet<TopicModel> TopicModels { get; set; }
        public virtual DbSet<UserGameModel> UserGameModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AnswerModel>(entity =>
            {
                entity.HasKey(e => e.AnswerId);

                entity.ToTable("AnswerModel");

                entity.Property(e => e.AnswerId).ValueGeneratedNever();

                entity.Property(e => e.AnswerName).HasMaxLength(2000);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswerModels)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_AnswerModel_QuestionModel");
            });

            modelBuilder.Entity<BeginGameModel>(entity =>
            {
                entity.HasKey(e => e.BeginGameId);

                entity.ToTable("BeginGameModel");

                entity.Property(e => e.BeginGameId).ValueGeneratedNever();

                entity.Property(e => e.ClassName).HasMaxLength(50);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.BeginGameModels)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_BeginGameModel_TopicModel");
            });

            modelBuilder.Entity<QuestionModel>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.ToTable("QuestionModel");

                entity.Property(e => e.QuestionId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.Property(e => e.QuestionName).HasMaxLength(2000);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.QuestionModels)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_QuestionModel_TopicModel");
            });

            modelBuilder.Entity<RemoteModel>(entity =>
            {
                entity.HasKey(e => e.RemoteId);

                entity.ToTable("RemoteModel");

                entity.Property(e => e.RemoteId).ValueGeneratedNever();

                entity.Property(e => e.RemoteName).HasMaxLength(50);
            });

            modelBuilder.Entity<SaveAnswerModel>(entity =>
            {
                entity.HasKey(e => e.SaveAnswerId);

                entity.ToTable("SaveAnswerModel");

                entity.Property(e => e.SaveAnswerId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.SaveAnswerModels)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_SaveAnswerModel_AnswerModel");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.SaveAnswerModels)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_SaveAnswerModel_QuestionModel");
            });

            modelBuilder.Entity<TopicModel>(entity =>
            {
                entity.HasKey(e => e.TopicId);

                entity.ToTable("TopicModel");

                entity.Property(e => e.TopicId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.Property(e => e.TopicName).HasMaxLength(500);
            });

            modelBuilder.Entity<UserGameModel>(entity =>
            {
                entity.HasKey(e => e.UserGameId);

                entity.ToTable("UserGameModel");

                entity.Property(e => e.UserGameId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(250);

                entity.HasOne(d => d.BeginGame)
                    .WithMany(p => p.UserGameModels)
                    .HasForeignKey(d => d.BeginGameId)
                    .HasConstraintName("FK_UserGameModel_BeginGameModel");

                entity.HasOne(d => d.Remote)
                    .WithMany(p => p.UserGameModels)
                    .HasForeignKey(d => d.RemoteId)
                    .HasConstraintName("FK_UserGameModel_RemoteModel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
