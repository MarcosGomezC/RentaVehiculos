using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RentaVehiculo.UI.Usuario
{
    public partial class UsuarioForm : Form
    {
        public UsuarioForm()
        {
            InitializeComponent();
        }

        private void menuUsuarioCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
