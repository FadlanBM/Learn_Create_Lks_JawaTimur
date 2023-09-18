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
            DialogResult dialog = MessageBox.Show("Apakah anda yakin ingin keluar dari aplikasi?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult.Yes==dialog)
            {
                Environment.Exit(0);
            }
        }

        private void employeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FEmployee fEmployee = new FEmployee();
            fEmployee.StartPosition = FormStartPosition.CenterParent;
            fEmployee.MdiParent = this;
            fEmployee.Show();
          
        }

        private void foodAndDrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMasterFoodDrink fMasterFood=new FMasterFoodDrink();
            fMasterFood.StartPosition=FormStartPosition.CenterParent;
            fMasterFood.MdiParent = this;
            fMasterFood.Show();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMasterItem Fitem=new FMasterItem();
            Fitem.StartPosition = FormStartPosition.CenterScreen;
            Fitem.MdiParent = this;
            Fitem.Show();
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMasterRoomType fMasterRoomType=new FMasterRoomType();  
            fMasterRoomType.StartPosition = FormStartPosition.CenterScreen;
            fMasterRoomType.MdiParent = this;
            fMasterRoomType.Show(); 
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMasterRoom room=new FMasterRoom();
            room.StartPosition = FormStartPosition.CenterScreen;    
            room.MdiParent = this;
            room.Show();    
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            FEmployee employee=new FEmployee(); 
            employee.StartPosition = FormStartPosition.CenterScreen;
            employee.MdiParent = this;
            employee.Show();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult.Yes==result)
            {
               new FLogin().Show();
                this.Close();
            }
        }
    }
}
