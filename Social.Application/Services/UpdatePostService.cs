using Social.Application.Interfaces;

namespace Social.Application.Services;

public class UpdatePostService
{
    private readonly IPostRepository _postRepository;

    public UpdatePostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<bool> ExecuteAsync(Guid postId, Guid userId, string imageUrl, string? caption)
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

        // Mettre à jour les propriétés via la méthode du domaine
        post.Update(imageUrl, caption);

        // Sauvegarder les modifications
        await _postRepository.UpdateAsync(post);

        return true;
    }
}