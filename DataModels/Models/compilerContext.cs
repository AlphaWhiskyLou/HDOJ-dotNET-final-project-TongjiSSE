using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataModels.Models
{
    public partial class compilerContext : DbContext
    {
        public compilerContext()
        {
        }

        public compilerContext(DbContextOptions<compilerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Code> Codes { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Reference> References { get; set; } = null!;
        public virtual DbSet<Sample> Samples { get; set; } = null!;
        public virtual DbSet<Solo> Solos { get; set; } = null!;
        public virtual DbSet<Solution> Solutions { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=;uid=;pwd=;port=;database=", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Code>(entity =>
            {
                entity.ToTable("code");

                entity.Property(e => e.CodeId).HasColumnName("code_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.Language)
                    .HasMaxLength(255)
                    .HasColumnName("language");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.SubmitTime).HasColumnName("submit_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentTime).HasColumnName("comment_time");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.SolutionId).HasColumnName("solution_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.ArgsType)
                    .HasMaxLength(255)
                    .HasColumnName("args_type");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Difficulty)
                    .HasMaxLength(255)
                    .HasColumnName("difficulty");

                entity.Property(e => e.Labels)
                    .HasMaxLength(255)
                    .HasColumnName("labels");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Pass).HasColumnName("pass");

                entity.Property(e => e.ReturnType)
                    .HasMaxLength(255)
                    .HasColumnName("return_type");

                entity.Property(e => e.Submission).HasColumnName("submission");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.ToTable("reference");

                entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

                entity.Property(e => e.Introduction)
                    .HasMaxLength(255)
                    .HasColumnName("introduction");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.ToTable("sample");

                entity.Property(e => e.SampleId).HasColumnName("sample_id");

                entity.Property(e => e.Input)
                    .HasMaxLength(255)
                    .HasColumnName("input");

                entity.Property(e => e.IsTest).HasColumnName("is_test");

                entity.Property(e => e.Output)
                    .HasMaxLength(255)
                    .HasColumnName("output");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");
            });

            modelBuilder.Entity<Solo>(entity =>
            {
                entity.ToTable("solo");

                entity.Property(e => e.SoloId).HasColumnName("solo_id");

                entity.Property(e => e.PassCount).HasColumnName("pass_count");

                entity.Property(e => e.Ranking).HasColumnName("ranking");

                entity.Property(e => e.TotalCount).HasColumnName("total_count");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Solution>(entity =>
            {
                entity.ToTable("solution");

                entity.Property(e => e.SolutionId).HasColumnName("solution_id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedTime).HasColumnName("created_time");

                entity.Property(e => e.Language)
                    .HasMaxLength(255)
                    .HasColumnName("language");

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.ToTable("submission");

                entity.Property(e => e.SubmissionId)
                    .HasMaxLength(225)
                    .HasColumnName("submission_id");

                entity.Property(e => e.Language)
                    .HasMaxLength(50)
                    .HasColumnName("language");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Result)
                    .HasMaxLength(50)
                    .HasColumnName("result");

                entity.Property(e => e.SubmissionTime)
                    .HasColumnType("datetime")
                    .HasColumnName("submission_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Avatar)
                    .HasColumnType("blob")
                    .HasColumnName("avatar");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Profile)
                    .HasMaxLength(255)
                    .HasColumnName("profile");

                entity.Property(e => e.SubmissionList)
                    .HasMaxLength(255)
                    .HasColumnName("submission_list");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.University)
                    .HasMaxLength(255)
                    .HasColumnName("university");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
