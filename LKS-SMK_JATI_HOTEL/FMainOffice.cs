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
    public partial class FMainOffice : Form
    {
        public FMainOffice()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += setTime;
            timer1.Start();
        }

        private void setTime(object sender, EventArgs e) {
            lb_time.Text = DateTime.Now.ToString("dd/MM/yyyy | HH:mm:ss");
        }

        private void reseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTestReservation reservation = new FTestReservation();
            reservation.StartPosition = FormStartPosition.CenterScreen;
            reservation.MdiParent= this;    
            reservation.Show();
        }
    }
}
