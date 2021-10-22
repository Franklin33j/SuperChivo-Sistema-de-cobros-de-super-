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
        static bool adminActivo = false;
        ConexionBD conexion;
        static double suma=0;
        bool va1 = false;
        static bool cant = false;
        static bool sumarCant = false;
        ValidarAdmin admin;




        public AreaTrabajo(string nombre, string puesto)
        {
            
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            conexion = new ConexionBD();
            conexion.Conectar();
            lbEstado.Text = "Ingrese codigo del producto";
            lbNombreCajero.Text = nombre;
            lbEstadoAdmin.Text = "INACTIVO";

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
                sumarCant = true;
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
                            lbEstado.Text = "Ingrese codigo del producto";
                            AgreagarExistente(datos.precio, a);
                            repetido = true;
                            SumarCantProductos();
                            break;
                        }
                    }
                    if (!repetido)
                    {
                        lbEstado.Text = "Ingrese codigo del producto";
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
            lista.SubItems.Add( precio.ToString("C"));
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

        private void  AgreagarExistente(double precioProducto,int fila )
        {
            string  cantidad = mostrarProductos.Items[fila].SubItems[3].Text;
            //obtener solo numeros con linq
            string cantidadNuevo = string.Concat(cantidad.Where(c => Char.IsDigit(c)));
            int cant = Convert.ToInt32(cantidadNuevo);
            cant++;
            if (sumarCant)
            {
                cant--;
                cant += contador;
                sumarCant = false;
                
            }
            
            mostrarProductos.Items[fila].SubItems[3].Text = string.Format(cant + " unid. ");
            precioProducto *= cant;
            mostrarProductos.Items[fila].SubItems[5].Text = precioProducto.ToString("C");
            EliminarTodasSelecciones();
            mostrarProductos.Items[fila].Selected = true;
            contador = 1;
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
                    MessageBox.Show("Error: " + e.Message);
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

        private void AreaTrabajo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (adminActivo)
                {
                    ///borrar los datos por la tecla delete
                    for (int i = 0; i < mostrarProductos.SelectedItems.Count; i++)
                    {
                        ListViewItem lista = mostrarProductos.SelectedItems[i];
                        mostrarProductos.Items[lista.Index].Remove();
                    }
                    //sumar los precios nuevamente
                    SumarCantProductos();
                }
                else
                {
                    
                    AdminNesesario validar = new AdminNesesario();
                    validar.ShowDialog();
                }
            }
            if(e.KeyCode == Keys.F12)
            {
                admin = new ValidarAdmin();
                admin.ShowDialog();
                //Si se han introducido credenciales validas se activara el admin
                if (admin.Estado)
                {
                    adminActivo =true;
                    lbEstadoAdmin.Text = "ACTIVO";
                }
            }
            if (e.KeyCode == Keys.F4)
            {
                if (adminActivo)
                {
                    adminActivo = false;
                    lbEstadoAdmin.Text = "INACTIVO";
                }
                else { MessageBox.Show("El administrador no esta activo","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
                this.Close();
            }
            if (e.KeyCode == Keys.F11)
            {
                btnPagar.PerformClick();
            }
            if (e.KeyCode == Keys.F1)
            {
                btnClienteF.PerformClick();
            }
        }
        /// <summary>
        /// Para que la ventana se muestre una sola vez la ventana
        /// </summary>
       

        private void AreaTrabajo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void BorrarDatos()
        {
            lbIVA.Text = "$0.00";
            lbTotal.Text= "$0.00";
            lbTotalNeto.Text ="$0.00";
            mostrarProductos.Items.Clear();
            txtCodigo.Clear();
            txtCodigo.Focus();
        }
        private void btnPagar_Click(object sender, EventArgs e)
        {
            if (lbTotalNeto.Text != "$0.00")
            {
                PagarCuenta pago = new PagarCuenta(lbTotalNeto.Text);
                pago.ShowDialog();
                if (pago.Pagado)
                {
                    GenerarTikets tikets = new GenerarTikets(mostrarProductos, lbIVA.Text, lbTotal.Text, lbTotalNeto.Text,lbNombreCajero.Text);
                    BorrarDatos();
                }
            }
            else { MessageBox.Show("No tiene ningun producto por cancelar", "COBRO CANCELADO", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void btnClienteF_Click(object sender, EventArgs e)
        {
            if (adminActivo)
            {
                double saldo = Convert.ToDouble(lbTotalNeto.Text);
                double descuento = saldo - ((saldo / 100) * 11);
                lbEstado.Text = "Descuento Cliente Frecuente Aplicado";
                lbTotalNeto.Text = descuento.ToString("C");
            }
            else
            {
                admin = new ValidarAdmin();
                admin.ShowDialog();
            }
           
        }
    }
}
