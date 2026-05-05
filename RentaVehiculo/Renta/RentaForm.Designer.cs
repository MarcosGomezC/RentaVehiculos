namespace RentaVehiculo.UI.Rentas
{
    partial class RentaForm
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
            numIdEmpleado = new NumericUpDown();
            numSucRec = new NumericUpDown();
            numSucEnt = new NumericUpDown();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numKmIni = new NumericUpDown();
            numCostoDia = new NumericUpDown();
            numDias = new NumericUpDown();
            numCostoTot = new NumericUpDown();
            numDep = new NumericUpDown();
            numEstado = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            numIdCliente.Location = new Point(200, 24);
            numIdCliente.Maximum = 1000000;
            numIdVehiculo.Location = new Point(200, 64);
            numIdVehiculo.Maximum = 1000000;
            numIdEmpleado.Location = new Point(200, 104);
            numIdEmpleado.Maximum = 1000000;
            numSucRec.Location = new Point(200, 144);
            numSucRec.Maximum = 1000000;
            numSucEnt.Location = new Point(200, 184);
            numSucEnt.Maximum = 1000000;
            dtpInicio.Location = new Point(200, 224);
            dtpFin.Location = new Point(200, 264);
            numKmIni.Location = new Point(200, 304);
            numKmIni.Maximum = 10000000;
            numCostoDia.Location = new Point(200, 344);
            numCostoDia.DecimalPlaces = 2;
            numCostoDia.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDias.Location = new Point(200, 384);
            numDias.Maximum = 1000;
            numCostoTot.Location = new Point(200, 424);
            numCostoTot.DecimalPlaces = 2;
            numCostoTot.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDep.Location = new Point(200, 464);
            numDep.DecimalPlaces = 2;
            numDep.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numEstado.Location = new Point(200, 504);
            btnGuardar.Location = new Point(200, 556);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(325, 556);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Id cliente:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Id vehículo:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Id empleado:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Sucursal recogida:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Sucursal entrega:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Inicio:", Location = new Point(20, 227), AutoSize = true },
                new Label { Text = "Fin programada:", Location = new Point(20, 267), AutoSize = true },
                new Label { Text = "Km inicial:", Location = new Point(20, 307), AutoSize = true },
                new Label { Text = "Costo/día:", Location = new Point(20, 347), AutoSize = true },
                new Label { Text = "Días:", Location = new Point(20, 387), AutoSize = true },
                new Label { Text = "Costo total:", Location = new Point(20, 427), AutoSize = true },
                new Label { Text = "Depósito:", Location = new Point(20, 467), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 507), AutoSize = true },
                numIdCliente, numIdVehiculo, numIdEmpleado, numSucRec, numSucEnt,
                dtpInicio, dtpFin, numKmIni, numCostoDia, numDias, numCostoTot, numDep, numEstado,
                btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(500, 520);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Renta";
            ResumeLayout(false);
        }

        private NumericUpDown numIdCliente;
        private NumericUpDown numIdVehiculo;
        private NumericUpDown numIdEmpleado;
        private NumericUpDown numSucRec;
        private NumericUpDown numSucEnt;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numKmIni;
        private NumericUpDown numCostoDia;
        private NumericUpDown numDias;
        private NumericUpDown numCostoTot;
        private NumericUpDown numDep;
        private NumericUpDown numEstado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
