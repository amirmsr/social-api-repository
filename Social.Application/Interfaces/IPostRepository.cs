using Social.Domain.Entities;

namespace Social.Application.Interfaces;

public interface IPostRepository
{
    Task AddAsync(Post post);
    Task<Post?> GetByIdAsync(Guid id);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId);

    Task UpdateAsync(Post post);
    Task DeleteAsync(Guid id);
}
