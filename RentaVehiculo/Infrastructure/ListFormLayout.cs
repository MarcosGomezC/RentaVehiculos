namespace RentaVehiculo.UI.Infrastructure;

/// <summary>
/// Estilo común para formularios de listado (rejilla legible, cabeceras y filas con aire).
/// </summary>
public static class ListFormLayout
{
    public static void ConfigureDataGrid(DataGridView dgv)
    {
        dgv.BorderStyle = BorderStyle.None;
        dgv.BackgroundColor = Color.White;
        dgv.GridColor = Color.FromArgb(226, 232, 240);
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.RowHeadersVisible = false;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.MultiSelect = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.ColumnHeadersHeight = 42;
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgv.RowTemplate.Height = 32;
        dgv.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 10, 10, 10);
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (col.MinimumWidth < 72)
                col.MinimumWidth = 72;
            if (col.FillWeight <= 0)
                col.FillWeight = 100;
        }
    }
}
