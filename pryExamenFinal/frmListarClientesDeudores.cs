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
    public partial class frmListarClientesDeudores : Form
    {
        public frmListarClientesDeudores()
        {
            InitializeComponent();
        }
        clsSocio objSocio = new clsSocio();
        private void cmdListar_Click(object sender, EventArgs e)
        {
            objSocio.ListarClientesDeudores(Grilla);
            lblMayorDeuda.Text = objSocio.DeudaMayor.ToString("$0.00");
            lblMenorDeuda.Text = objSocio.DeudaMenor.ToString("$0.00");
            lblPromedio.Text = objSocio.Promedio.ToString("$0.00");
            lblTotal.Text = objSocio.Deuda.ToString("$0.00");
            cmdImprimir.Enabled = true;
            cmdReporte.Enabled = true;
        }

        private void cmdReporte_Click(object sender, EventArgs e)
        {
            SaveFileDialog cuadroDialogo = new SaveFileDialog();
            cuadroDialogo.Title = "Seleccione carpeta y escriba el nombre de archivo";
            cuadroDialogo.RestoreDirectory = true;
            cuadroDialogo.Filter = "Archivos separados por coma (*.csv)|*.csv";
            cuadroDialogo.ShowDialog();
            objSocio.ReporteClientesDeudores(cuadroDialogo.FileName);
            MessageBox.Show("Reporte generado con éxito");
        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            {
                prtVentana.ShowDialog();
                prtDocumento.PrinterSettings = prtVentana.PrinterSettings;
                prtDocumento.Print();
                MessageBox.Show("Reporte impreso correctamente");
            }
        }

        private void prtDocumento_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            objSocio.ImprimirReporteDeudores(e);
        }
    }
}
