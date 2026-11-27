using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiApp.Exceptions;
using WebApiApp.Models;
using WebApiApp.Services;

namespace WebApiApp.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectMemberController : ControllerBase {
    private readonly IProjectMemberService _projectMemberService;

    public ProjectMemberController(IProjectMemberService projectMemberService) {
        _projectMemberService = projectMemberService;
    }

    [HttpPost("{projectId:guid}/members")]
    public async Task<ActionResult<MemberResponseDto>> Add(
        [FromRoute] Guid projectId,
        [FromBody] AddMemberRequestDto request,
        CancellationToken cancellationToken
    ) {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var actorId)) {
            return Unauthorized();
        }

        try {
            var response = await _projectMemberService.AddMemberAsync(
                projectId,
                request,
                actorId,
                cancellationToken
            );

            return StatusCode(StatusCodes.Status201Created, response);

        }
        catch (ProjectNotFoundException) {
            return NotFound();
        }
        catch (ProjectAccessDeniedException) {
            return Forbid();
        }
        catch (ProjectMemberValidationException exception) {
            return BadRequest(exception.Errors);
        }
        catch (ProjectConflictException exception) {
            return Conflict(exception.Errors);
        }
        catch (UserAccountConflictException exception) {
            return Conflict(exception.Errors);
        }

    }
}