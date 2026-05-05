namespace RentaVehiculo.UI.Reservas
{
    partial class ReservaForm
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
            numIdCliente = new NumericUpDown();
            numIdVehiculo = new NumericUpDown();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numMonto = new NumericUpDown();
            numEstado = new NumericUpDown();
            chkDepPagado = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            numIdCliente.Location = new Point(200, 24);
            numIdCliente.Maximum = 1000000;
            numIdVehiculo.Location = new Point(200, 64);
            numIdVehiculo.Maximum = 1000000;
            dtpInicio.Location = new Point(200, 104);
            dtpFin.Location = new Point(200, 144);
            numMonto.DecimalPlaces = 2;
            numMonto.Location = new Point(200, 184);
            numMonto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numEstado.Location = new Point(200, 224);
            chkDepPagado.Location = new Point(200, 264);
            chkDepPagado.Text = "Depósito pagado";
            btnGuardar.Location = new Point(200, 316);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(325, 316);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Id cliente:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Id vehículo:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Inicio reserva:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Fin reserva:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Monto depósito:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 227), AutoSize = true },
                numIdCliente, numIdVehiculo, dtpInicio, dtpFin, numMonto, numEstado, chkDepPagado, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(500, 380);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reserva";
            ResumeLayout(false);
        }

        private NumericUpDown numIdCliente;
        private NumericUpDown numIdVehiculo;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numMonto;
        private NumericUpDown numEstado;
        private CheckBox chkDepPagado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
