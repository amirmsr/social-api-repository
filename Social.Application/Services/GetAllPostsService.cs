using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Application.DTOs;
using Social.Application.Mappings;

namespace Social.Application.Services;

public class GetAllPostsService
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostDto>> ExecuteAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        return posts.Select(p => p.ToDto());
    }

}
