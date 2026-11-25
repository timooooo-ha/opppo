using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RepositoryUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextTicketNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    MustChangePassword = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_modules_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_members_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DuplicateOfId = table.Column<Guid>(type: "uuid", nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReproductionSteps = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false, defaultValue: "MEDIUM"),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "NEW"),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlanJustification = table.Column<string>(type: "text", nullable: true),
                    FixLink = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    FixedInVersion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tickets_modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_tickets_DuplicateOfId",
                        column: x => x.DuplicateOfId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    UploadedById = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attachments_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attachments_users_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comments_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "history_entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uuid", nullable: false),
                    Field = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_history_entries_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_history_entries_users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                    table.CheckConstraint("CK_notifications_single_target", "(\"TicketId\" IS NOT NULL) <> (\"ProjectId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_notifications_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_attachments_TicketId",
                table: "attachments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_UploadedById",
                table: "attachments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_comments_AuthorId",
                table: "comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_TicketId",
                table: "comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_history_entries_ChangedById",
                table: "history_entries",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_history_entries_TicketId",
                table: "history_entries",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_modules_ProjectId_Name",
                table: "modules",
                columns: new[] { "ProjectId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_ProjectId",
                table: "notifications",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_RecipientId",
                table: "notifications",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_TicketId",
                table: "notifications",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_project_members_ProjectId",
                table: "project_members",
                column: "ProjectId",
                unique: true,
                filter: "\"Role\" = 'LEAD'");

            migrationBuilder.CreateIndex(
                name: "IX_project_members_ProjectId_UserId",
                table: "project_members",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_members_UserId",
                table: "project_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_projects_Name",
                table: "projects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_AssigneeId",
                table: "tickets",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_AuthorId",
                table: "tickets",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_DuplicateOfId",
                table: "tickets",
                column: "DuplicateOfId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ModuleId",
                table: "tickets",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ProjectId_Number",
                table: "tickets",
                columns: new[] { "ProjectId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "history_entries");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "project_members");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
