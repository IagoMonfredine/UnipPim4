using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>Implementação em memória, apenas para desenvolvimento/demonstração.</summary>
    public class PlataformaServiceMock : IPlataformaService
    {
        private ConfiguracaoPlataforma _configuracao = new();

        private readonly List<LogEntry> _logs = new()
        {
            new() { Momento = new DateTime(2026, 5, 11, 9, 12, 0),  Mensagem = "LOGIN: Pedro Bala — Aluno" },
            new() { Momento = new DateTime(2026, 5, 11, 8, 55, 0),  Mensagem = "CONTEÚDO ENVIADO: \"Funções e Escopo\" — Prof. Silva" },
            new() { Momento = new DateTime(2026, 5, 10, 17, 30, 0), Mensagem = "QUESTÃO APROVADA: #023 — Admin" },
            new() { Momento = new DateTime(2026, 5, 10, 15, 40, 0), Mensagem = "CADASTRO: Maria Helena — Novo Aluno" },
            new() { Momento = new DateTime(2026, 5, 10, 11, 30, 0), Mensagem = "CERTIFICADO EMITIDO: Alberto Caiero" },
            new() { Momento = new DateTime(2026, 5, 9, 14, 22, 0),  Mensagem = "LOGIN: Álvaro de Campos — Aluno" },
            new() { Momento = new DateTime(2026, 5, 9, 13, 10, 0),  Mensagem = "CONTEÚDO ENVIADO: \"Design de Interface\" — Prof. Ana Lima" },
            new() { Momento = new DateTime(2026, 5, 8, 16, 5, 0),   Mensagem = "USUÁRIO INATIVADO: Carlos Dev — Admin" },
            new() { Momento = new DateTime(2026, 5, 8, 10, 20, 0),  Mensagem = "CERTIFICADO EMITIDO: Ricardo Reis" },
            new() { Momento = new DateTime(2026, 5, 7, 9, 0, 0),    Mensagem = "CONFIGURAÇÃO ATUALIZADA: Pontuação de gamificação" },
        };

        public ConfiguracaoPlataforma ObterConfiguracao() => _configuracao;

        public void SalvarConfiguracao(ConfiguracaoPlataforma configuracao)
        {
            _configuracao = configuracao;
            AdicionarLog("CONFIGURAÇÃO ATUALIZADA: Pontuação de gamificação — Admin");
        }

        public List<LogEntry> ObterLogs() => _logs.OrderByDescending(l => l.Momento).ToList();

        public void AdicionarLog(string mensagem)
        {
            _logs.Add(new LogEntry { Momento = DateTime.Now, Mensagem = mensagem });
        }
    }
}
