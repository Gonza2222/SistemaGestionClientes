using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryExamenFinal
{
    public partial class frmListadoDeClientesDeUnBarrio : Form
    {
        public frmListadoDeClientesDeUnBarrio()
        {
            InitializeComponent();
        }
        clsBarrio objBarrio = new clsBarrio();
        clsSocio objSocio = new clsSocio();
        private void frmListadoDeClientesDeUnBarrio_Load(object sender, EventArgs e)
        {
            objBarrio.ListarBarrio(cmbBarrio);
        }

        private void cmdListar_Click(object sender, EventArgs e)
        {
            Int32 idBarrio = Convert.ToInt32(cmbBarrio.SelectedValue);
            LimpiarEtiquetas();
            objSocio.ListarClienteBarrio(Grilla, idBarrio);           
            lblCantidadClientes.Text = objSocio.CantidadClientes.ToString();
            lblTotal.Text = objSocio.Deuda.ToString("$0.00");
            cmdImprimir.Enabled = true;
            cmdGenerarReporte.Enabled = true;
        }

        private void LimpiarEtiquetas()
        {
            lblTotal.Text = "";
            lblCantidadClientes.Text = "";
        }

        private void cmdGenerarReporte_Click(object sender, EventArgs e)
        {
            Int32 idBarrio = Convert.ToInt32(cmbBarrio.SelectedValue);
            SaveFileDialog cuadroDialogo = new SaveFileDialog();
            cuadroDialogo.Title = "Seleccione carpeta y escriba el nombre de archivo";
            cuadroDialogo.RestoreDirectory = true;
            cuadroDialogo.Filter = "Archivos separados por coma (*.csv)|*.csv";
            cuadroDialogo.ShowDialog();
            objSocio.ReporteClientesBarrio(cuadroDialogo.FileName, idBarrio);
            MessageBox.Show("Reporte generado con éxito");
        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            prtVentana.ShowDialog();
            prtDocumento.PrinterSettings = prtVentana.PrinterSettings;
            prtDocumento.Print();
            MessageBox.Show("Reporte impreso correctamente");
        }

        private void prtDocumento_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Int32 idBarrio = Convert.ToInt32(cmbBarrio.SelectedValue);
            objSocio.ImprimirReporteBarrio(e, idBarrio);
        }
    }
}
