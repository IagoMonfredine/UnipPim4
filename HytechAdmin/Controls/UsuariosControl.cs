using HytechAdmin.Forms;
using HytechAdmin.Models;
using HytechAdmin.Services;
using HytechAdmin.UI;

namespace HytechAdmin.Controls
{
    /// <summary>Tela "Gerenciar Usuários": listar, buscar, filtrar, criar, editar, ativar/inativar e excluir.</summary>
    public class UsuariosControl : UserControl
    {
        private readonly IUsuarioService _servico;
        private readonly IPlataformaService _plataforma;

        private TextBox _txtBusca = null!;
        private ComboBox _cmbPerfil = null!;
        private ComboBox _cmbStatus = null!;
        private DataGridView _grid = null!;
        private Label _lblVazio = null!;

        public UsuariosControl(IUsuarioService servico, IPlataformaService plataforma)
        {
            _servico = servico;
            _plataforma = plataforma;
            Dock = DockStyle.Fill;
            MontarLayout();
            CarregarDados();
        }

        private void MontarLayout()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Theme.CinzaFundo
            };
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(raiz);

            // Cabeçalho com título + botão novo usuário
            var painelTopo = new Panel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0, 0, 0, 8) };
            var subtitulo = Theme.CriarSubtitulo("Crie, edite, ative/inative e defina permissões dos usuários.");
            subtitulo.Location = new Point(0, 0);
            painelTopo.Controls.Add(subtitulo);

            var btnNovo = Theme.CriarBotaoPrimario("+ Novo Usuário");
            btnNovo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovo.Click += (_, _) => AbrirModalUsuario(null);
            painelTopo.Controls.Add(btnNovo);
            painelTopo.Resize += (_, _) => btnNovo.Location = new Point(painelTopo.Width - btnNovo.Width, 0);
            painelTopo.Height = 30;
            raiz.Controls.Add(painelTopo, 0, 0);

            // Filtros
            var painelFiltros = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 4, 0, 12)
            };

            _txtBusca = Theme.CriarCampoTexto();
            _txtBusca.Width = 240;
            _txtBusca.PlaceholderText = "🔍 Buscar por nome ou e-mail...";
            _txtBusca.TextChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_txtBusca));

            _cmbPerfil = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, Font = Theme.FonteBase };
            _cmbPerfil.Items.AddRange(new object[] { "Todos os Perfis", "Aluno", "Professor", "Administrador" });
            _cmbPerfil.SelectedIndex = 0;
            _cmbPerfil.SelectedIndexChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_cmbPerfil));

            _cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Font = Theme.FonteBase };
            _cmbStatus.Items.AddRange(new object[] { "Todos os Status", "Ativo", "Inativo" });
            _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_cmbStatus));

            raiz.Controls.Add(painelFiltros, 0, 1);

            // Grid
            var painelGrid = new Panel { Dock = DockStyle.Fill };
            _grid = Theme.CriarGridPadrao();
            _grid.AutoGenerateColumns = false;
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", FillWeight = 18 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "E-mail", FillWeight = 22 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Perfil", HeaderText = "Perfil", FillWeight = 13 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Especialidade", HeaderText = "Especialidade", FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", FillWeight = 10 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cadastro", HeaderText = "Cadastro", FillWeight = 10 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "AcaoEditar", HeaderText = "", Text = "✏️ Editar", UseColumnTextForButtonValue = true, FillWeight = 8 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "AcaoStatus", HeaderText = "", Text = "⏻ Status", UseColumnTextForButtonValue = true, FillWeight = 8 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "AcaoExcluir", HeaderText = "", Text = "🗑 Excluir", UseColumnTextForButtonValue = true, FillWeight = 8 });
            _grid.CellContentClick += Grid_CellContentClick;
            painelGrid.Controls.Add(_grid);

            _lblVazio = new Label
            {
                Text = "🔍  Nenhum usuário encontrado com os filtros selecionados.",
                Font = Theme.FonteBase,
                ForeColor = Theme.CinzaTexto,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };
            painelGrid.Controls.Add(_lblVazio);
            _lblVazio.BringToFront();

            raiz.Controls.Add(painelGrid, 0, 2);
        }

        private static Control EnvolverComMargem(Control controle)
        {
            var painel = new Panel { AutoSize = true, Margin = new Padding(0, 0, 10, 0) };
            controle.Location = Point.Empty;
            painel.Controls.Add(controle);
            painel.Height = controle.Height;
            painel.Width = controle.Width;
            return painel;
        }

        private void CarregarDados()
        {
            var busca = _txtBusca.Text.Trim().ToLower();
            var perfilFiltro = _cmbPerfil.SelectedIndex switch
            {
                1 => "aluno",
                2 => "professor",
                3 => "administrador",
                _ => ""
            };
            var statusFiltro = _cmbStatus.SelectedIndex switch
            {
                1 => "ativo",
                2 => "inativo",
                _ => ""
            };

            var lista = _servico.ObterTodos()
                .Where(u => string.IsNullOrEmpty(busca) || u.Nome.ToLower().Contains(busca) || u.Email.ToLower().Contains(busca))
                .Where(u => string.IsNullOrEmpty(perfilFiltro) || u.Perfil == perfilFiltro)
                .Where(u => string.IsNullOrEmpty(statusFiltro) || u.Status == statusFiltro)
                .ToList();

            _grid.Rows.Clear();
            foreach (var u in lista)
            {
                int idx = _grid.Rows.Add(
                    u.Nome,
                    u.Email,
                    FormatarPerfil(u.Perfil),
                    string.IsNullOrEmpty(u.Especialidade) ? "—" : u.Especialidade,
                    u.Status == "ativo" ? "🟢 Ativo" : "🔴 Inativo",
                    u.Cadastro,
                    "✏️ Editar",
                    u.Status == "ativo" ? "⏸ Inativar" : "▶ Ativar",
                    "🗑 Excluir");
                _grid.Rows[idx].Tag = u.Id;
            }

            _lblVazio.Visible = lista.Count == 0;
            _grid.Visible = lista.Count > 0;
        }

        private static string FormatarPerfil(string perfil) => perfil switch
        {
            "aluno" => "🎓 Aluno",
            "professor" => "👨‍🏫 Professor",
            "administrador" => "🛡️ Administrador",
            _ => perfil
        };

        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var linha = _grid.Rows[e.RowIndex];
            if (linha.Tag is not int id) return;
            var coluna = _grid.Columns[e.ColumnIndex].Name;
            var usuario = _servico.ObterPorId(id);
            if (usuario is null) return;

            switch (coluna)
            {
                case "AcaoEditar":
                    AbrirModalUsuario(usuario);
                    break;

                case "AcaoStatus":
                    _servico.AlternarStatus(id);
                    _plataforma.AdicionarLog($"USUÁRIO {(usuario.Status == "ativo" ? "INATIVADO" : "ATIVADO")}: {usuario.Nome} — Admin");
                    CarregarDados();
                    break;

                case "AcaoExcluir":
                    var confirmar = MessageBox.Show(this,
                        $"Tem certeza que deseja excluir o usuário \"{usuario.Nome}\"?\nEssa ação não poderá ser desfeita.",
                        "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmar == DialogResult.Yes)
                    {
                        _servico.Remover(id);
                        _plataforma.AdicionarLog($"USUÁRIO EXCLUÍDO: {usuario.Nome} — Admin");
                        CarregarDados();
                    }
                    break;
            }
        }

        private void AbrirModalUsuario(Usuario? usuarioExistente)
        {
            using var form = new UsuarioEditForm(usuarioExistente);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            if (usuarioExistente is null)
            {
                _servico.Adicionar(form.UsuarioResultado);
                _plataforma.AdicionarLog($"CADASTRO: {form.UsuarioResultado.Nome} — {FormatarPerfil(form.UsuarioResultado.Perfil)} (Admin)");
            }
            else
            {
                _servico.Atualizar(form.UsuarioResultado);
                _plataforma.AdicionarLog($"USUÁRIO EDITADO: {form.UsuarioResultado.Nome} — Admin");
            }
            CarregarDados();
        }
    }
}
