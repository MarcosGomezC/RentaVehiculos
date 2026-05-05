namespace RentaVehiculo.UI.Mantenimientos
{
    partial class MantenimientoList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            panelToolbar = new Panel();
            button1 = new Button();
            btnModificar = new Button();
            label1 = new Label();
            btnEliminar = new Button();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            panelToolbar.Controls.Add(btnEliminar);
            panelToolbar.Controls.Add(btnModificar);
            panelToolbar.Controls.Add(button1);
            panelToolbar.Controls.Add(label1);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Padding = new Padding(0, 0, 0, 8);
            panelToolbar.Size = new Size(1040, 116);
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label1.Location = new Point(20, 14);
            label1.Text = "Mantenimientos";
            button1.Font = new Font("Segoe UI", 9F);
            button1.Location = new Point(20, 58);
            button1.Size = new Size(132, 42);
            button1.Text = "Nuevo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            btnModificar.Font = new Font("Segoe UI", 9F);
            btnModificar.Location = new Point(164, 58);
            btnModificar.Size = new Size(132, 42);
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Font = new Font("Segoe UI", 9F);
            btnEliminar.Location = new Point(308, 58);
            btnEliminar.Size = new Size(132, 42);
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ClientSize = new Size(1040, 620);
            Controls.Add(dataGridView1);
            Controls.Add(panelToolbar);
            MinimumSize = new Size(900, 460);
            Name = "MantenimientoList";
            Text = "Mantenimientos";
            Load += MantenimientoList_Load;
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        private DataGridView dataGridView1;
        private Panel panelToolbar;
        private Button button1;
        private Button btnModificar;
        private Label label1;
        private Button btnEliminar;
    }
}
