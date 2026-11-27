using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Data.Entities;
using WebApiApp.Exceptions;
using WebApiApp.Models;

namespace WebApiApp.Services;

public class ProjectService : IProjectService {
    private readonly AppDbContext _context;
    private readonly ProjectAccessService _projectAccessService;

    public ProjectService(
        AppDbContext context,
        ProjectAccessService projectAccessService
    ) {
        _context = context;
        _projectAccessService = projectAccessService;
    }

    public async Task<ProjectResponseDto> CreateProjectAsync(
        CreateProjectRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    ) {
        var name = request.Name.Trim();

        var projectNameExists = await _context.Projects
            .AsNoTracking()
            .AnyAsync(project => project.Name == name, cancellationToken);

        if (projectNameExists) {
            throw new ProjectConflictException(new Dictionary<string, string> {
                ["name"] = "project with this name already exists"
            });
        }

        var now = DateTime.UtcNow;
        var project = new ProjectEntity {
            Name = name,
            Description = NormalizeOptionalText(request.Description),
            RepositoryUrl = request.RepositoryUrl.Trim(),
            RegisteredAt = now,
            NextTicketNumber = 1
        };

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        _context.Projects.Add(project);
        _context.ProjectMembers.Add(new ProjectMemberEntity {
            Project = project,
            UserId = actorId,
            Role = ProjectRole.LEAD,
            AddedAt = now
        });

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return MapToResponse(project, ProjectRole.LEAD);
    }

    public async Task<List<ProjectResponseDto>> GetForUserAsync(
        Guid actorId,
        CancellationToken cancellationToken
    ) {
        return await _context.ProjectMembers
            .AsNoTracking()
            .Where(member => member.UserId == actorId)
            .OrderBy(member => member.Project.Name)
            .ThenBy(member => member.Project.Id)
            .Select(member => new ProjectResponseDto {
                Id = member.Project.Id,
                Name = member.Project.Name,
                Description = member.Project.Description,
                RepositoryUrl = member.Project.RepositoryUrl,
                RegisteredAt = member.Project.RegisteredAt,
                NextTicketNumber = member.Project.NextTicketNumber,
                CurrentUserRole = member.Role
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ModuleResponseDto> AddModuleAsync(
        Guid projectId,
        AddModuleRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    ) {
        await _projectAccessService.EnsureRoleAsync(
            actorId,
            projectId,
            ProjectRole.LEAD,
            cancellationToken
        );

        var name = request.Name.Trim();
        var moduleNameExists = await _context.Modules
            .AsNoTracking()
            .AnyAsync(module => module.ProjectId == projectId && module.Name == name, cancellationToken);

        if (moduleNameExists) {
            throw new ProjectConflictException(new Dictionary<string, string> {
                ["name"] = "module with this name already exists in this project"
            });
        }

        var module = new ModuleEntity {
            ProjectId = projectId,
            Name = name,
            Description = NormalizeOptionalText(request.Description)
        };

        _context.Modules.Add(module);
        await _context.SaveChangesAsync(cancellationToken);

        return new ModuleResponseDto {
            Id = module.Id,
            ProjectId = module.ProjectId,
            Name = module.Name,
            Description = module.Description
        };

    }

    private static ProjectResponseDto MapToResponse(
        ProjectEntity project,
        ProjectRole currentRole
    ) {
        return new ProjectResponseDto {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            RepositoryUrl = project.RepositoryUrl,
            RegisteredAt = project.RegisteredAt,
            NextTicketNumber = project.NextTicketNumber,
            CurrentUserRole = currentRole
        };
    }

    private static string? NormalizeOptionalText(string? value) {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}