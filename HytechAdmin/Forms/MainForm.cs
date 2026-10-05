using HytechAdmin.Controls;
using HytechAdmin.Services;
using HytechAdmin.UI;

namespace HytechAdmin.Forms
{
    /// <summary>
    /// Janela principal do painel do Administrador.
    /// Contém a navegação lateral (sidebar) e troca o conteúdo central
    /// entre as 4 telas do Admin: Usuários, Conteúdos, Config e Plataforma.
    /// </summary>
    public class MainForm : Form
    {
        // Serviços — hoje mocks em memória. Quando a API/BD existir, basta
        // trocar por implementações reais de IUsuarioService/IConteudoService/IPlataformaService.
        private readonly IUsuarioService _usuarioService = new UsuarioServiceMock();
        private readonly IConteudoService _conteudoService = new ConteudoServiceMock();
        private readonly IPlataformaService _plataformaService = new PlataformaServiceMock();

        private Panel _painelSidebar = null!;
        private Panel _painelConteudo = null!;
        private Label _lblTituloTopo = null!;
        private readonly Dictionary<string, Button> _botoesMenu = new();
        private Control? _telaAtual;

        private const string CHAVE_USUARIOS = "usuarios";
        private const string CHAVE_CONTEUDOS = "conteudos";
        private const string CHAVE_CONFIG = "config";
        private const string CHAVE_PLATAFORMA = "plataforma";

        public MainForm()
        {
            ConfigurarJanela();
            MontarSidebar();
            MontarAreaPrincipal();
            AbrirTela(CHAVE_USUARIOS);
        }

        private void ConfigurarJanela()
        {
            Text = "HYTECH — Painel do Administrador";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1080, 660);
            Size = new Size(1220, 740);
            Font = Theme.FonteBase;
            BackColor = Theme.CinzaFundo;
            Icon = null;
        }

        private void MontarSidebar()
        {
            _painelSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 226,
                BackColor = Theme.RoxoEscuro
            };
            Controls.Add(_painelSidebar);

            var lblLogo = new Label
            {
                Text = "HYTECH",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 19F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 26),
                Size = new Size(226, 40)
            };
            _painelSidebar.Controls.Add(lblLogo);

            var lblBadge = new Label
            {
                Text = "🛡️  Administrador",
                ForeColor = Theme.RoxoClaro,
                Font = Theme.FonteMenu,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 66),
                Size = new Size(226, 26)
            };
            _painelSidebar.Controls.Add(lblBadge);

            var linha = new Panel
            {
                BackColor = Theme.RoxoMedio,
                Location = new Point(20, 104),
                Size = new Size(186, 1)
            };
            _painelSidebar.Controls.Add(linha);

            int y = 124;
            CriarItemMenu("👥  Gerenciar Usuários", CHAVE_USUARIOS, ref y);
            CriarItemMenu("📂  Gerenciar Conteúdos", CHAVE_CONTEUDOS, ref y);
            CriarItemMenu("⚙️  Config", CHAVE_CONFIG, ref y);
            CriarItemMenu("📊  Plataforma", CHAVE_PLATAFORMA, ref y);

            var lblRodape = new Label
            {
                Text = "Sair da conta",
                ForeColor = Theme.RoxoClaro,
                Font = Theme.FonteSubtitulo,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Location = new Point(0, _painelSidebar.Height - 46),
                Size = new Size(226, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            lblRodape.Click += (_, _) =>
            {
                var confirmar = MessageBox.Show(this, "Deseja realmente sair?", "Sair",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmar == DialogResult.Yes) Close();
            };
            _painelSidebar.Controls.Add(lblRodape);
        }

        private void CriarItemMenu(string texto, string chave, ref int y)
        {
            var botao = new Button
            {
                Text = "   " + texto,
                Font = Theme.FonteMenu,
                Tag = chave,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                BackColor = Theme.RoxoEscuro,
                Location = new Point(0, y),
                Size = new Size(226, 46),
                Cursor = Cursors.Hand
            };
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Theme.RoxoMedio;
            botao.Click += (_, _) => AbrirTela(chave);

            _painelSidebar.Controls.Add(botao);
            _botoesMenu[chave] = botao;
            y += 46;
        }

        private void AtualizarMenuAtivo(string chaveAtiva)
        {
            foreach (var (chave, botao) in _botoesMenu)
            {
                bool ativo = chave == chaveAtiva;
                botao.BackColor = ativo ? Theme.RoxoMedio : Theme.RoxoEscuro;
                botao.Font = ativo
                    ? new Font(Theme.FonteMenu, FontStyle.Bold)
                    : Theme.FonteMenu;
            }
        }

        private void MontarAreaPrincipal()
        {
            var painelDireita = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.CinzaFundo
            };
            Controls.Add(painelDireita);
            painelDireita.BringToFront();

            var cabecalho = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Theme.Branco
            };
            var linhaCabecalho = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Theme.CinzaBorda };
            cabecalho.Controls.Add(linhaCabecalho);

            _lblTituloTopo = new Label
            {
                Text = "Gerenciar Usuários",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(24, 18)
            };
            cabecalho.Controls.Add(_lblTituloTopo);
            painelDireita.Controls.Add(cabecalho);

            _painelConteudo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.CinzaFundo,
                Padding = new Padding(24, 20, 24, 20)
            };
            painelDireita.Controls.Add(_painelConteudo);
            _painelConteudo.BringToFront();
        }

        private void AbrirTela(string chave)
        {
            _painelConteudo.SuspendLayout();
            _painelConteudo.Controls.Clear();
            _telaAtual?.Dispose();

            Control tela = chave switch
            {
                CHAVE_USUARIOS => new UsuariosControl(_usuarioService, _plataformaService),
                CHAVE_CONTEUDOS => new ConteudosControl(_conteudoService, _plataformaService),
                CHAVE_CONFIG => new ConfigControl(_plataformaService),
                CHAVE_PLATAFORMA => new PlataformaControl(_usuarioService, _conteudoService, _plataformaService),
                _ => new UsuariosControl(_usuarioService, _plataformaService)
            };

            tela.Dock = DockStyle.Fill;
            _painelConteudo.Controls.Add(tela);
            _telaAtual = tela;
            _painelConteudo.ResumeLayout();

            AtualizarMenuAtivo(chave);
            _lblTituloTopo.Text = TituloParaChave(chave);
        }

        private static string TituloParaChave(string chave) => chave switch
        {
            CHAVE_USUARIOS => "Gerenciar Usuários",
            CHAVE_CONTEUDOS => "Gerenciar Conteúdos",
            CHAVE_CONFIG => "Configurações",
            CHAVE_PLATAFORMA => "Plataforma",
            _ => "HYTECH"
        };
    }
}
