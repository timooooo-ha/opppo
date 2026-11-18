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
    public DbSet<ReleaseEntity> Releases => Set<ReleaseEntity>();
    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<CommentEntity> Comments => Set<CommentEntity>();
    public DbSet<AttachmentEntity> Attachments => Set<AttachmentEntity>();
    public DbSet<HistoryEntryEntity> HistoryEntries => Set<HistoryEntryEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<ModuleEntity>()
            .HasOne(module => module.Project)
            .WithMany(project => project.Modules)
            .HasForeignKey(module => module.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReleaseEntity>()
            .HasOne(release => release.Project)
            .WithMany(project => project.Releases)
            .HasForeignKey(release => release.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

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
            .HasOne(ticket => ticket.FixedIn)
            .WithMany(release => release.FixedTickets)
            .HasForeignKey(ticket => ticket.FixedInId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TicketEntity>()
            .HasOne(ticket => ticket.DuplicateOf)
            .WithMany(ticket => ticket.Duplicates)
            .HasForeignKey(ticket => ticket.DuplicateOfId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CommentEntity>()
            .HasOne(comment => comment.Ticket)
            .WithMany(ticket => ticket.Comments)
            .HasForeignKey(comment => comment.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommentEntity>()
            .HasOne(comment => comment.Author)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AttachmentEntity>()
            .HasOne(attachment => attachment.Ticket)
            .WithMany(ticket => ticket.Attachments)
            .HasForeignKey(attachment => attachment.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistoryEntryEntity>()
            .HasOne(entry => entry.Ticket)
            .WithMany(ticket => ticket.History)
            .HasForeignKey(entry => entry.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<HistoryEntryEntity>()
            .HasOne(entry => entry.ChangedBy)
            .WithMany(user => user.HistoryEntries)
            .HasForeignKey(entry => entry.ChangedById)
            .OnDelete(DeleteBehavior.Restrict);

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
    }
}
