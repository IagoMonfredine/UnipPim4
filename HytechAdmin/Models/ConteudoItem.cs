namespace HytechAdmin.Models
{
    /// <summary>
    /// Representa um conteúdo publicado na plataforma: texto explicativo ou questão.
    /// </summary>
    public class ConteudoItem
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>texto | questao</summary>
        public string Tipo { get; set; } = "texto";

        public string Titulo { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;

        /// <summary>aprovado | pendente | rejeitado</summary>
        public string Status { get; set; } = "pendente";

        public string Data { get; set; } = string.Empty;

        /// <summary>Fácil | Médio | Difícil — usado apenas quando Tipo == "questao".</summary>
        public string Dificuldade { get; set; } = string.Empty;

        /// <summary>Corpo/descrição completa, exibido na tela de visualização.</summary>
        public string Corpo { get; set; } = string.Empty;

        public bool EhQuestao => Tipo == "questao";
    }
}
