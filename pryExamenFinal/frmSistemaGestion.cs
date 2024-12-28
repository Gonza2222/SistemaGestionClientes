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
    public partial class frmSistemaGestion : Form
    {
        public frmSistemaGestion()
        {
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acercaDelDesarrolladorDelSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAcercaDe Ventana = new frmAcercaDe();
            Ventana.ShowDialog();
        }

        private void agregarNuevosSociosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAgregarNuevoCliente Ventana = new frmAgregarNuevoCliente();
            Ventana.ShowDialog();
        }

        private void buscarSocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBuscarCliente Ventana = new frmBuscarCliente();
            Ventana.ShowDialog();
        }

        private void consultaDeUnSocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConsultarCliente Ventana = new frmConsultarCliente();
            Ventana.ShowDialog();
        }

        private void listadoDeTodosLosSociosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarClientes Ventana = new frmListarClientes();    
            Ventana.ShowDialog();
        }

        private void listadoDeSociosDeudoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarClientesDeudores Ventana = new frmListarClientesDeudores();
            Ventana.ShowDialog();
        }

        private void listadoDeSociosDeUnaActividadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarClientesPorActividad Ventana = new frmListarClientesPorActividad();
            Ventana.ShowDialog();
        }

        private void listadoDeSociosDeUnBarrioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListadoDeClientesDeUnBarrio Ventana = new frmListadoDeClientesDeUnBarrio();
            Ventana.ShowDialog();
        }
    }
}
