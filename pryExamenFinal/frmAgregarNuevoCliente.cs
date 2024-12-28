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
    public partial class frmAgregarNuevoCliente : Form
    {
        public frmAgregarNuevoCliente()
        {
            InitializeComponent();
        }
        clsBarrio objBarrio = new clsBarrio();
        clsActividad objActividad = new clsActividad();
        clsSocio objSocio = new clsSocio();
        private void frmAgregarNuevoCliente_Load(object sender, EventArgs e)
        {
            objActividad.ListarActividad(cmbActividad);
            objBarrio.ListarBarrio(cmbBarrio);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            objSocio.Nombre = txtNombre.Text;
            objSocio.DNI = Convert.ToInt32(txtDni.Text);
            objSocio.Direccion = txtDireccion.Text;           
            objSocio.idBarrio = Convert.ToInt32(cmbBarrio.SelectedValue);
            objSocio.idActividad = Convert.ToInt32(cmbActividad.SelectedValue);
            objSocio.AgregarNuevoSocio();
            MessageBox.Show("Socio cargado correctamente!!");
            txtNombre.Text = "";
            txtDni.Text = "";
            txtDireccion.Text = "";
            cmbBarrio.SelectedIndex = 0;    
            cmbActividad.SelectedIndex = 0;

        }

        private void ControlarCaja()
        {
            if (txtNombre.Text != "" && txtDireccion.Text != "" && txtDni.Text != "")
            {
                btnCargar.Enabled = true;
            }
            else { btnCargar.Enabled = false; }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            ControlarCaja();
        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {
            ControlarCaja();
        }

        private void txtDni_TextChanged(object sender, EventArgs e)
        {
            ControlarCaja();
        }
    }
}
