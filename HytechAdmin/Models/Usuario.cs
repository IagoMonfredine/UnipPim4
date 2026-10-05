namespace HytechAdmin.Models
{
    /// <summary>
    /// Representa um usuário da plataforma (aluno, professor ou administrador).
    /// Estrutura pensada para mapear 1:1 com a futura tabela "Usuarios" do banco de dados.
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>aluno | professor | administrador</summary>
        public string Perfil { get; set; } = "aluno";

        /// <summary>Só é relevante quando Perfil == "professor".</summary>
        public string Especialidade { get; set; } = string.Empty;

        /// <summary>ativo | inativo</summary>
        public string Status { get; set; } = "ativo";

        public string Cadastro { get; set; } = string.Empty;

        public bool EhProfessor => Perfil == "professor";
        public bool EhAtivo => Status == "ativo";
    }
}
