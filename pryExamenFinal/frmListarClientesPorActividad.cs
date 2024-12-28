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
    public partial class frmListarClientesPorActividad : Form
    {
        public frmListarClientesPorActividad()
        {
            InitializeComponent();
        }
        clsActividad objActividad = new clsActividad();
        clsSocio objSocio = new clsSocio();
        private void frmListarClientesPorActividad_Load(object sender, EventArgs e)
        {
            objActividad.ListarActividad(cmbActividad);
        }

        private void cmdListar_Click(object sender, EventArgs e)
        {
            Int32 idCiudad = Convert.ToInt32(cmbActividad.SelectedValue);
            LimpiarEtiquetas();
            objSocio.ListarClienteActividad(Grilla, idCiudad);
            lblTotal.Text = objSocio.Deuda.ToString("$0.00");
            lblPromedio.Text = objSocio.Promedio.ToString("$0.00");
            lblMayorDeuda.Text = objSocio.DeudaMayor.ToString("$0.00");
            lblMenorDeuda.Text = objSocio.DeudaMenor.ToString("$0.00");
            cmdImprimir.Enabled = true;
            cmdGenerarReporte.Enabled = true;
        }
        private void LimpiarEtiquetas()
        {
            lblTotal.Text = "";
            lblPromedio.Text = "";
            lblMayorDeuda.Text = "";
            lblMenorDeuda.Text = "";
        }

        private void cmdGenerarReporte_Click(object sender, EventArgs e)
        {
            Int32 idActividad = Convert.ToInt32(cmbActividad.SelectedValue);
            SaveFileDialog cuadroDialogo = new SaveFileDialog();
            cuadroDialogo.Title = "Seleccione carpeta y escriba el nombre de archivo";
            cuadroDialogo.RestoreDirectory = true;
            cuadroDialogo.Filter = "Archivos separados por coma (*.csv)|*.csv";
            cuadroDialogo.ShowDialog();
            objSocio.ReporteClientesActividad(cuadroDialogo.FileName, idActividad);
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
            Int32 idActividad = Convert.ToInt32(cmbActividad.SelectedValue);
            objSocio.ImprimirReporteActividad(e, idActividad);           
        }
    }
}
