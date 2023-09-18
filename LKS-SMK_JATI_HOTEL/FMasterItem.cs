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
    public partial class FMasterItem : Form
    {
        AppDbContextDataContext dbcontext;
        string id=string.Empty;
        public FMasterItem()
        {
            dbcontext = new AppDbContextDataContext();
            InitializeComponent();
            loadNormal();
        }

        private void loadNormal() {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            tb_name.Enabled = false;
            tb_requestPrie.Enabled = false;
            tb_compensation.Enabled = false;
            button4.Text = "";
        }

        private void loadInsert() {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            tb_name.Enabled = true;
            tb_requestPrie.Enabled = true;
            tb_compensation.Enabled = true;
        } private void loadupdate() {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            tb_name.Enabled = true;
            tb_requestPrie.Enabled = true;
            tb_compensation.Enabled = true;
        } private void loadDelete() {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            tb_name.Enabled = false;
            tb_requestPrie.Enabled = false;
            tb_compensation.Enabled = false;
        }

        private void clearData() {
            tb_compensation.Text = "";
            tb_name.Text = "";
            tb_requestPrie.Text = "";
        }

        private void loadData() {
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.Rows.Clear();
            var data = (from c in dbcontext.Items
                        select new
                        {
                            id=c.ID,
                            name= c.Name,
                            requestPrice=c.RequestPrice,
                            fee=c.CompensationFee
                        }).ToList();

            foreach (var item in data)
            {
                var num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item.id;
                dataGridView1.Rows[num].Cells[1].Value = item.name;
                dataGridView1.Rows[num].Cells[2].Value = item.requestPrice;
                dataGridView1.Rows[num].Cells[3].Value = item.fee;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadInsert();
            if (button4.Enabled==true)
            {
                button4.Text = "Save";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadupdate();
            if (button2.Enabled==true)
            {
                button4.Text = "Update";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadDelete();
            if (button3.Enabled==true)
            {
                button4.Text = "Submmit";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loadNormal();
            clearData();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            var status = true;
            if (tb_name.Text.Length==0)
            {
                status=false;
                errorProvider1.SetError(tb_name, "Form Name belum di masukkan");
            }
            else
            {
                errorProvider1.SetError(tb_name, "");
            }
            if (tb_requestPrie.Text.Length==0)
            {
                status = false;
                errorProvider1.SetError(tb_requestPrie, "Form request Price belum di masukkan");
            }
            else
            {
                errorProvider1.SetError(tb_requestPrie, "");
            }
            if (tb_compensation.Text.Length==0)
            {
                status = false;
                errorProvider1.SetError(tb_compensation, "Compensation Fee");
            }
            else
            {
                errorProvider1.SetError(tb_compensation, "");
            }

            if (!status)
            {
                return;
            }

            if (button1.Enabled==true)
            {
                Item item = new Item();
                item.Name=tb_name.Text;
                item.RequestPrice = int.Parse(tb_requestPrie.Text);
                item.CompensationFee =int.Parse( tb_compensation.Text);
                dbcontext.Items.InsertOnSubmit(item);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil input data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearData();
                loadData(); 
                return;
            }
            if (button2.Enabled==true)
            {
                var data=dbcontext.Items.Where(i=>i.ID==int.Parse(id)).FirstOrDefault();
                if (data!=null)
                {
                    data.Name = tb_name.Text;
                    data.RequestPrice =int.Parse( tb_requestPrie.Text);
                    data.CompensationFee = int.Parse(tb_compensation.Text);
                    dbcontext.SubmitChanges();
                    MessageBox.Show("Berhasil update data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearData();
                    loadData();
                    return;
                }
            }
            if (button3.Enabled==true)
            {
                
                if (id!=null)
                {
                    var data = dbcontext.Items.Where(i => i.ID == int.Parse(id)).FirstOrDefault();
                    if (data==null)
                    {
                        return;
                    }
                    DialogResult result = MessageBox.Show("Apakah anda ingin menghapus data ini", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);                  
                    if (result==DialogResult.Yes)
                    {
                        dbcontext.Items.DeleteOnSubmit(data);
                        dbcontext.SubmitChanges();
                        MessageBox.Show("Berhasil menghapus data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadData();
                        clearData();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Belum ada data yang di pilih", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void FMasterItem_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadtbInput() {
            var data = dbcontext.Items.Where(i => i.ID == int.Parse(id)).FirstOrDefault();
            tb_name.Text = data.Name;
            tb_requestPrie.Text = data.RequestPrice.ToString();
            tb_compensation.Text = data.CompensationFee.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (button1.Enabled==true)
            {
                return; 
            }
            loadtbInput();
        }
    }
}
