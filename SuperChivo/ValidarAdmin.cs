using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperChivo
{
    public partial class ValidarAdmin : Form
    {
        static bool estado = false;
        ConexionBD conexion;

       
        public ValidarAdmin()
        {
            InitializeComponent();
            txtID.Focus();
        }
        public bool Estado
        {
            get { return estado; }
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            conexion = new ConexionBD();
            conexion.Conectar();
            var datos =conexion.ConsultaUsuarios(txtID.Text, txtContracena.Text);
            if (datos.nombre != null)
            {
                if (datos.administrador)
                {
                 
                    estado = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No eres Administrador no tienes permisos para realizar esta opcion","Permisos Denegados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtContracena.Clear();
                    txtID.Clear();
                    txtID.Focus();
                }
            }
            else
            {
                MessageBox.Show("Error en los Datos introducidos","Datos Incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContracena.Clear();
                txtID.Clear();
                txtID.Focus();
            }
        }

        private void bntCerrar_Click(object sender, EventArgs e) => this.Close();

        private void ValidarAdmin_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter) SendKeys.Send("{TAB}");
        }
    }
}
