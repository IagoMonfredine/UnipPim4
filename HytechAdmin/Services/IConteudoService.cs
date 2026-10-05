using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>
    /// Contrato do serviço de conteúdos (textos explicativos e questões) usado pelo Admin.
    /// Ver observação em <see cref="IUsuarioService"/> sobre a futura troca por API/BD.
    /// </summary>
    public interface IConteudoService
    {
        List<ConteudoItem> ObterTodos();
        ConteudoItem? ObterPorId(string id);
        void Remover(string id);
    }
}
