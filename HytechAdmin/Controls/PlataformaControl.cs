using HytechAdmin.Services;
using HytechAdmin.UI;

namespace HytechAdmin.Controls
{
    /// <summary>Tela "Plataforma": métricas gerais, distribuição de perfis e log do sistema.</summary>
    public class PlataformaControl : UserControl
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConteudoService _conteudoService;
        private readonly IPlataformaService _plataformaService;

        private FlowLayoutPanel _painelCards = null!;
        private Panel _painelDistribuicao = null!;
        private ListBox _listaLog = null!;

        public PlataformaControl(IUsuarioService usuarioService, IConteudoService conteudoService, IPlataformaService plataformaService)
        {
            _usuarioService = usuarioService;
            _conteudoService = conteudoService;
            _plataformaService = plataformaService;
            Dock = DockStyle.Fill;
            MontarLayout();
            AtualizarDados();
        }

        private void MontarLayout()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(raiz);

            var subtitulo = Theme.CriarSubtitulo("Visão geral do uso da plataforma e histórico de atividades recentes.");
            raiz.Controls.Add(subtitulo, 0, 0);

            _painelCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 20)
            };
            raiz.Controls.Add(_painelCards, 0, 1);

            _painelDistribuicao = new Panel
            {
                BackColor = Theme.Branco,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(560, 190),
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(20)
            };
            raiz.Controls.Add(_painelDistribuicao, 0, 2);

            var painelLog = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Branco,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };
            var lblLogTitulo = new Label
            {
                Text = "🗒️ Log do Sistema",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.RoxoEscuro,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            painelLog.Controls.Add(lblLogTitulo);

            _listaLog = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = Theme.FonteMono,
                BorderStyle = BorderStyle.None,
                BackColor = Theme.CinzaFundo,
                IntegralHeight = false,
                Margin = new Padding(0, 8, 0, 0)
            };
            painelLog.Controls.Add(_listaLog);
            _listaLog.BringToFront();

            raiz.Controls.Add(painelLog, 0, 3);
        }

        private void AtualizarDados()
        {
            var usuarios = _usuarioService.ObterTodos();
            var conteudos = _conteudoService.ObterTodos();
            var config = _plataformaService.ObterConfiguracao();

            int totalUsuarios = usuarios.Count;
            int alunos = usuarios.Count(u => u.Perfil == "aluno");
            int professores = usuarios.Count(u => u.Perfil == "professor");
            int administradores = usuarios.Count(u => u.Perfil == "administrador");
            int ativos = usuarios.Count(u => u.EhAtivo);
            double taxaRetencao = totalUsuarios == 0 ? 0 : (double)ativos / totalUsuarios * 100;

            int questoesAprovadas = conteudos.Count(c => c.EhQuestao && c.Status == "aprovado");
            int chavesEstimadas = questoesAprovadas * config.ChavesQuestaoMedia;
            int certificadosEstimados = Math.Max(1, ativos / 3); // estimativa simples para demonstração

            // Cards de métricas
            _painelCards.Controls.Clear();
            _painelCards.Controls.Add(CriarCard("👤", "Total de Usuários", totalUsuarios.ToString(), Theme.RoxoEscuro));
            _painelCards.Controls.Add(CriarCard("🔑", "Chaves Distribuídas (est.)", chavesEstimadas.ToString("N0"), Theme.Laranja));
            _painelCards.Controls.Add(CriarCard("🎓", "Certificados Emitidos (est.)", certificadosEstimados.ToString(), Theme.Verde));
            _painelCards.Controls.Add(CriarCard("📈", "Taxa de Retenção", $"{taxaRetencao:0.#}%", Theme.RoxoMedio));

            // Distribuição de perfis
            _painelDistribuicao.Controls.Clear();
            var lblDistTitulo = new Label
            {
                Text = "👥 Distribuição de Usuários",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.RoxoEscuro,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            _painelDistribuicao.Controls.Add(lblDistTitulo);

            int y = 32;
            AdicionarBarraDistribuicao(_painelDistribuicao, "🎓 Alunos", alunos, totalUsuarios, Theme.RoxoMedio, ref y);
            AdicionarBarraDistribuicao(_painelDistribuicao, "👨‍🏫 Professores", professores, totalUsuarios, Theme.Amarelo, ref y);
            AdicionarBarraDistribuicao(_painelDistribuicao, "🛡️ Administradores", administradores, totalUsuarios, Theme.Verde, ref y);

            // Log
            _listaLog.Items.Clear();
            foreach (var log in _plataformaService.ObterLogs())
                _listaLog.Items.Add($"{log.HoraFormatada}  {log.Mensagem}");
        }

        private static Panel CriarCard(string icone, string titulo, string valor, Color corDestaque)
        {
            var card = new Panel
            {
                BackColor = Theme.Branco,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(210, 100),
                Margin = new Padding(0, 0, 16, 16),
                Padding = new Padding(16)
            };

            var barraCor = new Panel { BackColor = corDestaque, Dock = DockStyle.Left, Width = 4 };
            card.Controls.Add(barraCor);

            var lblIcone = new Label
            {
                Text = icone,
                Font = new Font("Segoe UI", 16F),
                AutoSize = true,
                Location = new Point(16, 12)
            };
            card.Controls.Add(lblIcone);

            var lblValor = new Label
            {
                Text = valor,
                Font = Theme.FonteMetrica,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(16, 40)
            };
            card.Controls.Add(lblValor);

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = Theme.FonteSubtitulo,
                ForeColor = Theme.CinzaTexto,
                AutoSize = true,
                Location = new Point(16, 76)
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private static void AdicionarBarraDistribuicao(Panel container, string rotulo, int quantidade, int total, Color cor, ref int y)
        {
            double percentual = total == 0 ? 0 : (double)quantidade / total * 100;

            var lbl = new Label
            {
                Text = $"{rotulo}  ({quantidade} · {percentual:0.#}%)",
                Font = Theme.FonteBase,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(0, y)
            };
            container.Controls.Add(lbl);
            y += 20;

            var trilha = new Panel
            {
                BackColor = Theme.CinzaFundo,
                Location = new Point(0, y),
                Size = new Size(500, 12)
            };
            var preenchido = new Panel
            {
                BackColor = cor,
                Location = new Point(0, 0),
                Size = new Size((int)(500 * percentual / 100), 12)
            };
            trilha.Controls.Add(preenchido);
            container.Controls.Add(trilha);
            y += 22;
        }
    }
}
