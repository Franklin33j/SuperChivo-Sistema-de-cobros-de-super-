using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//agregado
using System.Data.SqlClient;

namespace SuperChivo
{
    public partial class VerificarProducto : Form
    {
        ConexionBD conexion;
        public VerificarProducto()
        {
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void BuscarProducto()
        {
            conexion = new ConexionBD();
            conexion.Conectar();
            DataTable nProductos = conexion.NProductos(txtCodigo.Text);
            string codigo = "", producto = "", interno = "", descuento = "";
            double precio = 0;

            if (nProductos!= null && nProductos.Rows.Count>0 )
            {
                for(byte filas= 0; filas<nProductos.Rows.Count; filas++)
                {
                   
                    interno = nProductos.Rows[filas][3].ToString();
                    codigo = nProductos.Rows[filas][1].ToString();
                    producto=nProductos.Rows[filas][2].ToString();
                    descuento= nProductos.Rows[filas][4].ToString();
                    precio = Convert.ToDouble(nProductos.Rows[filas][5]);
                    
                    IntroducirDatos(interno, producto, codigo, descuento, precio);
                }
                SeleccionarCoincidencia();
            }
            else
            {
                MessageBox.Show("Producto no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexion.Desconectar();
        }
        private void IntroducirDatos(string interno, string producto, string codigo, string descuento, double precio )
        {
            ListViewItem a = new ListViewItem(interno);
            a.SubItems.Add(producto);
            a.SubItems.Add(codigo);
            a.SubItems.Add(descuento+"%");
            a.SubItems.Add(precio.ToString("C"));
            mostrarProductos.Items.Add(a);
        }
        private void SeleccionarCoincidencia()
        {
            int indice = mostrarProductos.Items.Count;
            for(byte i=0; i<indice; i++)
            {
                if (mostrarProductos.Items[i].SubItems[0].Text == txtCodigo.Text || mostrarProductos.Items[i].SubItems[2].Text == txtCodigo.Text) 
                { 
                    mostrarProductos.Items[i].Selected = true;
                    string cad_InfoProducto = string.Format("{0}  {1} con descuento de: {2}", mostrarProductos.Items[i].SubItems[1].Text, mostrarProductos.Items[i].SubItems[4].Text, mostrarProductos.Items[i].SubItems[3].Text);
                    MessageBox.Show(cad_InfoProducto, "Datos del Producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (mostrarProductos.Items.Count > 0)
            {
                mostrarProductos.Items.Clear();
            }
            BuscarProducto();
            txtCodigo.SelectAll();
            txtCodigo.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
