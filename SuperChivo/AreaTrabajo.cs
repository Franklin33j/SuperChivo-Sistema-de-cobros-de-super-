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
    public partial class AreaTrabajo : Form
    {
        static int contador = 1;
        ConexionBD conexion;
        static double suma=0;
        bool va1 = false;
        static bool cant = false;

        public AreaTrabajo()
        {
            
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            conexion = new ConexionBD();
            conexion.Conectar();
            lbEstado.Text = "Ingrese codigo del producto";
        }




        private void BuscarProductos(string codigo)
        {
            bool repetido = false;
            if (codigo.ToLower().Equals ("x"))
            {
                lbEstado.Text = "Ingrese cantidad";
                txtCodigo.Clear();
                txtCodigo.Focus();
                cant = true;
            }
            else if (cant)
            {
                contador = Convert.ToInt32(txtCodigo.Text);
                lbEstado.Text = "cantidad: "+txtCodigo.Text;
                txtCodigo.Clear();
                txtCodigo.Focus();
                cant = false;
            }
            else
            {
               
                var datos = conexion.ConsultarProductos(codigo);
                if (datos.producto != null)
                {

                    for (byte a = 0; a < mostrarProductos.Items.Count; a++)
                    {
                        if (datos.interno == mostrarProductos.Items[a].SubItems[0].Text)
                        {
                            AgreagarExistente(datos.precio, a);
                            repetido = true;
                            SumarCantProductos();
                            break;
                        }
                    }

                    if (!repetido)
                    {
                       
                        suma = datos.precio;
                        IntroducirValores(datos.interno, datos.producto, datos.descuento, datos.precio, contador);
                        contador = 1;
                        txtCodigo.SelectAll();
                        SumarCantProductos();
                    }
                }
                else
                {
                    MessageBox.Show("Producto no Encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodigo.SelectAll();
                }
            }
           

        }
        private void IntroducirValores(string interno, string descripcion,  int descuento, double precio, int cont)
        {
            double total = precio * contador;
            ListViewItem lista;
            lista = new ListViewItem(interno);
            lista.SubItems.Add(descripcion);
            lista.SubItems.Add("$" + precio.ToString());
            lista.SubItems.Add(cont.ToString() + " unid.");
            lista.SubItems.Add(descuento.ToString() + "%");
            lista.SubItems.Add(total.ToString("C"));
            mostrarProductos.Items.Add(lista);
            mostrarProductos.Select();
            mostrarProductos.HideSelection = false;
            mostrarProductos.FullRowSelect = true;

         
            mostrarProductos.Items[mostrarProductos.Items.Count - 1].Selected = true; // crea el foco para el item creado
            if (mostrarProductos.Items.Count > 1)
            {
                mostrarProductos.Items[mostrarProductos.Items.Count - 2].Selected = false;
            }
            SumarCantProductos();
            lista = null;
            txtCodigo.Focus();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)Keys.Enter) BuscarProductos(txtCodigo.Text);
        }

        //implementar
        private void  AgreagarExistente(double precioProducto,int fila )
        {
            StringBuilder cantidad = new StringBuilder(mostrarProductos.Items[fila].SubItems[3].Text);
            cantidad.Remove(1, cantidad.Length - 1);
            int cant = Convert.ToInt32(cantidad.ToString());
            cant++;
            mostrarProductos.Items[fila].SubItems[3].Text = string.Format(cant + " unid. ");
            precioProducto *= cant;
            mostrarProductos.Items[fila].SubItems[5].Text = precioProducto.ToString("C");
            EliminarTodasSelecciones();
            mostrarProductos.Items[fila].Selected = true;
        }
        private void SumarCantProductos()
        {
            double total = 0;
            int cantItems = mostrarProductos.Items.Count;

            for (byte a = 0; a < cantItems; a++)
            {
                try
                {
                    // eliminar el signo $ del string
                    string aux = mostrarProductos.Items[a].SubItems[5].Text;
                    StringBuilder modificada = new StringBuilder(aux);
                    modificada.Remove(0, 1);
                    total += Convert.ToDouble(modificada.ToString());
                }
                catch (Exception e)
                {
                }
            }
            lbTotal.Text = total.ToString("C");
            //  calcular el iva:
            double iva = (total / 100) * 13;
            lbIVA.Text = iva.ToString("C");
            lbTotalNeto.Text = string.Format("{0:C}", total + iva);
        }
        private void EliminarTodasSelecciones()
        {
            //elimina el ultimo elemento que posee el foco(que esta seleccionado)
            ListView.SelectedIndexCollection valores = mostrarProductos.SelectedIndices;
            foreach (var a in valores)
            {
                byte indice = Convert.ToByte(a.ToString());
                mostrarProductos.Items[indice].Selected = false;
            }
        }
    }
}
