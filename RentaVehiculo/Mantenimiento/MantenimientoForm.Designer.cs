namespace RentaVehiculo.UI.Mantenimientos
{
    partial class MantenimientoForm
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
            numIdVehiculo = new NumericUpDown();
            numTipo = new NumericUpDown();
            numCosto = new NumericUpDown();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numKm = new NumericUpDown();
            numProx = new NumericUpDown();
            numEstado = new NumericUpDown();
            txtProveedor = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            numIdVehiculo.Maximum = 1000000;
            numIdVehiculo.Location = new Point(180, 24);
            numTipo.Location = new Point(180, 64);
            numCosto.DecimalPlaces = 2;
            numCosto.Location = new Point(180, 104);
            numCosto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            dtpInicio.Location = new Point(180, 144);
            dtpFin.Location = new Point(180, 184);
            numKm.Location = new Point(180, 224);
            numKm.Maximum = 10000000;
            numProx.Location = new Point(180, 264);
            numProx.Maximum = 10000000;
            numEstado.Location = new Point(180, 304);
            txtProveedor.Location = new Point(180, 344);
            txtProveedor.Width = 260;
            btnGuardar.Location = new Point(180, 396);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(305, 396);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Id vehículo:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Tipo:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Costo:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Inicio:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Fin:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Km mant.:", Location = new Point(20, 227), AutoSize = true },
                new Label { Text = "Próx. km:", Location = new Point(20, 267), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 307), AutoSize = true },
                new Label { Text = "Proveedor:", Location = new Point(20, 347), AutoSize = true },
                numIdVehiculo, numTipo, numCosto, dtpInicio, dtpFin, numKm, numProx, numEstado, txtProveedor, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(460, 460);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mantenimiento";
            ResumeLayout(false);
        }

        private NumericUpDown numIdVehiculo;
        private NumericUpDown numTipo;
        private NumericUpDown numCosto;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numKm;
        private NumericUpDown numProx;
        private NumericUpDown numEstado;
        private TextBox txtProveedor;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
