using HytechAdmin.Models;
using HytechAdmin.UI;

namespace HytechAdmin.Forms
{
    /// <summary>Modal simples de visualização de um conteúdo (texto explicativo ou questão).</summary>
    public class ConteudoViewForm : Form
    {
        public ConteudoViewForm(ConteudoItem conteudo)
        {
            Text = "Visualizar Conteúdo";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(460, 380);
            BackColor = Theme.Branco;
            Font = Theme.FonteBase;
            Padding = new Padding(28, 24, 28, 20);

            var lblTitulo = new Label
            {
                Text = conteudo.Titulo,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Theme.RoxoEscuro,
                AutoSize = false,
                Location = new Point(28, 20),
                Size = new Size(404, 46)
            };
            Controls.Add(lblTitulo);

            var lblMeta = new Label
            {
                Text = $"{(conteudo.EhQuestao ? "❓ Questão" : "📄 Texto Explicativo")} · {conteudo.Materia} · por {conteudo.Autor}",
                Font = Theme.FonteSubtitulo,
                ForeColor = Theme.CinzaTexto,
                AutoSize = false,
                Location = new Point(28, 64),
                Size = new Size(404, 20)
            };
            Controls.Add(lblMeta);

            var y = 92;

            if (conteudo.EhQuestao)
            {
                var lblDificuldade = new Label
                {
                    Text = $"Dificuldade: {conteudo.Dificuldade}",
                    Font = Theme.FonteBaseNegrito,
                    ForeColor = Theme.Preto,
                    AutoSize = true,
                    Location = new Point(28, y)
                };
                Controls.Add(lblDificuldade);
                y += 26;
            }

            var lblStatus = new Label
            {
                Text = $"Status: {FormatarStatus(conteudo.Status)}      Data: {conteudo.Data}",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(28, y)
            };
            Controls.Add(lblStatus);
            y += 32;

            var txtCorpo = new TextBox
            {
                Text = string.IsNullOrWhiteSpace(conteudo.Corpo) ? "(Sem descrição cadastrada.)" : conteudo.Corpo,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Theme.CinzaFundo,
                BorderStyle = BorderStyle.FixedSingle,
                Font = Theme.FonteBase,
                Location = new Point(28, y),
                Size = new Size(404, 168)
            };
            Controls.Add(txtCorpo);
            y += 178;

            var btnFechar = Theme.CriarBotaoPrimario("Fechar");
            btnFechar.Location = new Point(404 - btnFechar.PreferredSize.Width + 28, y);
            btnFechar.Click += (_, _) => Close();
            Controls.Add(btnFechar);

            AcceptButton = btnFechar;
        }

        private static string FormatarStatus(string status) => status switch
        {
            "aprovado" => "✅ Aprovado",
            "pendente" => "🕓 Pendente",
            "rejeitado" => "⛔ Rejeitado",
            _ => status
        };
    }
}
