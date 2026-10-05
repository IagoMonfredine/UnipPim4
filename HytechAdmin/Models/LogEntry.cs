namespace HytechAdmin.Models
{
    /// <summary>Uma linha do log do sistema exibido na aba Plataforma.</summary>
    public class LogEntry
    {
        public DateTime Momento { get; set; } = DateTime.Now;
        public string Mensagem { get; set; } = string.Empty;

        public string HoraFormatada => $"[{Momento:dd/MM/yyyy HH:mm}]";
    }
}
