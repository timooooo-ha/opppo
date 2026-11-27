using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Data.Entities;
using WebApiApp.Exceptions;
using WebApiApp.Models;

namespace WebApiApp.Services;

public class ProjectMemberService : IProjectMemberService {
    private readonly AppDbContext _context;
    private readonly ProjectAccessService _projectAccessService;
    private readonly UserAccountService _userAccountService;
    private readonly INotificationsWriter _notificationsWriter;

    public ProjectMemberService(
       AppDbContext context,
       ProjectAccessService projectAccessService,
       UserAccountService userAccountService,
       INotificationsWriter notificationsWriter
    ) {
        _context = context;
        _projectAccessService = projectAccessService;
        _userAccountService = userAccountService;
        _notificationsWriter = notificationsWriter;
    }


    public async Task<MemberResponseDto> AddMemberAsync(
        Guid projectId,
        AddMemberRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    ) {
        if (request.Role != ProjectRole.DEVELOPER &&
        request.Role != ProjectRole.TESTER) {
            throw new ProjectMemberValidationException(new Dictionary<string, string> {
                ["role"] = "must be DEVELOPER or TESTER"
            });
        }

        var role = request.Role.Value;

        await _projectAccessService.EnsureRoleAsync(
            actorId,
            projectId,
            ProjectRole.LEAD,
            cancellationToken
        );

        var project = await _context.Projects
            .AsNoTracking()
            .SingleAsync(project => project.Id == projectId, cancellationToken);

        var email = request.Email.Trim().ToLowerInvariant();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var user = await _context.Users
            .SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

        string? temporaryPassword = null;

        if (user is null) {

            var newUserResult = await _userAccountService.CreateUserAsync(
                request.FullName,
                email,
                cancellationToken
            );

            user = newUserResult.User;
            temporaryPassword = newUserResult.TemporaryPassword;
        }
        else {
            var membershipExists = await _context.ProjectMembers
                .AsNoTracking()
                .AnyAsync(
                    member => member.ProjectId == projectId && member.UserId == user.Id,
                    cancellationToken
                );

            if (membershipExists) {
                throw new ProjectConflictException(new Dictionary<string, string> {
                    ["email"] = "this user is already a member of the project"
                });
            }
        }

        var now = DateTime.UtcNow;
        var member = new ProjectMemberEntity {
            ProjectId = projectId,
            UserId = user.Id,
            Role = role,
            AddedAt = now
        };

        _context.ProjectMembers.Add(member);

        _notificationsWriter.Add(
            recipientId: user.Id,
            eventType: EventType.ADDED_TO_PROJECT,
            text: $"Вы добавлены в проект '{project.Name}' с ролью: {GetRoleDisplayName(role)}",
            createdAt: now,
            projectId: projectId
        );

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new MemberResponseDto {
            Id = member.Id,
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Role = member.Role,
            AddeddAt = member.AddedAt,
            MustChangePassword = user.MustChangePassword,
            TemporaryPassword = temporaryPassword
        };

    }


    private static string GetRoleDisplayName(ProjectRole role) {
        return role switch {
            ProjectRole.DEVELOPER => "Разработчик",
            ProjectRole.TESTER => "Тестировщик",
            _ => throw new ArgumentOutOfRangeException(nameof(role))
        };
    }
}