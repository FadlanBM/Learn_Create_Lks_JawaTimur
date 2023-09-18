using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_SMK_JATI_HOTEL
{
    public partial class FMasterFoodDrink : Form
    {
        OpenFileDialog ofd;
        private string path=Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"\image\";
        AppDbContextDataContext dbcontext;
        private string id;
        public FMasterFoodDrink()
        {
            dbcontext= new AppDbContextDataContext();   
            ofd = new OpenFileDialog();
            InitializeComponent();
            loadNormal();
        }

        private void loadNormal() {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            lb_name.Enabled = false;
            lb_price.Enabled = false;
            cb_type.Enabled=false;
            button5.Text = "";
        }

        private void modeInsert()
        {
            button5.Text = "Insert";
            button1.Enabled =true;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            lb_name.Enabled = true;
            lb_price.Enabled = true;
            cb_type.Enabled = true;
            clearData();
        }

        private void  modeUpdate() {
            button5.Text = "Update";
            button1.Enabled =true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            lb_name.Enabled = true;
            lb_price.Enabled = true;
            cb_type.Enabled = true;
        }

        private void modeDelete()
        {
            button5.Text = "Delete";
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            lb_name.Enabled = false;
            lb_price.Enabled = false;
            cb_type.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            modeInsert();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modeUpdate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            modeDelete();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var valid = true;
            if (button4.Enabled == true)
            {
                var data = dbcontext.FoodsAndDrinks.Where(c => c.ID == int.Parse(id)).FirstOrDefault();
                if (data != null)
                {
                    DialogResult message = MessageBox.Show("Apakahan anda yanin ingin menghapus data ini?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (DialogResult.Yes == message)
                    {
                        dbcontext.FoodsAndDrinks.DeleteOnSubmit(data);
                        dbcontext.SubmitChanges();
                        MessageBox.Show("Berhasil delete data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearData();
                        loadData();
                        return;
                    }
                }
            }
            if (lb_name.Text.Length==0)
            {
                valid = false;
                errorProvider1.SetError(lb_name, "Form name bmsih kosong");
            }
            else
            {
                errorProvider1.SetError(lb_name, "");
            }

            if (lb_price.Text.Length==0)
            {
                valid = false;
                errorProvider1.SetError(lb_price, "Form price masih kosong");
            }
            else
            {
                errorProvider1.SetError(lb_price, "");
            }
            if (cb_type.Text.Length==0)
            {
                valid = false;
                errorProvider1.SetError(cb_type, "Form cb masih kosong");
            }
            else
            {
                errorProvider1.SetError(cb_type, "");
            }
            if (pictureBox1.Image==null) {
                errorProvider1.SetError(button1, "Picture masih kosong");
            }
            else
            {
                errorProvider1.SetError(button1, "");
            }                
            if (!valid)
            {
                return;
            }
            if (ofd.FileName.Length==0)
            {
                errorProvider1.SetError(button1, "Image belum di Masukkan");
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var name=DateTime.Now.Ticks.ToString()+Path.GetFileName(ofd.FileName);
            var price = lb_price.Text.Replace(".", "").Replace(",", "");
            int i;
            if (!int.TryParse(price, out i))
            {
                MessageBox.Show($"Price yang anda masukkan tidak valid max = {int.MaxValue}");
                return;
            }
            if (button2.Enabled==true)
            {
                FoodsAndDrink fd = new FoodsAndDrink();               
                fd.Name=lb_name.Text;
                fd.Price=int.Parse(price);
                if (cb_type.Text=="Makanan")
                {
                    fd.Type = '0';
                }
                if (cb_type.Text=="Minuman")
                {
                    fd.Type = '1';
                }
                fd.Photo =name;
                File.Copy(ofd.FileName, path+name);
                dbcontext.FoodsAndDrinks.InsertOnSubmit(fd);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil input data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearData();
                loadData();
                return;
            }
            if (button3.Enabled==true)
            {
                var data=dbcontext.FoodsAndDrinks.Where(d=>d.ID==int.Parse(id)).FirstOrDefault();
                if (data.Photo==Path.GetFileName(ofd.FileName))
                {
                    return;
                }
                if (File.Exists(path+data.Photo))
                {
                    File.Delete(path + data.Photo);
                }
                data.Name=lb_name.Text;
                data.Price = int.Parse(price);
                if (cb_type.Text== "Makanan")
                {
                    data.Type = '0';
                }
                if (cb_type.Text== "Minuman")
                {
                    data.Type = '1';
                }
                File.Copy(ofd.FileName, path +name);
                data.Photo=name;
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil update data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearData();
                loadData();
                return;
            }         
        }

        private void clearData() {
            pictureBox1.Image = null;
            lb_name.Text = "";
            lb_price.Text = "";
            cb_type.Text = "";
        }

        private void loadData() {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Rows.Clear();
            var data = (from c in dbcontext.FoodsAndDrinks
                        select new
                        {
                            id=c.ID,name=c.Name, price=c.Price, type=c.Type
                        }).ToList();
            foreach (var item in data)
            {
                var num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item.id;
                dataGridView1.Rows[num].Cells[1].Value = item.name;
                dataGridView1.Rows[num].Cells[2].Value = string.Format("{0:n}", item.price);
                dataGridView1.Rows[num].Cells[3].Value = item.type==0?"Makanan":"Minuman";

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadNormal();
            clearData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Multiselect = false;
            ofd.Filter = "(Filter | *jpg;*png;)";
            if (DialogResult.OK==ofd.ShowDialog())
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void FMasterFoodDrink_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (button2.Enabled==Enabled)
            {
                return;
            }
            loadDataForm();
            
        }

        private void loadDataForm() {
            var data = dbcontext.FoodsAndDrinks.Where(fd => fd.ID == int.Parse(id)).FirstOrDefault();
            lb_name.Text=data.Name;
            if (data.Type=='0')
            {
                cb_type.Text = "Makanan";
            }
            if (data.Type=='1')
            {
                cb_type.Text = "Minuman";
            }
            lb_price.Text=data.Price.ToString();
            if (!File.Exists(path + data.Photo))
            {
                return;
            }
            using (var image=File.OpenRead(path+data.Photo))
            {
                pictureBox1.Image = new Bitmap(image);
                ofd.FileName= path + data.Photo;
            }
        }
    }
}
