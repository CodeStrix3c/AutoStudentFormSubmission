using ASFS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ASFS.Infrastructure.Data
{
    public class ASFSDbContext : DbContext
    {
        public ASFSDbContext(DbContextOptions<ASFSDbContext> options) : base(options) { }

        public DbSet<FormRequest> FormRequests => Set<FormRequest>();

        // Extra entities added later (attachments, approvals) will also go here:
        public DbSet<FormAttachment> FormAttachments => Set<FormAttachment>();
        public DbSet<Approval> Approvals => Set<Approval>();
        public DbSet<Workflow> Workflows => Set<Workflow>();
        public DbSet<ApprovalStep> ApprovalSteps => Set<ApprovalStep>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormRequest>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.DataJson).IsRequired();
                b.Property(x => x.StudentAadId).HasMaxLength(200);
                b.Property(x => x.CurrentStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50);
                b.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<FormAttachment>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FileName).HasMaxLength(500);
                b.Property(x => x.FilePath).HasMaxLength(2000);
                b.Property(x => x.ContentType).HasMaxLength(200);
                b.HasOne<FormRequest>()
                    .WithMany()
                    .HasForeignKey(x => x.FormId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Approval>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Decision)
                    .HasConversion<string>()
                    .HasMaxLength(50);
                b.HasOne<FormRequest>()
                    .WithMany()
                    .HasForeignKey(x => x.FormId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Workflow>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ApprovalStep>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.ApproverRole).HasMaxLength(100).IsRequired();
                b.HasOne<Workflow>()
                    .WithMany(w => w.Steps)
                    .HasForeignKey(x => x.WorkflowId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
