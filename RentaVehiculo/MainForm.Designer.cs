namespace RentaVehiculo
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStripMain = new MenuStrip();
            menuArchivo = new ToolStripMenuItem();
            menuArchivoSalir = new ToolStripMenuItem();
            toolStripMain = new ToolStrip();
            toolBtnNuevo = new ToolStripButton();
            toolBtnGuardar = new ToolStripButton();
            toolBtnSalir = new ToolStripButton();
            menuStripMain.SuspendLayout();
            toolStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new ToolStripItem[] { menuArchivo });
            menuStripMain.Location = new Point(0, 0);
            menuStripMain.Name = "menuStripMain";
            menuStripMain.Size = new Size(900, 24);
            menuStripMain.TabIndex = 0;
            menuStripMain.Text = "menuStripMain";
            // 
            // menuArchivo
            // 
            menuArchivo.DropDownItems.AddRange(new ToolStripItem[] { menuArchivoSalir });
            menuArchivo.Name = "menuArchivo";
            menuArchivo.Size = new Size(60, 20);
            menuArchivo.Text = "&Archivo";
            // 
            // menuArchivoSalir
            // 
            menuArchivoSalir.Name = "menuArchivoSalir";
            menuArchivoSalir.Size = new Size(96, 22);
            menuArchivoSalir.Text = "&Salir";
            menuArchivoSalir.Click += menuArchivoSalir_Click;
            // 
            // toolStripMain
            // 
            toolStripMain.Items.AddRange(new ToolStripItem[] { toolBtnNuevo, toolBtnGuardar, toolBtnSalir });
            toolStripMain.Location = new Point(0, 24);
            toolStripMain.Name = "toolStripMain";
            toolStripMain.Size = new Size(900, 25);
            toolStripMain.TabIndex = 1;
            toolStripMain.Text = "toolStripMain";
            // 
            // toolBtnNuevo
            // 
            toolBtnNuevo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolBtnNuevo.Name = "toolBtnNuevo";
            toolBtnNuevo.Size = new Size(46, 22);
            toolBtnNuevo.Text = "Nuevo";
            // 
            // toolBtnGuardar
            // 
            toolBtnGuardar.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolBtnGuardar.Name = "toolBtnGuardar";
            toolBtnGuardar.Size = new Size(53, 22);
            toolBtnGuardar.Text = "Guardar";
            // 
            // toolBtnSalir
            // 
            toolBtnSalir.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolBtnSalir.Name = "toolBtnSalir";
            toolBtnSalir.Size = new Size(33, 22);
            toolBtnSalir.Text = "Salir";
            toolBtnSalir.Click += menuArchivoSalir_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(toolStripMain);
            Controls.Add(menuStripMain);
            MainMenuStrip = menuStripMain;
            Name = "MainForm";
            Text = "RentaVehiculo";
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            toolStripMain.ResumeLayout(false);
            toolStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripMain;
        private ToolStripMenuItem menuArchivo;
        private ToolStripMenuItem menuArchivoSalir;
        private ToolStrip toolStripMain;
        private ToolStripButton toolBtnNuevo;
        private ToolStripButton toolBtnGuardar;
        private ToolStripButton toolBtnSalir;
    }
}
