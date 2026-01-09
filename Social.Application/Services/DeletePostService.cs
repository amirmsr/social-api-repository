using Social.Application.Interfaces;

namespace Social.Application.Services;

public class DeletePostService
{
    private readonly IPostRepository _postRepository;

    public DeletePostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<bool> ExecuteAsync(Guid postId, Guid userId)
    {
        // Récupérer le post existant
        var post = await _postRepository.GetByIdAsync(postId);

        // Vérifier que le post existe
        if (post == null)
        {
            return false;
        }

        // Vérifier que l'utilisateur est bien le propriétaire du post
        if (post.UserId != userId)
        {
            return false;
        }

        // Supprimer le post
        await _postRepository.DeleteAsync(postId);

        return true;
    }
}