using HytechAdmin.Models;

namespace HytechAdmin.Services
{
    /// <summary>
    /// Contrato do serviço de configurações de gamificação e log do sistema.
    /// Ver observação em <see cref="IUsuarioService"/> sobre a futura troca por API/BD.
    /// </summary>
    public interface IPlataformaService
    {
        ConfiguracaoPlataforma ObterConfiguracao();
        void SalvarConfiguracao(ConfiguracaoPlataforma configuracao);
        List<LogEntry> ObterLogs();
        void AdicionarLog(string mensagem);
    }
}
