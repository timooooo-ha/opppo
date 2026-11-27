using WebApiApp.Models;

namespace WebApiApp.Services;

public interface IProjectMemberService {
    Task<MemberResponseDto> AddMemberAsync(
        Guid projectId,
        AddMemberRequestDto request,
        Guid actorId,
        CancellationToken cancellationToken
    );
}