using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>
    /// Implementação em memória, apenas para desenvolvimento/demonstração da interface.
    /// Substituir futuramente por uma implementação que consuma a API/BD real.
    /// </summary>
    public class UsuarioServiceMock : IUsuarioService
    {
        private readonly List<Usuario> _usuarios;
        private int _proximoId;

        public UsuarioServiceMock()
        {
            _usuarios = new List<Usuario>
            {
                new() { Id = 1,  Nome = "Pedro Bala",       Email = "pedrobala@gmail.com",        Perfil = "aluno",         Status = "ativo",   Cadastro = "01/05/2026" },
                new() { Id = 2,  Nome = "Prof. Silva",      Email = "professor@hytech.com",       Perfil = "professor",     Status = "ativo",   Cadastro = "15/03/2026", Especialidade = "Programação" },
                new() { Id = 3,  Nome = "Maria Helena",     Email = "maria@gmail.com",             Perfil = "aluno",         Status = "ativo",   Cadastro = "20/04/2026" },
                new() { Id = 4,  Nome = "Carlos Dev",       Email = "carlos@email.com",            Perfil = "aluno",         Status = "inativo", Cadastro = "10/02/2026" },
                new() { Id = 5,  Nome = "Prof. Ana Lima",   Email = "ana.lima@escola.edu.br",      Perfil = "professor",     Status = "ativo",   Cadastro = "05/01/2026", Especialidade = "Banco de Dados" },
                new() { Id = 6,  Nome = "Alberto Caiero",   Email = "alberto.caiero@edu.br",       Perfil = "aluno",         Status = "ativo",   Cadastro = "03/01/2026" },
                new() { Id = 7,  Nome = "Álvaro de Campos", Email = "alvaro.campos@edu.br",        Perfil = "aluno",         Status = "ativo",   Cadastro = "07/01/2026" },
                new() { Id = 8,  Nome = "Ricardo Reis",     Email = "ricardo.reis@edu.br",         Perfil = "aluno",         Status = "ativo",   Cadastro = "10/01/2026" },
                new() { Id = 9,  Nome = "Bernardo Soares",  Email = "bernardo.soares@edu.br",      Perfil = "aluno",         Status = "inativo", Cadastro = "12/01/2026" },
                new() { Id = 10, Nome = "Administrador",    Email = "admin@hytech.com",            Perfil = "administrador", Status = "ativo",   Cadastro = "01/01/2026" },
            };

            _proximoId = _usuarios.Max(u => u.Id) + 1;
        }

        public List<Usuario> ObterTodos() => _usuarios.ToList();

        public Usuario? ObterPorId(int id) => _usuarios.FirstOrDefault(u => u.Id == id);

        public void Adicionar(Usuario usuario)
        {
            usuario.Id = _proximoId++;
            usuario.Cadastro = DateTime.Now.ToString("dd/MM/yyyy");
            _usuarios.Insert(0, usuario);
        }

        public void Atualizar(Usuario usuario)
        {
            var existente = ObterPorId(usuario.Id);
            if (existente is null) return;

            existente.Nome = usuario.Nome;
            existente.Email = usuario.Email;
            existente.Perfil = usuario.Perfil;
            existente.Especialidade = usuario.Especialidade;
            existente.Status = usuario.Status;
        }

        public void Remover(int id)
        {
            _usuarios.RemoveAll(u => u.Id == id);
        }

        public void AlternarStatus(int id)
        {
            var usuario = ObterPorId(id);
            if (usuario is null) return;
            usuario.Status = usuario.Status == "ativo" ? "inativo" : "ativo";
        }
    }
}
