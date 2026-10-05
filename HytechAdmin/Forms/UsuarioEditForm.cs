using HytechAdmin.Models;
using HytechAdmin.UI;

namespace HytechAdmin.Forms
{
    /// <summary>Modal de criação/edição de usuário (aluno, professor ou administrador).</summary>
    public class UsuarioEditForm : Form
    {
        private readonly Usuario? _usuarioOriginal;

        private TextBox _txtNome = null!;
        private TextBox _txtEmail = null!;
        private ComboBox _cmbPerfil = null!;
        private ComboBox _cmbEspecialidade = null!;
        private Label _lblEspecialidade = null!;
        private ComboBox _cmbStatus = null!;
        private Label _lblStatus = null!;

        /// <summary>Usuário preenchido, disponível após ShowDialog retornar DialogResult.OK.</summary>
        public Usuario UsuarioResultado { get; private set; } = new();

        public UsuarioEditForm(Usuario? usuarioExistente)
        {
            _usuarioOriginal = usuarioExistente;
            MontarLayout();
            if (usuarioExistente is not null) PreencherCampos(usuarioExistente);
        }

        private void MontarLayout()
        {
            Text = _usuarioOriginal is null ? "Novo Usuário" : "Editar Usuário";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(400, _usuarioOriginal is null ? 360 : 400);
            Font = Theme.FonteBase;
            BackColor = Theme.Branco;
            Padding = new Padding(28, 24, 28, 20);

            var lblTitulo = new Label
            {
                Text = (_usuarioOriginal is null ? "✏️ Novo Usuário" : "✏️ Editar Usuário"),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Theme.RoxoEscuro,
                AutoSize = true,
                Location = new Point(28, 20)
            };
            Controls.Add(lblTitulo);

            int y = 60;

            AdicionarRotulo("Nome", ref y);
            _txtNome = AdicionarCampoTexto(ref y);

            AdicionarRotulo("E-mail", ref y);
            _txtEmail = AdicionarCampoTexto(ref y);

            AdicionarRotulo("Perfil", ref y);
            _cmbPerfil = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(28, y),
                Width = 340,
                Font = Theme.FonteBase
            };
            _cmbPerfil.Items.AddRange(new object[] { "Aluno", "Professor", "Administrador" });
            _cmbPerfil.SelectedIndex = 0;
            _cmbPerfil.SelectedIndexChanged += (_, _) => AtualizarVisibilidadeEspecialidade();
            Controls.Add(_cmbPerfil);
            y += 40;

            _lblEspecialidade = new Label
            {
                Text = "Especialidade *",
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(28, y)
            };
            Controls.Add(_lblEspecialidade);
            y += 20;

            _cmbEspecialidade = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(28, y),
                Width = 340,
                Font = Theme.FonteBase
            };
            _cmbEspecialidade.Items.AddRange(new object[] { "💻 Programação", "🎨 Design", "🗄️ Banco de Dados", "🌐 Redes" });
            _cmbEspecialidade.SelectedIndex = 0;
            Controls.Add(_cmbEspecialidade);
            y += 40;

            if (_usuarioOriginal is not null)
            {
                _lblStatus = new Label
                {
                    Text = "Status",
                    Font = Theme.FonteBaseNegrito,
                    ForeColor = Theme.Preto,
                    AutoSize = true,
                    Location = new Point(28, y)
                };
                Controls.Add(_lblStatus);
                y += 20;

                _cmbStatus = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Location = new Point(28, y),
                    Width = 340,
                    Font = Theme.FonteBase
                };
                _cmbStatus.Items.AddRange(new object[] { "Ativo", "Inativo" });
                _cmbStatus.SelectedIndex = 0;
                Controls.Add(_cmbStatus);
                y += 40;
            }

            var btnCancelar = Theme.CriarBotaoSecundario("Cancelar");
            btnCancelar.Location = new Point(340 - btnCancelar.PreferredSize.Width - 100 + 28, y + 8);
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

            var btnSalvar = Theme.CriarBotaoPrimario("Salvar");
            btnSalvar.Click += BtnSalvar_Click;

            var painelBotoes = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Location = new Point(28, y + 8),
                Width = 340,
                AutoSize = true
            };
            painelBotoes.Controls.Add(btnSalvar);
            painelBotoes.Controls.Add(btnCancelar);
            Controls.Add(painelBotoes);

            AtualizarVisibilidadeEspecialidade();
            AcceptButton = btnSalvar;
            CancelButton = btnCancelar;
        }

        private void AdicionarRotulo(string texto, ref int y)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = Theme.FonteBaseNegrito,
                ForeColor = Theme.Preto,
                AutoSize = true,
                Location = new Point(28, y)
            };
            Controls.Add(lbl);
            y += 20;
        }

        private TextBox AdicionarCampoTexto(ref int y)
        {
            var txt = new TextBox
            {
                Location = new Point(28, y),
                Width = 340,
                Font = Theme.FonteBase
            };
            Controls.Add(txt);
            y += 40;
            return txt;
        }

        private void AtualizarVisibilidadeEspecialidade()
        {
            bool ehProfessor = _cmbPerfil.SelectedIndex == 1; // Professor
            _lblEspecialidade.Visible = ehProfessor;
            _cmbEspecialidade.Visible = ehProfessor;
        }

        private void PreencherCampos(Usuario usuario)
        {
            _txtNome.Text = usuario.Nome;
            _txtEmail.Text = usuario.Email;
            _cmbPerfil.SelectedIndex = usuario.Perfil switch
            {
                "professor" => 1,
                "administrador" => 2,
                _ => 0
            };

            if (!string.IsNullOrEmpty(usuario.Especialidade))
            {
                var idx = _cmbEspecialidade.Items.IndexOf(
                    _cmbEspecialidade.Items.Cast<string>().FirstOrDefault(i => i.Contains(usuario.Especialidade)) ?? "");
                if (idx >= 0) _cmbEspecialidade.SelectedIndex = idx;
            }

            if (_cmbStatus is not null)
                _cmbStatus.SelectedIndex = usuario.Status == "ativo" ? 0 : 1;

            AtualizarVisibilidadeEspecialidade();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            var nome = _txtNome.Text.Trim();
            var email = _txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show(this, "Preencha nome e e-mail.", "Campos obrigatórios",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var perfil = _cmbPerfil.SelectedIndex switch
            {
                1 => "professor",
                2 => "administrador",
                _ => "aluno"
            };

            if (perfil == "professor" && _cmbEspecialidade.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Selecione a especialidade do professor.", "Campo obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var especialidade = perfil == "professor"
                ? RemoverEmoji(_cmbEspecialidade.Text)
                : string.Empty;

            UsuarioResultado = new Usuario
            {
                Id = _usuarioOriginal?.Id ?? 0,
                Nome = nome,
                Email = email,
                Perfil = perfil,
                Especialidade = especialidade,
                Status = _cmbStatus is not null
                    ? (_cmbStatus.SelectedIndex == 0 ? "ativo" : "inativo")
                    : "ativo",
                Cadastro = _usuarioOriginal?.Cadastro ?? string.Empty
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private static string RemoverEmoji(string texto)
        {
            var partes = texto.Split(' ', 2);
            return partes.Length == 2 ? partes[1].Trim() : texto.Trim();
        }
    }
}
