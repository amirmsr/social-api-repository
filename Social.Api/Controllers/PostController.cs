using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Social.Application.Services;
using Microsoft.Extensions.Logging;

namespace Social.Api.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly CreatePostService _createPostService;
    private readonly GetAllPostsService _getAllPostsService;
    private readonly GetUserPostsService _getUserPostsService;
    private readonly UpdatePostService _updatePostService;
    private readonly DeletePostService _deletePostService;
    private readonly ILogger<PostController> _logger;

    

    public PostController(
        CreatePostService createPostService,
        GetAllPostsService getAllPostsService,
        GetUserPostsService getUserPostsService,
        UpdatePostService updatePostService,
        DeletePostService deletePostService,
        ILogger<PostController> logger)
    {
        _createPostService = createPostService;
        _getAllPostsService = getAllPostsService;
        _getUserPostsService = getUserPostsService;
        _updatePostService = updatePostService;
        _deletePostService = deletePostService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var UserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        var postId = await _createPostService.ExecuteAsync(
            UserId,
            request.ImageUrl,
            request.Caption
        );


        return StatusCode(201, new { id = postId, message = "Post créé avec succès pour user" + UserId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _getAllPostsService.ExecuteAsync();
        return Ok(posts);
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserPosts(Guid userId)
    {
        var posts = await _getUserPostsService.ExecuteAsync(userId);
        return Ok(posts);

    }
    
    [Authorize]
    [HttpPut("{postId:guid}")]
    public async Task<IActionResult> Update(Guid postId, [FromBody] UpdatePostRequest request)
    {
        // Récupérer l'ID de l'utilisateur connecté
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        _logger.LogInformation("Verifying update for PostId: {PostId} by UserId: {UserId}", postId, currentUserId);
        // Appeler le service
        var success = await _updatePostService.ExecuteAsync(
            postId,
            currentUserId,
            request.ImageUrl,
            request.Caption
        );

        if (!success)
        {
            return NotFound("Post non trouvé ou vous n'êtes pas autorisé à le modifier");
        }

        return Ok(new { message = "Post mis à jour avec succès" });
    }

    [Authorize]
    [HttpDelete("{postId:guid}")]
    public async Task<IActionResult> Delete(Guid postId)
    {
        // Récupérer l'ID de l'utilisateur connecté
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        // Appeler le service
        var success = await _deletePostService.ExecuteAsync(postId, currentUserId);

        if (!success)
        {
            return NotFound("Post non trouvé ou vous n'êtes pas autorisé à le supprimer");
        }

        return Ok(new { message = "Post supprimé avec succès" });
    }

}
public record CreatePostRequest(
    string ImageUrl,
    string? Caption
);

public record UpdatePostRequest(
    string ImageUrl,
    string? Caption
);
