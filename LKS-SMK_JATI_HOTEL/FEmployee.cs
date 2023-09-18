using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_SMK_JATI_HOTEL
{
    public partial class FEmployee : Form
    {
        OpenFileDialog ofd;
        AppDbContextDataContext dbcontext;
        private string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\image\";
        string id = string.Empty;
        public FEmployee()
        {
            dbcontext = new AppDbContextDataContext();
            ofd = new OpenFileDialog();
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FEmployee_Load(object sender, EventArgs e)
        {
            loadDataToCb();   
            loadData();
            normalMode();
        }



        private void normalMode() {
            lb_name.Enabled = false;
            lb_email.Enabled = false;
            lb_address.Enabled = false;
            lb_password.Enabled = false;
            lb_confirm.Enabled = false;
            lb_username.Enabled = false;
            cb_job.Enabled = false;
            dt_birth.Enabled=false;
            button1.Enabled =false;
            button2.Enabled = true;
            button3.Enabled=true;
            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            button5.Text = "Save";
        }
        private void insertMode() {
            lb_name.Enabled = true;
            lb_email.Enabled=true;
            lb_address.Enabled=true;
            lb_confirm.Enabled=true;
            lb_username.Enabled=true;
            lb_password.Enabled=true;
            cb_job.Enabled = true;
            dt_birth.Enabled=true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;

        
        }    
        private void updateMode() {
            lb_name.Enabled = true;
            lb_email.Enabled=true;
            lb_address.Enabled=true;
            lb_confirm.Enabled=true;
            lb_username.Enabled=true;
            lb_password.Enabled=true;
            cb_job.Enabled = true;
            dt_birth.Enabled=true;
            button1.Enabled =true;
            button2.Enabled =false;
            button3.Enabled =true;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;

        
        }  private void deleteMode() {
            lb_name.Enabled = false;
            lb_email.Enabled=false;
            lb_address.Enabled=false;
            lb_confirm.Enabled=false;
            lb_username.Enabled=false;
            lb_password.Enabled=false;
            cb_job.Enabled = false;
            dt_birth.Enabled=false;
            button1.Enabled =false;
            button2.Enabled =false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;

        
        }
        private void button2_Click(object sender, EventArgs e)
        {
            insertMode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateMode();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            deleteMode();
            button5.Text = "Confirm";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            normalMode();
            clearFrom();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var status = false;
            if (lb_name.Text.Length==0)
            {
                status = true;
                errorProvider1.SetError(lb_name, "Form Name belum di isi");               
            }
            else {
                errorProvider1.SetError(lb_name, "");
            }
            
            if (lb_username.Text.Length==0)
            {
                status = true;
                errorProvider1.SetError(lb_username, "Form Username belum di isi");               
            }
            else {
                errorProvider1.SetError(lb_username, "");
            }   
            if (lb_email.Text.Length==0)
            {
                status = true;
                errorProvider1.SetError(lb_email, "Form Email belum di isi");               
            }
            else {
                errorProvider1.SetError(lb_email, "");
            }
            if (lb_address.Text.Length==0)
            {
                status = true;
                errorProvider1.SetError(lb_address, "Form Address belum di isi");               
            }
            else {
                errorProvider1.SetError(lb_address, "");
            }
            if (cb_job.Text.Length==0)
            {
                status = true;
                errorProvider1.SetError(cb_job, "Form Address belum di isi");               
            }
            else {
                errorProvider1.SetError(cb_job, "");
            }   
            if (pictureBox1.Image==null)
            {
                status = true;
                errorProvider1.SetError(button1, "Form image belum di isi");               
            }
            else {
                errorProvider1.SetError(button1, "");
            }           

            if (status)
            {
                return;
            }

            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var nameimage = DateTime.Now.Ticks.ToString()+Path.GetFileName(ofd.FileName);

            if (button2.Enabled==true)
            {
                if (lb_password.Text.Length == 0)
                {
                    status = true;
                    errorProvider1.SetError(lb_password, "Form Password belum di isi");
                }
                else
                {
                    errorProvider1.SetError(lb_password, "");
                }
                if (lb_confirm.Text.Length == 0)
                {
                    status = true;
                    errorProvider1.SetError(lb_confirm, "Form Password belum di isi");
                }
                else
                {
                    errorProvider1.SetError(lb_confirm, "");
                }

                if (lb_password.Text != lb_confirm.Text)
                {
                    MessageBox.Show("Password dan confirm password tidak sama!");
                    return;
                }

                if (status)
                {
                    return;
                }

                Employee employee = new Employee();
                var job = dbcontext.Jobs.Where(j => j.Name == cb_job.Text).FirstOrDefault();
                if (job==null)
                {
                    MessageBox.Show("Job tidak ditemukan");
                    return;
                }
                employee.Username=lb_username.Text;
                employee.Name = lb_name.Text;
                employee.Email = lb_email.Text;
                employee.Address = lb_address.Text;
                employee.DateOfBirth = dt_birth.Value;
                employee.JobID = job.ID;
                employee.Password = getSHA(lb_password.Text);
                employee.Photo = nameimage;
                File.Copy(ofd.FileName, path + nameimage);
                dbcontext.Employees.InsertOnSubmit(employee);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil input data", "Warining", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData();
                clearFrom();
                return;
            }
            if (button3.Enabled==true)
            {
                var data=dbcontext.Employees.Where(em=>em.ID==int.Parse(id)).FirstOrDefault();
                var jab = dbcontext.Jobs.Where(j => j.Name == cb_job.Text).FirstOrDefault();
                var imageOld =path+data.Photo;

                if (File.Exists(imageOld)) {
                    File.Delete(imageOld);
                }
                data.Name = lb_name.Text;
                data.Username=lb_username.Text;
                data.Email=lb_email.Text;
                data.Address=lb_address.Text;
                data.JobID=jab.ID;
                data.DateOfBirth=dt_birth.Value;
                data.Password=getSHA(lb_password.Text);
                File.Copy(ofd.FileName,path+nameimage);
                data.Photo = nameimage;
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil Update data","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                clearFrom();
                loadData();
                normalMode();
            }
            if (button4.Enabled==true)
            {
                DialogResult dialog = MessageBox.Show("Apakaah anda ingin menghapus data ini", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.Yes==dialog)
                {
                    var data = dbcontext.Employees.Where(em => em.ID == int.Parse(id)).FirstOrDefault();
                    if (data!=null)
                    {
                        dbcontext.Employees.DeleteOnSubmit(data);
                        dbcontext.SubmitChanges();
                        MessageBox.Show("Berhasil delete data","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        loadData();
                        clearFrom();
                        normalMode();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Fiter | *jpg;*png;";
            ofd.Multiselect = false;
            if (DialogResult.OK==ofd.ShowDialog())
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void loadDataToCb() {
            var data =(from ej in dbcontext.Jobs
                       select new { 
                        name= ej.Name,
                       }).ToList();
            foreach (var item in data)
            {
                cb_job.Items.Add(item.name);
            }
        }

        private string getSHA(string p) { 
            StringBuilder sb=new StringBuilder();
            using (var sha=SHA256.Create())
            {
                var baytes = sha.ComputeHash(Encoding.UTF8.GetBytes(p));
                for (int i = 0; i < baytes.Length; i++)
                {
                    sb.Append(baytes[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        private void clearFrom() {
            lb_name.Text = "";
            lb_email.Text = "";
            lb_password.Text = "";
            lb_confirm.Text = "";
            cb_job.Text = "";
            dt_birth.Value = DateTime.Now;
            lb_address.Text = "";
            lb_username.Text = "";
            pictureBox1.Image = null;
        }

        private void loadData() {
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.Rows.Clear();
            var data = (from c in dbcontext.Employees
                        join e in dbcontext.Employees
                        on c.JobID equals e.JobID
                        select new { 
                            id=c.ID,
                            username=c.Username,
                            name=c.Name,
                            email=c.Email,
                            address=c.Address,
                            dateOfBirt=c.DateOfBirth,
                            jobName=e.Name
                        }).ToList();

            foreach (var item in data)
            {
                var num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item.id;
                dataGridView1.Rows[num].Cells[1].Value = item.username;
                dataGridView1.Rows[num].Cells[2].Value = item.name;
                dataGridView1.Rows[num].Cells[3].Value = item.email;
                dataGridView1.Rows[num].Cells[4].Value = item.dateOfBirt;
                dataGridView1.Rows[num].Cells[5].Value = item.jobName; 
                dataGridView1.Rows[num].Cells[6].Value = item.address;
            }
        }

        private void loadDetail() { 
            var data=dbcontext.Employees.Where(e=>e.ID==int.Parse(id)).FirstOrDefault();
            var job=dbcontext.Jobs.Where(j=>j.ID==data.JobID).FirstOrDefault();
            if (data!=null)
            {
                lb_email.Text=data.Email;
                lb_username.Text=data.Username;
                lb_name.Text=data.Name;
                lb_address.Text=data.Address;
                cb_job.Text = job.Name;
                dt_birth.Value = data.DateOfBirth;
               var imagePath = path + data.Photo;
                using (var bitmap=File.OpenRead(imagePath))
                {
                    pictureBox1.Image = new Bitmap(bitmap);
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (button2.Enabled==true)
            {
                return;
            }
            loadDetail();
        }
    }
}
