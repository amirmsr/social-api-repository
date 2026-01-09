using Social.Application.DTOs;
using Social.Domain.Entities;

namespace Social.Application.Mappings;

public static class PostMapping
{
    public static PostDto ToDto(this Post post)
    {
        return new PostDto(
            post.Id,
            post.UserId,
            post.Caption,
            post.ImageUrl,
            post.CreatedAt
        );
    }
}
