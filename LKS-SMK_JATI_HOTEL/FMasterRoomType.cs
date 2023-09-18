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
    public partial class FMasterRoomType : Form
    {
        OpenFileDialog ofd;
        private string path=Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"\image\";
        AppDbContextDataContext dbcontext;
        private string id = string.Empty;
        public FMasterRoomType()
        {
            dbcontext=new AppDbContextDataContext();
            ofd= new OpenFileDialog();  
            InitializeComponent();  
        }

        private void FMasterRoomType_Load(object sender, EventArgs e)
        {
            normalMode();
            getData();           
        }

        private void getData() {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows=false;
            dataGridView1.Rows.Clear(); 
            var data = (from ry in dbcontext.RoomTypes
                        select new
                        {
                            id=ry.ID,
                            name=ry.Name,
                            capacity=ry.Capacity,
                            roomprice=ry.RoomPrice
                        }).ToList();
            foreach (var item in data)
            {
                var num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item.id;
                dataGridView1.Rows[num].Cells[1].Value = item.name;
                dataGridView1.Rows[num].Cells[2].Value = item.capacity;
                dataGridView1.Rows[num].Cells[3].Value = string.Format("{0:n}",item.roomprice);
               
            }
        }

        private void loadData() { 
        
            var data=dbcontext.RoomTypes.Where(r=>r.ID==int.Parse(id)).FirstOrDefault();
            tb_name.Text = data.Name;
            tb_priceRoom.Text = data.RoomPrice.ToString();
            up_capacity.Value=data.Capacity;
            var nameImage = path + data.Photo;
            if (!File.Exists(nameImage))
            {
                return;
            }
            using (var image=File.OpenRead(nameImage))
            {
                pictureBox1.Image = new Bitmap(image);
            }
        }

        private void normalMode() { 
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled=true;
            button3.Enabled= true;
            button4.Enabled= true;
            button5.Enabled= false;
            button6.Enabled= false;
            tb_name.Enabled= false;
            tb_priceRoom.Enabled= false;
            up_capacity.Enabled= false;
        }
        private void insertMode() {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            tb_name.Enabled = true;
            tb_priceRoom.Enabled = true;
            up_capacity.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insertMode();
            clearForm();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Apakah anda yakin ingin membatalkan aksi","Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (DialogResult.Yes==dialog)
            {
                normalMode();
                clearForm();
            }                        
        }

        private void clearForm() {
            tb_name.Text = "";
            tb_priceRoom.Text = "";
            up_capacity.Value = 0;
            pictureBox1.Image = null;
        }
        private void updateMode() {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;            
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            tb_name.Enabled = true;
            tb_priceRoom.Enabled = true;
            up_capacity.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            updateMode();
        }

        private void deleteMode() {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            tb_name.Enabled = true;
            tb_priceRoom.Enabled = true;
            up_capacity.Enabled = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            deleteMode();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "(Files | *jpg;*png;*jpeg)";
            ofd.Multiselect=false;
            if (DialogResult.OK==ofd.ShowDialog())
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var lanjut = true;
            if (tb_name.Text.Length == 0)
            {
               lanjut=false;
                errorProvider1.SetError(tb_name, "Form nama belum terisi");
            }      
            else
            {
                lanjut = false;
                errorProvider1.SetError(tb_name,"");
            }


            if (up_capacity.Value == 0)
            {
                lanjut = false;
                errorProvider1.SetError(up_capacity, "Form capacity belum terisi");
            }
            else
            {
                lanjut = false;
                errorProvider1.SetError(up_capacity, "");
            }


            if (tb_priceRoom.Text.Length == 0)
            {
                lanjut = false;
                errorProvider1.SetError(tb_priceRoom, "Form Pric eRoom belum terisi");
            }
            else
            {
                lanjut = false;
                errorProvider1.SetError(tb_priceRoom, "");
            }

            if (pictureBox1.Image == null)
            {
                lanjut = false;
                errorProvider1.SetError(pictureBox1, "Picture belum di masukkan");
            }
            else
            {
                lanjut = false;
                errorProvider1.SetError(pictureBox1, "");
            }

            if (lanjut)
            {
                return;
            }         
         
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (button2.Enabled==true)
            {
                var nameImage = DateTime.Now.Ticks.ToString() + Path.GetFileName(ofd.FileName);
               RoomType roomType = new RoomType();
                roomType.Name=tb_name.Text;
                roomType.Capacity =(int)up_capacity.Value;
                int price; 
                var sucess = int.TryParse( tb_priceRoom.Text.Replace(",",""),out price);
                if (!sucess)
                {
                    MessageBox.Show("Price harus angka");
                }
                roomType.RoomPrice=price;   
                roomType.Photo = nameImage;
                File.Copy(ofd.FileName, path + nameImage);
                dbcontext.RoomTypes.InsertOnSubmit(roomType);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil input data Room Type","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
                getData();
                clearForm();
                normalMode();
                return;
            }
            if (button3.Enabled==true)
            {
                var nameImage=DateTime.Now.Ticks.ToString()+Path.GetFileName(ofd.FileName);
                var data = dbcontext.RoomTypes.Where(r => r.ID == int.Parse(id)).FirstOrDefault();
                var image = path + data.Photo;
                if (File.Exists(image))
                {
                    File.Delete(image);
                }
               data.Name=tb_name.Text;
                data.Capacity=(int)up_capacity.Value;
                data.RoomPrice = int.Parse( tb_priceRoom.Text);
                data.Photo = nameImage;
                File.Copy(ofd.FileName, path + nameImage);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil Update data","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                getData();
                normalMode();
                clearForm() ;   
                return;
            }
            if (button4.Enabled==true)
            {
                DialogResult dialog = MessageBox.Show("Apakah anda ingin menghapus data ini?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.Yes==dialog)
                {
                    var data = dbcontext.RoomTypes.Where(d => d.ID == int.Parse(id)).FirstOrDefault();
                    var imageName=path+ data.Photo;
                    if (data!=null)
                    {
                        if (File.Exists(imageName))
                        {
                            File.Delete(imageName);
                        }
                        var nameImage = path + data.Photo;
                        dbcontext.RoomTypes.DeleteOnSubmit(data);
                        dbcontext.SubmitChanges();
                        MessageBox.Show("Berhasil Delete data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getData();
                        normalMode();
                        clearForm();
                        return;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (button3.Enabled == true) {
                id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                loadData();
            }
           
        }
    }
}
