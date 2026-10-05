using HytechAdmin.Models;
using HytechAdmin.Services;
using HytechAdmin.UI;

namespace HytechAdmin.Controls
{
    /// <summary>Tela "Gerenciar Conteúdos": listar, buscar, filtrar, visualizar e remover textos/questões.</summary>
    public class ConteudosControl : UserControl
    {
        private readonly IConteudoService _servico;
        private readonly IPlataformaService _plataforma;

        private TextBox _txtBusca = null!;
        private ComboBox _cmbTipo = null!;
        private ComboBox _cmbStatus = null!;
        private DataGridView _grid = null!;
        private Label _lblVazio = null!;

        public ConteudosControl(IConteudoService servico, IPlataformaService plataforma)
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

            var subtitulo = Theme.CriarSubtitulo("Acompanhe, visualize e remova textos explicativos e questões enviados pelos professores.");
            raiz.Controls.Add(subtitulo, 0, 0);

            var painelFiltros = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 4, 0, 12)
            };

            _txtBusca = new TextBox { Width = 240, Font = Theme.FonteBase, PlaceholderText = "🔍 Buscar por título ou autor..." };
            _txtBusca.TextChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_txtBusca));

            _cmbTipo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170, Font = Theme.FonteBase };
            _cmbTipo.Items.AddRange(new object[] { "Todos os Tipos", "Texto Explicativo", "Questão" });
            _cmbTipo.SelectedIndex = 0;
            _cmbTipo.SelectedIndexChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_cmbTipo));

            _cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, Font = Theme.FonteBase };
            _cmbStatus.Items.AddRange(new object[] { "Todos os Status", "Aprovado", "Pendente", "Rejeitado" });
            _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += (_, _) => CarregarDados();
            painelFiltros.Controls.Add(EnvolverComMargem(_cmbStatus));

            raiz.Controls.Add(painelFiltros, 0, 1);

            var painelGrid = new Panel { Dock = DockStyle.Fill };
            _grid = Theme.CriarGridPadrao();
            _grid.AutoGenerateColumns = false;
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Título", FillWeight = 24 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Tipo", HeaderText = "Tipo", FillWeight = 13 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Materia", HeaderText = "Matéria", FillWeight = 14 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Autor", HeaderText = "Autor", FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", FillWeight = 12 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Data", HeaderText = "Data", FillWeight = 10 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "AcaoVer", HeaderText = "", Text = "👁 Ver", UseColumnTextForButtonValue = true, FillWeight = 6 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "AcaoExcluir", HeaderText = "", Text = "🗑 Remover", UseColumnTextForButtonValue = true, FillWeight = 8 });
            _grid.CellContentClick += Grid_CellContentClick;
            painelGrid.Controls.Add(_grid);

            _lblVazio = new Label
            {
                Text = "📂  Nenhum conteúdo encontrado com os filtros selecionados.",
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
            var tipoFiltro = _cmbTipo.SelectedIndex switch
            {
                1 => "texto",
                2 => "questao",
                _ => ""
            };
            var statusFiltro = _cmbStatus.SelectedIndex switch
            {
                1 => "aprovado",
                2 => "pendente",
                3 => "rejeitado",
                _ => ""
            };

            var lista = _servico.ObterTodos()
                .Where(c => string.IsNullOrEmpty(busca) || c.Titulo.ToLower().Contains(busca) || c.Autor.ToLower().Contains(busca))
                .Where(c => string.IsNullOrEmpty(tipoFiltro) || c.Tipo == tipoFiltro)
                .Where(c => string.IsNullOrEmpty(statusFiltro) || c.Status == statusFiltro)
                .ToList();

            _grid.Rows.Clear();
            foreach (var c in lista)
            {
                int idx = _grid.Rows.Add(
                    c.Titulo,
                    c.EhQuestao ? $"❓ Questão ({c.Dificuldade})" : "📄 Texto",
                    c.Materia,
                    c.Autor,
                    FormatarStatus(c.Status),
                    c.Data,
                    "👁 Ver",
                    "🗑 Remover");
                _grid.Rows[idx].Tag = c.Id;
            }

            _lblVazio.Visible = lista.Count == 0;
            _grid.Visible = lista.Count > 0;
        }

        private static string FormatarStatus(string status) => status switch
        {
            "aprovado" => "✅ Aprovado",
            "pendente" => "🕓 Pendente",
            "rejeitado" => "⛔ Rejeitado",
            _ => status
        };

        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var linha = _grid.Rows[e.RowIndex];
            if (linha.Tag is not string id) return;
            var coluna = _grid.Columns[e.ColumnIndex].Name;
            var conteudo = _servico.ObterPorId(id);
            if (conteudo is null) return;

            switch (coluna)
            {
                case "AcaoVer":
                    using (var form = new HytechAdmin.Forms.ConteudoViewForm(conteudo))
                        form.ShowDialog(this);
                    break;

                case "AcaoExcluir":
                    var confirmar = MessageBox.Show(this,
                        $"Tem certeza que deseja remover \"{conteudo.Titulo}\"?\nEssa ação não poderá ser desfeita.",
                        "Confirmar remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmar == DialogResult.Yes)
                    {
                        _servico.Remover(id);
                        _plataforma.AdicionarLog($"CONTEÚDO REMOVIDO: \"{conteudo.Titulo}\" — Admin");
                        CarregarDados();
                    }
                    break;
            }
        }
    }
}
