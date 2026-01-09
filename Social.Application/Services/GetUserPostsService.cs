using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Application.DTOs;
using Social.Application.Mappings;

namespace Social.Application.Services;

public class GetUserPostsService
{
    private readonly IPostRepository _postRepository;

    public GetUserPostsService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostDto>> ExecuteAsync(Guid userId)
    {
        var posts = await _postRepository.GetByUserIdAsync(userId);
        return posts.Select(p => p.ToDto());
    }
}


