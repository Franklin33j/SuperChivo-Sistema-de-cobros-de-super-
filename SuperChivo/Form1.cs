using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//namespaces usados
using System.Runtime.InteropServices;

namespace SuperChivo
{
    public partial class Form1 : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);


        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            tiempo.Start();

        }

        private void tiempo_Tick(object sender, EventArgs e)
        {
            lbHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToLongDateString();
        }

        #region Metodos Creados Por el programador
        private void Mover()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        //iconos de minimizar y cerrar ventana
        private void pictureBox3_Click(object sender, EventArgs e) => Application.Exit();
        private void pictureBox2_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        //permite mover la ventana
        private void panel2_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void panel1_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void panel3_MouseDown(object sender, MouseEventArgs e) => Mover(); 
        private void Form1_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void panel4_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void label1_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void label3_MouseDown(object sender, MouseEventArgs e) => Mover();
        private void label2_MouseDown(object sender, MouseEventArgs e) => Mover();
    }
}
