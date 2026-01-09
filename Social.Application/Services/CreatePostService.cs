using Social.Application.Interfaces;
using Social.Domain.Entities;

namespace Social.Application.Services;

public class CreatePostService
{
    private readonly IPostRepository _postRepository;

    public CreatePostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Guid> ExecuteAsync(Guid userId, string imageUrl, string? caption)
    {
        var post = new Post(userId, imageUrl, caption);

        await _postRepository.AddAsync(post);

        return post.Id;
    }
}
