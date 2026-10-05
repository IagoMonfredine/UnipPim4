namespace HytechAdmin.UI
{
    /// <summary>
    /// Paleta e tipografia únicas do app, baseadas na identidade visual da HYTECH,
    /// simplificadas para uma interface desktop objetiva.
    /// </summary>
    public static class Theme
    {
        public static readonly Color RoxoEscuro   = ColorTranslator.FromHtml("#2D0A6E");
        public static readonly Color RoxoMedio    = ColorTranslator.FromHtml("#5B2D8E");
        public static readonly Color RoxoClaro    = ColorTranslator.FromHtml("#E8D5FF");
        public static readonly Color RoxoBg       = ColorTranslator.FromHtml("#F3E8FF");
        public static readonly Color Amarelo      = ColorTranslator.FromHtml("#F5C518");
        public static readonly Color AmareloHover = ColorTranslator.FromHtml("#E0B000");
        public static readonly Color Branco       = Color.White;
        public static readonly Color CinzaFundo   = ColorTranslator.FromHtml("#F5F5F7");
        public static readonly Color CinzaBorda   = ColorTranslator.FromHtml("#E0E0E0");
        public static readonly Color CinzaTexto   = ColorTranslator.FromHtml("#5A5A5A");
        public static readonly Color Preto        = ColorTranslator.FromHtml("#1A1A1A");
        public static readonly Color Verde        = ColorTranslator.FromHtml("#2E7D32");
        public static readonly Color VerdeBg      = ColorTranslator.FromHtml("#E6F4EA");
        public static readonly Color Vermelho     = ColorTranslator.FromHtml("#C62828");
        public static readonly Color VermelhoBg   = ColorTranslator.FromHtml("#FBE9E7");
        public static readonly Color LaranjaBg    = ColorTranslator.FromHtml("#FFF3E0");
        public static readonly Color Laranja      = ColorTranslator.FromHtml("#B45309");

        public static readonly Font FonteTitulo    = new("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font FonteSubtitulo = new("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FonteBase      = new("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FonteBaseNegrito = new("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FonteBotao     = new("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FonteMenu      = new("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FonteMetrica   = new("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font FonteMono      = new("Consolas", 9F, FontStyle.Regular);

        /// <summary>Botão de destaque (amarelo), usado para ações principais.</summary>
        public static Button CriarBotaoPrimario(string texto)
        {
            var botao = new Button
            {
                Text = texto,
                Font = FonteBotao,
                BackColor = Amarelo,
                ForeColor = Preto,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 34,
                AutoSize = true,
                Padding = new Padding(14, 4, 14, 4)
            };
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = AmareloHover;
            return botao;
        }

        /// <summary>Botão secundário (contorno), usado para ações neutras.</summary>
        public static Button CriarBotaoSecundario(string texto)
        {
            var botao = new Button
            {
                Text = texto,
                Font = FonteBaseNegrito,
                BackColor = Branco,
                ForeColor = RoxoEscuro,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 34,
                AutoSize = true,
                Padding = new Padding(14, 4, 14, 4)
            };
            botao.FlatAppearance.BorderColor = RoxoMedio;
            botao.FlatAppearance.BorderSize = 1;
            botao.FlatAppearance.MouseOverBackColor = RoxoBg;
            return botao;
        }

        /// <summary>Botão pequeno de ação em tabela (ex.: Editar, Remover).</summary>
        public static Button CriarBotaoTabela(string texto, bool perigo = false)
        {
            var botao = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 8.5F),
                BackColor = perigo ? VermelhoBg : RoxoBg,
                ForeColor = perigo ? Vermelho : RoxoEscuro,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(2),
                AutoSize = true,
                Padding = new Padding(8, 3, 8, 3)
            };
            botao.FlatAppearance.BorderSize = 0;
            return botao;
        }

        public static Label CriarTituloPagina(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = FonteTitulo,
                ForeColor = Preto,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 2)
            };
        }

        public static Label CriarSubtitulo(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = FonteSubtitulo,
                ForeColor = CinzaTexto,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 14)
            };
        }

        public static TextBox CriarCampoTexto(string placeholderComoNome = "")
        {
            return new TextBox
            {
                Font = FonteBase,
                Width = 220,
                Height = 28
            };
        }

        public static DataGridView CriarGridPadrao()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Branco,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = CinzaBorda,
                Font = FonteBase,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 38 }
            };
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = RoxoBg;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = RoxoEscuro;
            grid.ColumnHeadersDefaultCellStyle.Font = FonteBaseNegrito;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersHeight = 36;
            grid.DefaultCellStyle.SelectionBackColor = RoxoClaro;
            grid.DefaultCellStyle.SelectionForeColor = Preto;
            grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            return grid;
        }

        /// <summary>Cria uma "pill" (etiqueta) colorida para status, no mesmo espírito dos badges do HTML.</summary>
        public static Panel CriarBadgeStatus(string texto, Color corFundo, Color corTexto)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = corTexto,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            var painel = new Panel
            {
                BackColor = corFundo,
                AutoSize = true,
                Padding = new Padding(8, 3, 8, 3)
            };
            painel.Controls.Add(lbl);
            return painel;
        }
    }
}
