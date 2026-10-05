using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>Implementação em memória, apenas para desenvolvimento/demonstração.</summary>
    public class ConteudoServiceMock : IConteudoService
    {
        private readonly List<ConteudoItem> _conteudos;

        public ConteudoServiceMock()
        {
            _conteudos = new List<ConteudoItem>
            {
                new() { Id = "c-static-1", Tipo = "texto",   Titulo = "Introdução a Variáveis",       Materia = "Programação",    Autor = "Prof. Silva",    Status = "aprovado", Data = "10/04/2026", Corpo = "Conteúdo introdutório sobre declaração, tipos e uso de variáveis." },
                new() { Id = "c-static-2", Tipo = "texto",   Titulo = "Estruturas de Repetição",      Materia = "Programação",    Autor = "Prof. Silva",    Status = "aprovado", Data = "12/04/2026", Corpo = "Uso de laços for e while com exemplos práticos." },
                new() { Id = "c-static-3", Tipo = "texto",   Titulo = "Fundamentos de Design",        Materia = "Design",         Autor = "Prof. Ana Lima", Status = "aprovado", Data = "15/04/2026", Corpo = "Princípios básicos de hierarquia visual, cor e tipografia." },
                new() { Id = "c-static-4", Tipo = "texto",   Titulo = "SQL Básico",                   Materia = "Banco de Dados", Autor = "Prof. Ana Lima", Status = "aprovado", Data = "18/04/2026", Corpo = "Comandos SELECT, INSERT, UPDATE e DELETE." },
                new() { Id = "c-static-5", Tipo = "questao", Titulo = "O que é tipagem dinâmica?",    Materia = "Programação",    Autor = "Prof. Silva",    Status = "aprovado", Data = "20/04/2026", Dificuldade = "Fácil",  Corpo = "Questão de múltipla escolha sobre tipagem dinâmica." },
                new() { Id = "c-static-6", Tipo = "questao", Titulo = "Explique o uso de JOIN em SQL",Materia = "Banco de Dados", Autor = "Prof. Ana Lima", Status = "aprovado", Data = "22/04/2026", Dificuldade = "Médio",  Corpo = "Questão sobre os tipos de JOIN em SQL." },
                new() { Id = "c-static-7", Tipo = "texto",   Titulo = "Funções e Escopo",              Materia = "Programação",    Autor = "Prof. Silva",    Status = "pendente", Data = "02/05/2026", Corpo = "Conteúdo sobre funções, escopo local e global." },
            };
        }

        public List<ConteudoItem> ObterTodos() => _conteudos.ToList();

        public ConteudoItem? ObterPorId(string id) => _conteudos.FirstOrDefault(c => c.Id == id);

        public void Remover(string id) => _conteudos.RemoveAll(c => c.Id == id);
    }
}
