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
    public partial class PagarCuenta : Form
    {
        bool pagado = false;
        double pago;
        public PagarCuenta(string totalActual)
        {
            InitializeComponent();
            pago = ConvertirDecimal(totalActual);
            txtPago.Text = pago.ToString();
            txtPago.SelectAll();
            txtPago.Focus();

        }

        public bool Pagado
        {
            get { return pagado; }
        } 


        private void PagarCuenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            ValidarMonto();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private double ConvertirDecimal(string dinero)
        {
            double retorno = 0.0;
            try
            {
                string nueva = dinero.Substring(1);
                retorno = Convert.ToDouble(nueva);
            }catch(Exception e)
            {
                MessageBox.Show("No se ha podido convertir la cadena a decimal");
            }
            return retorno;
        }
        private void ValidarMonto()
        {
            if (pago > Convert.ToDouble(txtPago.Text))
            {

                MessageBox.Show("La cantidad introducida es menor que el monto a pagar ", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPago.SelectAll();
                txtPago.Focus();
            }
            else
            {
                double cambio = Convert.ToDouble(txtPago.Text) - pago;
                MessageBox.Show (string.Format("El cambio es: {0}", cambio.ToString("C")), "Cambio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pagado = true;
                this.Close();
               
            }
        }
    }
}
