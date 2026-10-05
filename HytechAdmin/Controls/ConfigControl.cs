using HytechAdmin.Models;
using HytechAdmin.Services;
using HytechAdmin.UI;

namespace HytechAdmin.Controls
{
    /// <summary>Tela "Config": parâmetros de gamificação (chaves distribuídas por dificuldade e certificado).</summary>
    public class ConfigControl : UserControl
    {
        private readonly IPlataformaService _servico;

        private NumericUpDown _numFacil = null!;
        private NumericUpDown _numMedia = null!;
        private NumericUpDown _numDificil = null!;
        private NumericUpDown _numCertificado = null!;

        public ConfigControl(IPlataformaService servico)
        {
            _servico = servico;
            Dock = DockStyle.Fill;
            MontarLayout();
            CarregarConfiguracao();
        }

        private void MontarLayout()
        {
            var subtitulo = Theme.CriarSubtitulo("Defina a quantidade de chaves (pontos de gamificação) concedidas por ação na plataforma.");
            subtitulo.Location = new Point(0, 0);
            Controls.Add(subtitulo);

            var cartao = new Panel
            {
                BackColor = Theme.Branco,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 40),
                Size = new Size(480, 330),
                Padding = new Padding(24)
            };
            Controls.Add(cartao);

            var lblCartaoTitulo = new Label
            {
                Text = "🔑 Pontuação de Gamificação",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.RoxoEscuro,
                AutoSize = true,
                Location = new Point(20, 16)
            };
            cartao.Controls.Add(lblCartaoTitulo);

            int y = 56;
            _numFacil = AdicionarCampoNumerico(cartao, "Questão nível Fácil", ref y);
            _numMedia = AdicionarCampoNumerico(cartao, "Questão nível Médio", ref y);
            _numDificil = AdicionarCampoNumerico(cartao, "Questão nível Difícil", ref y);
            _numCertificado = AdicionarCampoNumerico(cartao, "Emissão de certificado", ref y);

            var btnSalvar = Theme.CriarBotaoPrimario("💾 Salvar Configurações");
            btnSalvar.Location = new Point(20, y + 10);
            btnSalvar.Click += BtnSalvar_Click;
            cartao.Controls.Add(btnSalvar);
        }

        private NumericUpDown AdicionarCampoNumerico(Panel container, string rotulo, ref int y)
        {
            var lbl = new Label
            {
                Text = rotulo,
                Font = Theme.FonteBase,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(20, y + 4)
            };
            container.Controls.Add(lbl);

            var num = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000,
                Font = Theme.FonteBase,
                Location = new Point(300, y),
                Width = 100,
                TextAlign = HorizontalAlignment.Center
            };
            container.Controls.Add(num);

            var lblChaves = new Label
            {
                Text = "chaves",
                Font = Theme.FonteSubtitulo,
                ForeColor = Theme.CinzaTexto,
                AutoSize = true,
                Location = new Point(408, y + 4)
            };
            container.Controls.Add(lblChaves);

            y += 44;
            return num;
        }

        private void CarregarConfiguracao()
        {
            var config = _servico.ObterConfiguracao();
            _numFacil.Value = config.ChavesQuestaoFacil;
            _numMedia.Value = config.ChavesQuestaoMedia;
            _numDificil.Value = config.ChavesQuestaoDificil;
            _numCertificado.Value = config.ChavesCertificado;
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            var config = new ConfiguracaoPlataforma
            {
                ChavesQuestaoFacil = (int)_numFacil.Value,
                ChavesQuestaoMedia = (int)_numMedia.Value,
                ChavesQuestaoDificil = (int)_numDificil.Value,
                ChavesCertificado = (int)_numCertificado.Value
            };
            _servico.SalvarConfiguracao(config);

            MessageBox.Show(this, "Configurações salvas com sucesso!", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
