
namespace Social.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }

    public Guid UserId { get; private set; }
    public string ImageUrl { get; private set; }
    public string? Caption { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Post() { } // EF Core

    public Post(Guid userId, string imageUrl, string? caption)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ImageUrl = imageUrl;
        Caption = caption;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string imageUrl, string? caption)
    {
        ImageUrl = imageUrl;
        Caption = caption;
    }
}
