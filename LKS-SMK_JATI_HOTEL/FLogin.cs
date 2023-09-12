using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_SMK_JATI_HOTEL
{
    public partial class FLogin : Form
    {
        AppDbContextDataContext dbcontext;
        public FLogin()
        {
            dbcontext= new AppDbContextDataContext();   
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lb_username.Text.Length == 0)
                errorProvider1.SetError(lb_username, "Form Username belum di isi!");
            if (lb_password.Text.Length == 0)
                errorProvider1.SetError(lb_password, "Form Password belum di isi!");

            var username = dbcontext.Employees.Where(u => u.Username == lb_username.Text).FirstOrDefault();
            var job=dbcontext.Jobs.Where(j=>j.ID==username.JobID).FirstOrDefault();
            if (username!=null)
            {
                if (username.Password==createSha(lb_password.Text))
                {
                    if (job.Name=="Admin")
                    {
                        new FMainAdmin().Show();
                        this.Hide();
                        return;
                    }
                    else
                    {
                        new FMainOffice().Show();
                        this.Hide(); 
                        return;
                    }
                }
                else
                {

                    errorProvider1.SetError(lb_password, "Password yang anda masukkan salah!");
                    return;
                }

            }
            else
            {
                errorProvider1.SetError(lb_username, "Username tidak di temukan");
                return;
            }

        }

        private string createSha(string s) { 
            StringBuilder sb = new StringBuilder();
            using (var sha=SHA256.Create())
            {
                var baytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
                for (int i = 0; i < baytes.Length; i++) {
                    sb.Append(baytes[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
