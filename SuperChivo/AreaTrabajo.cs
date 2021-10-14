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
        public AreaTrabajo()
        {
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
 

            Control a = new Panel();
            a.BackColor = Color.Azure;
            a.Text = "Hola mundo";
            this.tableLayoutPanel1.Controls.Add(a, 0, 0);

            
        }

       
    }
}
