using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_SMK_JATI_HOTEL
{
    public partial class FMainAdmin : Form
    {
        public FMainAdmin()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += showTime;
            timer1.Start();
        }

        private void showTime(object sender, EventArgs e) {
            lb_timer.Text = DateTime.Now.ToString("dd/MM/yyyy | HH:mm:ss");
        }
        private void toolStripLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
