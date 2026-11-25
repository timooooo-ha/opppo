using Microsoft.EntityFrameworkCore;
using WebApiApp.Data.Entities;

namespace WebApiApp.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ProjectMemberEntity> ProjectMembers => Set<ProjectMemberEntity>();
    public DbSet<ModuleEntity> Modules => Set<ModuleEntity>();
    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<CommentEntity> Comments => Set<CommentEntity>();
    public DbSet<AttachmentEntity> Attachments => Set<AttachmentEntity>();
    public DbSet<HistoryEntryEntity> HistoryEntries => Set<HistoryEntryEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .Property(user => user.FullName)
            .HasMaxLength(150);
        modelBuilder.Entity<UserEntity>()
            .Property(user => user.Email)
            .HasMaxLength(254);
        modelBuilder.Entity<UserEntity>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<ProjectEntity>()
            .Property(project => project.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<ProjectEntity>()
            .Property(project => project.RepositoryUrl)
            .HasMaxLength(500);
        modelBuilder.Entity<ProjectEntity>()
            .HasIndex(project => project.Name)
            .IsUnique();

        modelBuilder.Entity<ProjectMemberEntity>()
            .HasOne(member => member.Project)
            .WithMany(project => project.Members)
            .HasForeignKey(member => member.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProjectMemberEntity>()
            .HasOne(member => member.User)
            .WithMany(user => user.ProjectMemberships)
            .HasForeignKey(member => member.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProjectMemberEntity>()
            .Property(member => member.Role)
            .HasConversion<string>();
        modelBuilder.Entity<ProjectMemberEntity>()
            .HasIndex(member => new { member.ProjectId, member.UserId })
            .IsUnique();
        modelBuilder.Entity<ProjectMemberEntity>()
            .HasIndex(member => member.ProjectId)
            .IsUnique()
            .HasFilter("\"Role\" = 'LEAD'");

        modelBuilder.Entity<ModuleEntity>()
            .HasOne(module => module.Project)
            .WithMany(project => project.Modules)
            .HasForeignKey(module => module.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ModuleEntity>()
            .Property(module => module.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<ModuleEntity>()
            .HasIndex(module => new { module.ProjectId, module.Name })
            .IsUnique();

        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.Project)
            .WithMany(project => project.Tickets)
            .HasForeignKey(ticket => ticket.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.Module)
            .WithMany(module => module.Tickets)
            .HasForeignKey(ticket => ticket.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.Author)
            .WithMany(user => user.AuthoredTickets)
            .HasForeignKey(ticket => ticket.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.Assignee)
            .WithMany(user => user.AssignedTickets)
            .HasForeignKey(ticket => ticket.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.DuplicateOf)
            .WithMany(ticket => ticket.Duplicates)
            .HasForeignKey(ticket => ticket.DuplicateOfId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.Priority)
            .HasConversion<string>()
            .HasDefaultValue(Priority.MEDIUM);
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.Status)
            .HasConversion<string>()
            .HasDefaultValue(TicketStatus.NEW);
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.RejectReason)
            .HasConversion<string>();
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.Title)
            .HasMaxLength(200);
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.FixedInVersion)
            .HasMaxLength(30);
        modelBuilder.Entity<TicketEntity>()
            .Property(ticket => ticket.FixLink)
            .HasMaxLength(1000);
        modelBuilder.Entity<TicketEntity>()
            .HasIndex(ticket => new { ticket.ProjectId, ticket.Number })
            .IsUnique();

        modelBuilder.Entity<CommentEntity>()
            .HasOne(comment => comment.Ticket)
            .WithMany(ticket => ticket.Comments)
            .HasForeignKey(comment => comment.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CommentEntity>()
            .HasOne(comment => comment.Author)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommentEntity>()
            .Property(comment => comment.Kind)
            .HasConversion<string>();
        modelBuilder.Entity<CommentEntity>()
            .Property(comment => comment.Text)
            .HasMaxLength(2000);

        modelBuilder.Entity<AttachmentEntity>()
            .HasOne(attachment => attachment.Ticket)
            .WithMany(ticket => ticket.Attachments)
            .HasForeignKey(attachment => attachment.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<AttachmentEntity>()
            .HasOne(attachment => attachment.UploadedBy)
            .WithMany(user => user.UploadedAttachments)
            .HasForeignKey(attachment => attachment.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AttachmentEntity>()
            .Property(attachment => attachment.FileName)
            .HasMaxLength(255);

        modelBuilder.Entity<HistoryEntryEntity>()
            .HasOne(entry => entry.Ticket)
            .WithMany(ticket => ticket.History)
            .HasForeignKey(entry => entry.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<HistoryEntryEntity>()
            .HasOne(entry => entry.ChangedBy)
            .WithMany(user => user.HistoryEntries)
            .HasForeignKey(entry => entry.ChangedById)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<HistoryEntryEntity>()
            .Property(entry => entry.Field)
            .HasMaxLength(100);


        modelBuilder.Entity<NotificationEntity>()
            .HasOne(notification => notification.Recipient)
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<NotificationEntity>()
            .HasOne(notification => notification.Ticket)
            .WithMany(ticket => ticket.Notifications)
            .HasForeignKey(notification => notification.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<NotificationEntity>()
            .HasOne(notification => notification.Project)
            .WithMany(project => project.Notifications)
            .HasForeignKey(notification => notification.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<NotificationEntity>()
            .Property(notification => notification.Event)
            .HasConversion<string>();
        modelBuilder.Entity<NotificationEntity>()
            .Property(notification => notification.Text)
            .HasMaxLength(300);
        modelBuilder.Entity<NotificationEntity>()
            .Property(notification => notification.IsRead)
            .HasDefaultValue(false);
        modelBuilder.Entity<NotificationEntity>()
            .ToTable(table => table.HasCheckConstraint(
                "CK_notifications_single_target",
                "(\"TicketId\" IS NOT NULL) <> (\"ProjectId\" IS NOT NULL)"
            ));
    }
}
