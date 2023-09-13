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
    public partial class FMasterRoom : Form
    {
        AppDbContextDataContext dbcontext;
        private string id=String.Empty; 
        public FMasterRoom()
        {
            dbcontext = new AppDbContextDataContext();
            InitializeComponent();
        }

        private void normalMode() { 
            lb_number.Enabled = false;
            cb_type.Enabled = false;
            lb_description.Enabled = false;
            lb_floor.Enabled = false;
            button1.Enabled = true;
            button2.Enabled=true;
            button3.Enabled=true;
            button4.Enabled=false;
            button5.Enabled=false;
            button4.Text = "Save";
        }
        private void InsertMode() { 
            lb_number.Enabled = true;
            cb_type.Enabled = true;
            lb_description.Enabled = true;
            lb_floor.Enabled = true;
            button1.Enabled = true;
            button2.Enabled=false;
            button3.Enabled=false;
            button4.Enabled=true;
            button5.Enabled=true;
        }
               private void updateMode() { 
            lb_number.Enabled = true;
            cb_type.Enabled = true;
            lb_description.Enabled = true;
            lb_floor.Enabled = true;
            button1.Enabled = false;
            button2.Enabled=true;
            button3.Enabled=false;
            button4.Enabled=true;
            button5.Enabled=true;
        }  private void deletMode() { 
            lb_number.Enabled = true;
            cb_type.Enabled = true;
            lb_description.Enabled = true;
            lb_floor.Enabled = true;
            button1.Enabled = false;
            button2.Enabled=false;
            button3.Enabled=true;
            button4.Enabled=true;
            button5.Enabled=true;
        }
        private void clearForm() {
            lb_number.Text = string.Empty;
            lb_floor.Text = string.Empty;
            lb_description.Text=string.Empty;   
            cb_type.Text = string.Empty;
           
        }

        private void loadcb() {
            var roomtype = (from ty in dbcontext.RoomTypes
                            select new
                            {
                                name=ty.Name
                            }).ToList();

            foreach (var room in roomtype)
            {
                cb_type.Items.Add(room.name);
            }
        }

        private void loadData() {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Rows.Clear();
            var data = (from r in dbcontext.Rooms
                        join ry in dbcontext.RoomTypes
                        on r.RoomTypeID equals ry.ID
                        select new { 
                            id=r.ID,
                            roomNumber=r.RoomNumber,
                            roomtype=ry.Name,
                            fromFloor=r.RoomFloor,
                            description=r.Description
                        }).ToList();

            foreach (var room in data) { 
                var num =dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = room.id;
                dataGridView1.Rows[num].Cells[1].Value = room.roomNumber;
                dataGridView1.Rows[num].Cells[2].Value=room.roomtype;
                dataGridView1.Rows[num].Cells[3].Value=room.fromFloor;
                dataGridView1.Rows[num].Cells[4].Value = room.description;
            }
        }
       

        private void button5_Click(object sender, EventArgs e)
        {
            normalMode();
            clearForm();
        }

        private void FMasterRoom_Load(object sender, EventArgs e)
        {
            normalMode();
            loadcb();
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InsertMode();   
            clearForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateMode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deletMode();
            button4.Text = "Confirm";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var status = false;

            if (lb_number.Text.Length==0)
            {
                errorProvider1.SetError(lb_number, "Form Room Number belum di isi");
            }
            else
            {
                status = true;
                errorProvider1.SetError(lb_number, "");
            }

            if (cb_type.Text=="")
            {
                errorProvider1.SetError(cb_type, "Room Type belum di isi");
            }
            else
            {
                status = true;
                errorProvider1.SetError(cb_type, "");
            }

            if (lb_floor.Text.Length==0)
            {
                errorProvider1.SetError(lb_floor, "Form Room Floor belum di isi");
            }
            else
            {
                status = true;
                errorProvider1.SetError(lb_floor, "");
            }
            if (lb_description.Text.Length==0)
            {
                errorProvider1.SetError(lb_description,"Form Description belum di isi") ;
            }
            else
            {
                status = true;
                errorProvider1.SetError(lb_description, "");
            }

            if (!status)
            {
                return;
            }

            if (button1.Enabled==true)
            {
                Room room= new Room();
                var data=dbcontext.RoomTypes.Where(r=>r.Name==cb_type.Text).FirstOrDefault();
                if (data == null) {
                    MessageBox.Show("Room Type tidak di temukan");
                    return;
                }
                room.RoomNumber = lb_number.Text;
                room.RoomTypeID =data.ID;
                room.RoomFloor = lb_floor.Text;
                room.Description = lb_description.Text;
                dbcontext.Rooms.InsertOnSubmit(room);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil input data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                normalMode();
                clearForm();
                loadData();
                return;
            }
            if (button2.Enabled==true) {
                var data=dbcontext.Rooms.Where(r=>r.ID==int.Parse(id)).FirstOrDefault();
                var romtype = dbcontext.RoomTypes.Where(ry => ry.Name == cb_type.Text).FirstOrDefault();
                if (romtype==null)
                {
                    MessageBox.Show("Form Room Type tidak di temukan");
                    return;
                }
                data.RoomNumber= lb_number.Text;
                data.RoomTypeID = romtype.ID;
                data.RoomFloor = lb_floor.Text;
                data.Description = lb_description.Text;
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhasil update data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData(); 
                clearForm();
                normalMode();
                return;
            }
            if (button3.Enabled==true)
            {
                DialogResult dialog = MessageBox.Show("Apakah anda yakin ingin menghapus data ini?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.Yes==dialog)
                {
                    var data = dbcontext.Rooms.Where(r => r.ID == int.Parse(id)).FirstOrDefault();
                    if (data == null) {
                        return;
                    }
                    dbcontext.Rooms.DeleteOnSubmit(data);
                    dbcontext.SubmitChanges();
                    MessageBox.Show("Berhasil input data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    normalMode();
                    clearForm();
                    return;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (button1.Enabled==true)
            {
                return;
            }
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();           
            loadForm();
        }

        private void loadForm() {
            var data=dbcontext.Rooms.Where(r=>r.ID==int.Parse(id)).FirstOrDefault();
            var roomType=dbcontext.RoomTypes.Where(ry=>ry.ID==data.RoomTypeID).FirstOrDefault();
            if (data == null)
            {
                return;
            }
            lb_number.Text = data.RoomNumber;
            lb_floor.Text = data.RoomFloor;
            lb_description.Text = data.Description;
            cb_type.Text = roomType.Name;
        }
    }
}
