using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Data.Entities;
using WebApiApp.Exceptions;

namespace WebApiApp.Services;

public class ProjectAccessService {
    private readonly AppDbContext _context;

    public ProjectAccessService(
        AppDbContext context
    ) {
        _context = context;
    }

    public async Task<ProjectMemberEntity?> GetMembershipAsync(
        Guid actorId,
        Guid projectId,
        CancellationToken cancellationToken
    ) {
        return await _context.ProjectMembers
            .AsNoTracking()
            .SingleOrDefaultAsync(
                member => member.ProjectId == projectId && member.UserId == actorId,
                cancellationToken
            );
    }

    public async Task<ProjectMemberEntity> EnsureMemberAsync(
        Guid actorId,
        Guid projectId,
        CancellationToken cancellationToken
    ) {
        var projectExists = await _context.Projects
            .AsNoTracking()
            .AnyAsync(project => project.Id == projectId, cancellationToken);

        if (!projectExists) {
            throw new ProjectNotFoundException();
        }

        var membership = await GetMembershipAsync(
            actorId,
            projectId,
            cancellationToken
        );

        if (membership is null) {
            throw new ProjectAccessDeniedException();
        }

        return membership;
    }

    public async Task<ProjectMemberEntity> EnsureRoleAsync(
        Guid actorId,
        Guid projectId,
        ProjectRole role,
        CancellationToken cancellationToken
    ) {
        var membership = await EnsureMemberAsync(
            actorId,
            projectId,
            cancellationToken
        );

        if (membership.Role != role) {
            throw new ProjectAccessDeniedException();
        }

        return membership;
    }

}