namespace Social.Application.DTOs;

public record PostDto(
    Guid Id,
    Guid UserId,
    string? Caption,
    string ImageUrl,
    DateTime CreatedAt
);
