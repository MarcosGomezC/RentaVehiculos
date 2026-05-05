namespace RentaVehiculo.UI.Facturas
{
    partial class FacturaForm
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
            numIdRenta = new NumericUpDown();
            txtNumero = new TextBox();
            numSub = new NumericUpDown();
            numImp = new NumericUpDown();
            numTot = new NumericUpDown();
            numMetodo = new NumericUpDown();
            numEstado = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            numIdRenta.Maximum = 1000000;
            numIdRenta.Location = new Point(200, 24);
            txtNumero.Location = new Point(200, 64);
            txtNumero.Width = 260;
            numSub.DecimalPlaces = numImp.DecimalPlaces = numTot.DecimalPlaces = 2;
            numSub.Location = new Point(200, 104);
            numImp.Location = new Point(200, 144);
            numTot.Location = new Point(200, 184);
            numSub.Maximum = numImp.Maximum = numTot.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMetodo.Location = new Point(200, 224);
            numEstado.Location = new Point(200, 264);
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
                new Label { Text = "Id renta:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Número factura:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Subtotal:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Impuestos:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Total:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Método pago:", Location = new Point(20, 227), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 267), AutoSize = true },
                numIdRenta, txtNumero, numSub, numImp, numTot, numMetodo, numEstado, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(500, 380);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Factura";
            ResumeLayout(false);
        }

        private NumericUpDown numIdRenta;
        private TextBox txtNumero;
        private NumericUpDown numSub;
        private NumericUpDown numImp;
        private NumericUpDown numTot;
        private NumericUpDown numMetodo;
        private NumericUpDown numEstado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
