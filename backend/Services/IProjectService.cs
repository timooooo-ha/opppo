using WebApiApp.Models;

namespace WebApiApp.Services;

public interface IProjectService {
    Task<ProjectResponseDto> CreateProjectAsync(
        CreateProjectRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    );

    Task<List<ProjectResponseDto>> GetForUserAsync(
        Guid actorId,
        CancellationToken cancellationToken
    );

    Task<ModuleResponseDto> AddModuleAsync(
        Guid projectId,
        AddModuleRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    );
}