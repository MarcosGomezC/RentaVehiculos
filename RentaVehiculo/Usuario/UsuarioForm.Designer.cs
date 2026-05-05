namespace RentaVehiculo.UI.Usuario
{
    partial class UsuarioForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStripUsuario = new MenuStrip();
            menuUsuario = new ToolStripMenuItem();
            menuUsuarioNuevo = new ToolStripMenuItem();
            menuUsuarioGuardar = new ToolStripMenuItem();
            menuUsuarioCerrar = new ToolStripMenuItem();
            toolStripAccesos = new ToolStrip();
            toolBtnNuevo = new ToolStripButton();
            toolBtnGuardar = new ToolStripButton();
            toolBtnCerrar = new ToolStripButton();
            menuStripUsuario.SuspendLayout();
            toolStripAccesos.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripUsuario
            // 
            menuStripUsuario.Items.AddRange(new ToolStripItem[] { menuUsuario });
            menuStripUsuario.Location = new Point(0, 0);
            menuStripUsuario.Name = "menuStripUsuario";
            menuStripUsuario.Size = new Size(900, 24);
            menuStripUsuario.TabIndex = 0;
            menuStripUsuario.Text = "menuStripUsuario";
            // 
            // menuUsuario
            // 
            menuUsuario.DropDownItems.AddRange(new ToolStripItem[] { menuUsuarioNuevo, menuUsuarioGuardar, menuUsuarioCerrar });
            menuUsuario.Name = "menuUsuario";
            menuUsuario.Size = new Size(64, 20);
            menuUsuario.Text = "&Usuario";
            // 
            // menuUsuarioNuevo
            // 
            menuUsuarioNuevo.Name = "menuUsuarioNuevo";
            menuUsuarioNuevo.Size = new Size(116, 22);
            menuUsuarioNuevo.Text = "&Nuevo";
            // 
            // menuUsuarioGuardar
            // 
            menuUsuarioGuardar.Name = "menuUsuarioGuardar";
            menuUsuarioGuardar.Size = new Size(116, 22);
            menuUsuarioGuardar.Text = "&Guardar";
            // 
            // menuUsuarioCerrar
            // 
            menuUsuarioCerrar.Name = "menuUsuarioCerrar";
            menuUsuarioCerrar.Size = new Size(116, 22);
            menuUsuarioCerrar.Text = "&Cerrar";
            menuUsuarioCerrar.Click += menuUsuarioCerrar_Click;
            // 
            // toolStripAccesos
            // 
            toolStripAccesos.Items.AddRange(new ToolStripItem[] { toolBtnNuevo, toolBtnGuardar, toolBtnCerrar });
            toolStripAccesos.Location = new Point(0, 24);
            toolStripAccesos.Name = "toolStripAccesos";
            toolStripAccesos.Size = new Size(900, 25);
            toolStripAccesos.TabIndex = 1;
            toolStripAccesos.Text = "toolStripAccesos";
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
            // toolBtnCerrar
            // 
            toolBtnCerrar.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolBtnCerrar.Name = "toolBtnCerrar";
            toolBtnCerrar.Size = new Size(45, 22);
            toolBtnCerrar.Text = "Cerrar";
            toolBtnCerrar.Click += menuUsuarioCerrar_Click;
            // 
            // UsuarioForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(toolStripAccesos);
            Controls.Add(menuStripUsuario);
            MainMenuStrip = menuStripUsuario;
            Name = "UsuarioForm";
            Text = "Formulario de Usuario";
            menuStripUsuario.ResumeLayout(false);
            menuStripUsuario.PerformLayout();
            toolStripAccesos.ResumeLayout(false);
            toolStripAccesos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripUsuario;
        private ToolStripMenuItem menuUsuario;
        private ToolStripMenuItem menuUsuarioNuevo;
        private ToolStripMenuItem menuUsuarioGuardar;
        private ToolStripMenuItem menuUsuarioCerrar;
        private ToolStrip toolStripAccesos;
        private ToolStripButton toolBtnNuevo;
        private ToolStripButton toolBtnGuardar;
        private ToolStripButton toolBtnCerrar;
    }
}