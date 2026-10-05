using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>
    /// Contrato do serviço de usuários usado pelas telas do Admin.
    /// Hoje existe apenas a implementação em memória (<see cref="UsuarioServiceMock"/>).
    /// Quando a API/BD estiver disponível, crie uma "UsuarioServiceApi : IUsuarioService"
    /// (por ex. usando HttpClient) e troque a instância no MainForm — as telas não mudam.
    /// </summary>
    public interface IUsuarioService
    {
        List<Usuario> ObterTodos();
        Usuario? ObterPorId(int id);
        void Adicionar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void Remover(int id);
        void AlternarStatus(int id);
    }
}
