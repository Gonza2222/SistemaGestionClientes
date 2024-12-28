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
    public partial class frmConsultarCliente : Form
    {
        public frmConsultarCliente()
        {
            InitializeComponent();
        }
        clsSocio objSocio = new clsSocio();

        private void frmConsultarCliente_Load(object sender, EventArgs e)
        {
            objSocio.ListarSocio(cmbCliente);
        }

        private void cmdSeleccionar_Click(object sender, EventArgs e)
        {
            objSocio.ConsultarSocio(cmbCliente);
            lblDni.Text = objSocio.DNI.ToString();
            lblDomicilio.Text = objSocio.Direccion;
            lblBarrio.Text = objSocio.NombreBarrio;
            lblActividad.Text = objSocio.NombreActividad;
            lblDeuda.Text = objSocio.Deuda.ToString("$0.00");

        }
    }
}
