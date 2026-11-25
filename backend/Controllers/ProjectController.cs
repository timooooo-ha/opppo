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
public class ProjectController : ControllerBase {
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService) {
        _projectService = projectService;
    }

    [HttpPost()]
    public async Task<ActionResult<ProjectResponseDto>> Create(
        [FromBody] CreateProjectRequestDto request,
        CancellationToken cancellationToken
    ) {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var actorId)) {
            return Unauthorized();
        }

        try {
            var response = await _projectService.CreateProjectAsync(
                request,
                actorId,
                cancellationToken
            );

            return StatusCode(StatusCodes.Status201Created, response);

        }
        catch (ProjectConflictException exception) {
            return Conflict(exception.Errors);
        }

    }

    [HttpGet("my")]
    public async Task<ActionResult<ProjectResponseDto>> GetMine(
        CancellationToken cancellationToken
    ) {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var actorId)) {
            return Unauthorized();
        }

        var response = await _projectService.GetForUserAsync(
            actorId,
            cancellationToken
        );

        return Ok(response);
    }

    [HttpPost("{projectId:guid}/modules")]
    public async Task<ActionResult<ModuleResponseDto>> AddModule(
        [FromRoute] Guid projectId,
        [FromBody] AddModuleRequestDto request,
        CancellationToken cancellationToken
    ) {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var actorId)) {
            return Unauthorized();
        }

        try {
            var response = await _projectService.AddModuleAsync(
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
        catch (ProjectAccesDeniedException) {
            return Forbid();
        }
        catch (ProjectConflictException exception) {
            return Conflict(exception.Errors);
        }

    }
}