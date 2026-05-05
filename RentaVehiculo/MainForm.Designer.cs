namespace RentaVehiculo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            designTimeHint = new Panel();
            labelDesignTimeNote = new Label();
            SuspendLayout();
            // 
            // designTimeHint
            // 
            designTimeHint.Controls.Add(labelDesignTimeNote);
            designTimeHint.Dock = DockStyle.Fill;
            designTimeHint.Location = new Point(0, 0);
            designTimeHint.Name = "designTimeHint";
            designTimeHint.Size = new Size(1293, 697);
            designTimeHint.TabIndex = 0;
            // 
            // labelDesignTimeNote
            // 
            labelDesignTimeNote.Dock = DockStyle.Fill;
            labelDesignTimeNote.Font = new Font("Segoe UI", 10F);
            labelDesignTimeNote.ForeColor = Color.FromArgb(71, 85, 105);
            labelDesignTimeNote.Location = new Point(0, 0);
            labelDesignTimeNote.Name = "labelDesignTimeNote";
            labelDesignTimeNote.Size = new Size(1293, 697);
            labelDesignTimeNote.TabIndex = 0;
            labelDesignTimeNote.Text = "Vista de diseño\r\n\r\nEl panel principal se genera en código en tiempo de ejecución (MainForm.BuildDashboard).\r\nVisual Studio no ejecuta ese código en esta vista previa.\r\n\r\nPulse F5 o «Iniciar» para ver el dashboard completo.";
            labelDesignTimeNote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1293, 697);
            Controls.Add(designTimeHint);
            MinimumSize = new Size(1024, 640);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RentCar Pro";
            ResumeLayout(false);
        }

        private Panel designTimeHint;
        private Label labelDesignTimeNote;
    }
}
