using System.ComponentModel;
using Guna.UI2.WinForms;

namespace RentaVehiculo
{
    public partial class MainForm : Form
    {
        private readonly Color _sidebarBg = Color.FromArgb(15, 23, 42);
        private readonly Color _mainBg = Color.FromArgb(248, 250, 252);
        private readonly Color _accentBlue = Color.FromArgb(37, 99, 235);
        private readonly Color _textMuted = Color.FromArgb(148, 163, 184);
        private readonly Color _textDark = Color.FromArgb(15, 23, 42);

        private Guna2Panel _pnlSidebar = null!;
        private Guna2Panel _pnlMain = null!;
        private readonly List<Guna2Button> _navButtons = new();

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            // En tiempo de diseño el diseñador solo pinta lo declarado en MainForm.Designer.cs.
            // BuildDashboard() no se ejecuta de forma fiable aquí; no mostrar controles Guna en diseño.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            RemoveDesignTimeHintPanel();
            BackColor = _mainBg;
            BuildDashboard();
        }

        private void RemoveDesignTimeHintPanel()
        {
            if (!Controls.Contains(designTimeHint))
                return;
            Controls.Remove(designTimeHint);
            designTimeHint.Dispose();
        }

        private void BuildDashboard()
        {
            _pnlSidebar = new Guna2Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                FillColor = _sidebarBg,
                BorderRadius = 0,
                Padding = new Padding(20, 28, 20, 24)
            };

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                Padding = new Padding(0, 0, 0, 8),
                BackColor = Color.Transparent
            };
            var lblBrand = new Label
            {
                Text = "RentCar Pro",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            var lblSubBrand = new Label
            {
                Text = "Sistema de Gestión",
                Font = new Font("Segoe UI", 9F),
                ForeColor = _textMuted,
                AutoSize = true,
                Location = new Point(0, 34)
            };
            pnlHeader.Controls.Add(lblBrand);
            pnlHeader.Controls.Add(lblSubBrand);

            var pnlNavHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 8)
            };

            (int iconCode, string label)[] navItems =
            {
                (0xE804, "Gestión de Flota"),
                (0xE716, "Clientes"),
                (0xE787, "Reservas"),
                (0xE8F1, "Rentas Activas"),
                (0xE713, "Mantenimiento"),
                (0xE8A5, "Facturación")
            };

            for (var i = 0; i < navItems.Length; i++)
            {
                var btn = CreateNavButton(navItems[i].label, navItems[i].iconCode, i);
                btn.Location = new Point(0, i * 46);
                btn.Size = new Size(pnlNavHost.Width, 40);
                pnlNavHost.Controls.Add(btn);
                _navButtons.Add(btn);
            }

            SetSelectedNav(0);

            var pnlProfile = new Guna2Panel
            {
                Dock = DockStyle.Bottom,
                Height = 72,
                FillColor = Color.FromArgb(30, 41, 59),
                BorderRadius = 12,
                Margin = new Padding(0, 12, 0, 0)
            };

            var avatar = new Guna2Panel
            {
                Size = new Size(40, 40),
                Location = new Point(12, 12),
                FillColor = _accentBlue,
                BorderRadius = 20
            };
            var lblAv = new Label
            {
                Text = "AD",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = avatar.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            avatar.Controls.Add(lblAv);

            var lblUser = new Label
            {
                Text = "Admin",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(60, 14),
                AutoSize = true
            };
            var lblMail = new Label
            {
                Text = "admin@rentcar.com",
                Font = new Font("Segoe UI", 8F),
                ForeColor = _textMuted,
                Location = new Point(60, 34),
                AutoSize = true
            };

            pnlProfile.Controls.Add(avatar);
            pnlProfile.Controls.Add(lblUser);
            pnlProfile.Controls.Add(lblMail);

            _pnlSidebar.Controls.Add(pnlProfile);
            _pnlSidebar.Controls.Add(pnlHeader);
            _pnlSidebar.Controls.Add(pnlNavHost);
            _pnlSidebar.Resize += (_, _) =>
            {
                foreach (Guna2Button b in _navButtons)
                    b.Width = pnlNavHost.ClientSize.Width;
            };

            _pnlMain = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = _mainBg,
                BorderRadius = 0,
                Padding = new Padding(32, 28, 32, 24),
                AutoScroll = true
            };

            var headerTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            var headerSub = new Label
            {
                Text = "Bienvenido al sistema de gestión de renta de vehículos",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(0, 38)
            };

            var cardsRow = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Dock = DockStyle.Top,
                Height = 130,
                Margin = new Padding(0, 12, 0, 0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            cardsRow.Controls.Add(CreateSummaryCard(
                "Vehículos Activos", "42", "+5 este mes", Color.FromArgb(34, 197, 94),
                Mdl2Char(0xE7C1)), 0, 0);
            cardsRow.Controls.Add(CreateSummaryCard(
                "Rentas Próximas a Vencer", "8", "Hoy y mañana", Color.FromArgb(249, 115, 22),
                Mdl2Char(0xE916), subForeColor: Color.FromArgb(239, 68, 68)), 1, 0);
            cardsRow.Controls.Add(CreateSummaryCard(
                "Alertas de Mantenimiento", "3", "Requieren atención", Color.FromArgb(234, 88, 12),
                Mdl2Char(0xE946), subForeColor: Color.FromArgb(239, 68, 68)), 2, 0);

            var lblQuick = new Label
            {
                Text = "Accesos Rápidos",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(0, 0),
                Margin = new Padding(0, 20, 0, 0)
            };

            var quickGrid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 12, 0, 0)
            };
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));

            quickGrid.Controls.Add(CreateQuickCard("Nueva Renta", "Registrar una nueva renta de vehículo", Mdl2Char(0xE710)), 0, 0);
            quickGrid.Controls.Add(CreateQuickCard("Registrar Cliente", "Añadir un nuevo cliente al sistema", Mdl2Char(0xE716)), 1, 0);
            quickGrid.Controls.Add(CreateQuickCard("Ver Disponibilidad", "Consultar vehículos disponibles", Mdl2Char(0xE7B3)), 0, 1);
            quickGrid.Controls.Add(CreateQuickCard("Estado de Mantenimiento", "Revisar mantenimientos programados", Mdl2Char(0xE713)), 1, 1);

            var quickWrap = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Width = _pnlMain.ClientSize.Width - 64
            };
            quickWrap.Controls.Add(lblQuick);
            quickWrap.Controls.Add(quickGrid);
            lblQuick.Location = new Point(0, 0);
            quickGrid.Location = new Point(0, 32);
            quickGrid.Width = quickWrap.Width;

            var lblAlerts = new Label
            {
                Text = "Alertas y Notificaciones",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            var pnlAlertsBox = new Guna2Panel
            {
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                ShadowDecoration = { Enabled = true, Depth = 4, Color = Color.FromArgb(40, 0, 0, 0) },
                AutoSize = true,
                Padding = new Padding(4, 4, 4, 8)
            };

            var flpAlerts = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = 800,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 8, 8, 8)
            };

            flpAlerts.Controls.Add(CreateAlertRow(
                Color.FromArgb(59, 130, 246), Mdl2Char(0xE713),
                "Mantenimiento Programado", "Toyota Camry 2023 - Revisión de 10,000 km", "Mañana"));

            flpAlerts.Controls.Add(CreateAlertRow(
                Color.FromArgb(249, 115, 22), Mdl2Char(0xE916),
                "Renta por Vencer", "Honda CR-V - Cliente: Juan Pérez", "En 2 horas"));

            flpAlerts.Controls.Add(CreateAlertRow(
                Color.FromArgb(239, 68, 68), Mdl2Char(0xE814),
                "Renta Vencida", "Nissan Sentra - Cliente: María González", "Hace 1 día"));

            pnlAlertsBox.Controls.Add(lblAlerts);
            pnlAlertsBox.Controls.Add(flpAlerts);
            lblAlerts.Left = 16;
            lblAlerts.Top = 12;
            flpAlerts.Left = 8;
            flpAlerts.Top = 44;
            flpAlerts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlAlertsBox.Resize += (_, _) =>
            {
                flpAlerts.Width = pnlAlertsBox.ClientSize.Width - 16;
            };

            var alertsOuter = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 24, 0, 0),
                Dock = DockStyle.Top
            };
            alertsOuter.Controls.Add(pnlAlertsBox);
            pnlAlertsBox.Dock = DockStyle.Top;

            var contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(0)
            };

            var stack = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            stack.Controls.Add(cardsRow);
            stack.Controls.Add(quickWrap);
            stack.Controls.Add(alertsOuter);

            cardsRow.Dock = DockStyle.Top;
            quickWrap.Dock = DockStyle.Top;
            alertsOuter.Dock = DockStyle.Top;

            contentHost.Controls.Add(stack);
            stack.Dock = DockStyle.Top;

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(headerTitle);
            headerPanel.Controls.Add(headerSub);

            _pnlMain.Controls.Add(contentHost);
            _pnlMain.Controls.Add(headerPanel);

            Controls.Add(_pnlMain);
            Controls.Add(_pnlSidebar);

            Shown += (_, _) =>
            {
                quickWrap.Width = _pnlMain.ClientSize.Width - _pnlMain.Padding.Horizontal;
                quickGrid.Width = quickWrap.Width;
                flpAlerts.Width = Math.Max(flpAlerts.Width, _pnlMain.ClientSize.Width - _pnlMain.Padding.Horizontal - 24);
            };
        }

        private static string Mdl2Char(int codepoint) =>
            char.ConvertFromUtf32(codepoint);

        private Guna2Button CreateNavButton(string text, int mdl2IconCode, int index)
        {
            var btn = new Guna2Button
            {
                Text = string.Empty,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BorderRadius = 8,
                BorderThickness = 0,
                FillColor = Color.Transparent,
                HoverState = { FillColor = Color.FromArgb(51, 65, 85) },
                TextAlign = HorizontalAlignment.Left,
                AnimatedGIF = false,
                Cursor = Cursors.Hand,
                Tag = index
            };
            var lblIcon = new Label
            {
                Text = Mdl2Char(mdl2IconCode),
                Font = new Font("Segoe MDL2 Assets", 11F),
                ForeColor = Color.FromArgb(203, 213, 225),
                AutoSize = false,
                Size = new Size(32, 40),
                Location = new Point(10, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            var lblText = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(btn.Width - 48, 40),
                Location = new Point(42, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btn.Controls.Add(lblIcon);
            btn.Controls.Add(lblText);
            btn.Resize += (_, _) => lblText.Width = btn.ClientSize.Width - 48;
            void forwardClick(object? s, EventArgs e) => NavButton_Click(btn, e);
            lblIcon.Click += forwardClick;
            lblText.Click += forwardClick;
            btn.Click += NavButton_Click;
            return btn;
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            var btn = sender as Guna2Button ?? (sender as Control)?.Parent as Guna2Button;
            if (btn?.Tag is int idx)
                SetSelectedNav(idx);
        }

        private void SetSelectedNav(int index)
        {
            for (var i = 0; i < _navButtons.Count; i++)
            {
                var sel = i == index;
                var b = _navButtons[i];
                b.FillColor = sel ? _accentBlue : Color.Transparent;
                b.ForeColor = Color.White;
                b.HoverState.FillColor = sel ? _accentBlue : Color.FromArgb(51, 65, 85);
                foreach (Control c in b.Controls)
                {
                    if (c is not Label l)
                        continue;
                    var isIcon = string.Equals(l.Font?.FontFamily.Name, "Segoe MDL2 Assets", StringComparison.OrdinalIgnoreCase);
                    l.ForeColor = isIcon
                        ? (sel ? Color.White : Color.FromArgb(203, 213, 225))
                        : Color.White;
                }
            }
        }

        private Control CreateSummaryCard(string title, string value, string sub, Color iconBg, string iconChar,
            Color? subForeColor = null)
        {
            var outer = new Panel
            {
                Padding = new Padding(0, 0, 12, 0),
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Padding = new Padding(18, 16, 18, 16),
                Margin = new Padding(0),
                ShadowDecoration = { Enabled = true, Depth = 6, Color = Color.FromArgb(35, 0, 0, 0) }
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(18, 16)
            };

            var lblVal = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(18, 38)
            };

            var subColor = subForeColor ?? (sub.StartsWith('+') ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68));
            var lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 9F),
                ForeColor = subColor,
                AutoSize = true,
                Location = new Point(18, 86)
            };

            var iconPanel = new Guna2Panel
            {
                Size = new Size(48, 48),
                FillColor = iconBg,
                BorderRadius = 10,
                Location = new Point(card.Width - 66, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var fontMdl = new Font("Segoe MDL2 Assets", 18F, FontStyle.Regular, GraphicsUnit.Point);
            var lblIcon = new Label
            {
                Text = iconChar,
                Font = fontMdl,
                ForeColor = Color.White,
                AutoSize = false,
                Size = iconPanel.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconPanel.Controls.Add(lblIcon);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblVal);
            card.Controls.Add(lblSub);
            card.Controls.Add(iconPanel);

            card.Resize += (_, _) =>
            {
                iconPanel.Left = card.ClientSize.Width - iconPanel.Width - 18;
            };

            outer.Controls.Add(card);
            return outer;
        }

        private Control CreateQuickCard(string title, string desc, string iconChar)
        {
            var wrap = new Panel
            {
                Padding = new Padding(0, 0, 12, 12),
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                MinimumSize = new Size(200, 76)
            };

            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Cursor = Cursors.Hand,
                ShadowDecoration = { Enabled = true, Depth = 3, Color = Color.FromArgb(25, 0, 0, 0) }
            };

            var iconBox = new Guna2Panel
            {
                Size = new Size(44, 44),
                Location = new Point(14, 16),
                FillColor = Color.FromArgb(224, 242, 254),
                BorderRadius = 10
            };
            var ic = new Label
            {
                Text = iconChar,
                Font = new Font("Segoe MDL2 Assets", 16F),
                ForeColor = Color.FromArgb(14, 165, 233),
                AutoSize = false,
                Size = iconBox.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconBox.Controls.Add(ic);

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(68, 16)
            };
            var lblD = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                Size = new Size(280, 36),
                Location = new Point(68, 36)
            };

            card.Controls.Add(iconBox);
            card.Controls.Add(lblT);
            card.Controls.Add(lblD);
            wrap.Controls.Add(card);

            return wrap;
        }

        private Control CreateAlertRow(Color accent, string iconChar, string title, string detail, string when)
        {
            var row = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 12),
                Width = 760
            };

            var iconCircle = new Guna2Panel
            {
                Size = new Size(40, 40),
                Location = new Point(4, 4),
                FillColor = Blend(accent, Color.White, 0.88f),
                BorderRadius = 20
            };

            var li = new Label
            {
                Text = iconChar,
                Font = new Font("Segoe MDL2 Assets", 14F),
                ForeColor = accent,
                AutoSize = false,
                Size = iconCircle.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconCircle.Controls.Add(li);

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = _textDark,
                Location = new Point(52, 4),
                AutoSize = true
            };
            var lblD = new Label
            {
                Text = detail,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(52, 24),
                AutoSize = true,
                MaximumSize = new Size(620, 0)
            };
            var lblW = new Label
            {
                Text = when,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = _textMuted,
                Location = new Point(52, 44),
                AutoSize = true
            };

            row.Controls.Add(iconCircle);
            row.Controls.Add(lblT);
            row.Controls.Add(lblD);
            row.Controls.Add(lblW);

            return row;
        }

        private static Color Blend(Color c, Color with, float amount)
        {
            var r = (int)(c.R * (1 - amount) + with.R * amount);
            var g = (int)(c.G * (1 - amount) + with.G * amount);
            var b = (int)(c.B * (1 - amount) + with.B * amount);
            return Color.FromArgb(r, g, b);
        }
    }
}
