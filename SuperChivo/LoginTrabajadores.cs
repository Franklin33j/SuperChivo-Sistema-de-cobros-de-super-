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
    public partial class LoginTrabajadores : Form
    {
        public LoginTrabajadores()
        {
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.KeyPreview = true;
            ValidarErrores();
        }
        private void LoginTrabajadores_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            } 
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(txtId.TextLength.ToString());
            ConexionBD conexion = new ConexionBD();
            conexion.Conectar();
            var info = conexion.ConsultaUsuarios(txtId.Text, txtPassword.Text);
            //Si un campo es null es porque no se ha encontrado las credenciales introducidas
            if (txtId != null)
            {
                MessageBox.Show("Error en los datos introducidos", "Datos de usuario Invalidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Clear();
                txtPassword.Clear();
                txtId.Focus();
            }
        }
        private void ValidarErrores()
        {
            if (txtId.TextLength < 5) Errores.SetError(txtId, "Id con menos de 5 valores");
            if (txtId.TextLength == 5) Errores.Clear();
            if (txtPassword.TextLength < 5) Errores.SetError(txtPassword, "Contraceña con menos de 5 valores") ;
            if (txtPassword.TextLength == 5) Errores.Clear();
        }

        private void txtId_TextChanged(object sender, EventArgs e)=> ValidarErrores();
        private void txtPassword_TextChanged(object sender, EventArgs e) => ValidarErrores();
    }
}
