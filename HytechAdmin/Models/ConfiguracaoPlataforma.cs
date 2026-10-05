namespace HytechAdmin.Models
{
    /// <summary>
    /// Parâmetros de gamificação configuráveis pelo Admin (Aba Config).
    /// </summary>
    public class ConfiguracaoPlataforma
    {
        public int ChavesQuestaoFacil { get; set; } = 5;
        public int ChavesQuestaoMedia { get; set; } = 10;
        public int ChavesQuestaoDificil { get; set; } = 15;
        public int ChavesCertificado { get; set; } = 50;
    }
}
