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
    public partial class frmBuscarCliente : Form
    {
        public frmBuscarCliente()
        {
            InitializeComponent();
        }
        clsSocio objSocio = new clsSocio();

        private void cmdBuscar_Click(object sender, EventArgs e)
        {
            Int32 idSocio = Convert.ToInt32(txtDNI.Text);           
            objSocio.BuscarCliente(idSocio);
            if (objSocio.DNI == 0)
            {
                MessageBox.Show("El DNI ingresado no corresponde a ningún socio");
                txtDNI.Text = "";
                lblNombre.Text = "";
                lblDNI.Text = "";
                lblBarrio.Text = "";
                lblDireccion.Text = "";
                lblActividad.Text = "";
                txtDeuda.Text = "";
            }
            else 
            {
                lblNombre.Text = objSocio.Nombre;
                lblDNI.Text = objSocio.DNI.ToString();
                lblBarrio.Text = objSocio.NombreBarrio;
                lblDireccion.Text = objSocio.Direccion;
                lblActividad.Text = objSocio.NombreActividad;
                txtDeuda.Text = objSocio.Deuda.ToString("$0.00"); 
                cmdEliminar.Enabled = true;
                cmdModificar.Enabled = true;
            }
        }

        private void ControlarCaja()
        {
            if (txtDNI.Text != "")
            {
                cmdBuscar.Enabled = true;
            }
            else { cmdBuscar.Enabled = false; }
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
            ControlarCaja();
        }

        private void cmdModificar_Click(object sender, EventArgs e)
        {
            cmdEliminar.Enabled = false;
            cmdModificar.Enabled = false;
            cmdGuardar.Enabled = true;
            txtDeuda.ReadOnly = false;
        }

        private void cmdGuardar_Click(object sender, EventArgs e)
        {
            Int32 Dni = Convert.ToInt32(txtDNI.Text);
            objSocio.Nombre = lblNombre.Text;
            objSocio.Direccion = lblDireccion.Text;
            objSocio.Deuda = Convert.ToDecimal(txtDeuda.Text);
            objSocio.ModificarCliente(Dni);
            MessageBox.Show("Dato modificado correctamente");
            cmdGuardar.Enabled = false;
            cmdEliminar.Enabled = true;
            cmdModificar.Enabled = true;
            txtDeuda.ReadOnly = true;
            LimpiarCajasTexto();
            cmdEliminar.Enabled = false;
            cmdModificar.Enabled = false;
        }
         private void LimpiarCajasTexto()
        {
            txtDNI.Text = "";
            lblNombre.Text = "";
            lblActividad.Text = "";
            lblBarrio.Text = "";
            lblDireccion.Text = "";
            txtDeuda.Text = "";
            lblDNI.Text = "";
        }

        private void cmdEliminar_Click(object sender, EventArgs e)
        {
            Int32 Dni = Convert.ToInt32(txtDNI.Text);
            objSocio.DNI = Convert.ToInt32(txtDNI.Text);
            objSocio.EliminarClientes(Dni);
            MessageBox.Show("Dato eliminado correctamente");
            LimpiarCajasTexto();
            cmdEliminar.Enabled = false;
            cmdModificar.Enabled = false;
        }

    }
}
