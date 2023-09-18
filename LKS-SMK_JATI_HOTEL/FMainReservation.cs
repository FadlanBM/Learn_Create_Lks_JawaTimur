using Microsoft.SqlServer.Server;
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
    public partial class FMainReservation : Form
    {
        AppDbContextDataContext dbcontext;
        private DataGridViewRow selectedRow; // Deklarasikan variabel untuk menyimpan baris yang dipilih

        public FMainReservation()
        {
            dbcontext = new AppDbContextDataContext();
            InitializeComponent();
        }

        private void loadDataCustomer() { 
            dataGrid_Customer.AllowUserToAddRows = false;
            dataGrid_Customer.AllowUserToDeleteRows = false;
            dataGrid_Customer.Rows.Clear();

            var data =(from c  in dbcontext.Customers
                       select c).ToList();

            if (tb_searchName.Text.Length!=0)
            {
                data = data.Where(d => d.Name.Contains(tb_searchName.Text)).ToList();
            }

            foreach ( var c in data )
            {
                var num =dataGrid_Customer.Rows.Add();
                dataGrid_Customer.Rows[num].Cells[1].Value = c.ID;
                dataGrid_Customer.Rows[num].Cells[2].Value = c.Name;
                dataGrid_Customer.Rows[num].Cells[3].Value = c.Email;
                dataGrid_Customer.Rows[num].Cells[4].Value=c.Gender==0?"Female":"Male";
            }

        }


        private void loadDataRoom() { 
            var data=(from rm in dbcontext.Rooms
                      join rt in dbcontext.RoomTypes
                      on rm.RoomTypeID equals rt.ID
                      select new { 
                            number=rm.RoomNumber,
                            floor=rm.RoomFloor,
                            price=rt.RoomPrice,
                            desc=rm.Description
                      }).ToList();
            dataGrid_room_availebel.Columns[2].DefaultCellStyle.Format = "#,##";
            foreach ( var item in data ) { 
                var num=dataGrid_room_availebel.Rows.Add();
                dataGrid_room_availebel.Rows[num].Cells[0].Value = item.number;
                dataGrid_room_availebel.Rows[num].Cells[1].Value = item.floor;
                dataGrid_room_availebel.Rows[num].Cells[2].Value = item.price;
                dataGrid_room_availebel.Rows[num].Cells[3].Value = item.desc;               
            }
        }

        private void FMainReservation_Load(object sender, EventArgs e)
        {
            loadDataCustomer();
            loadDataRoom();
        }

        private void tb_searchName_TextChanged(object sender, EventArgs e)
        {
            loadDataCustomer();
        }

        private void logicRb() {
            if (rb_addCustomer.Checked==true)
            {
                panel1.Visible = true;
                rd_search_Customer.Checked = false;
            }
            if (rd_search_Customer.Checked==true)
            {
                panel1.Visible = false;
                rb_addCustomer.Checked = false;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void rd_search_Customer_CheckedChanged(object sender, EventArgs e)
        {
            logicRb();
        }

        private void rb_addCustomer_CheckedChanged(object sender, EventArgs e)
        {
            logicRb();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGrid_room_availebel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {           
                selectedRow = dataGrid_room_availebel.Rows[e.ColumnIndex];
          
        }

      
       

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (selectedRow != null)
            {
                // Tambahkan baris baru di datagrid_select_room
                int index = datagrid_select_room.Rows.Add();

                // Setel nilai sel di datagrid_select_room dengan nilai sel yang dipilih di dataGrid_room_availebel
                for (int i = 0; i < selectedRow.Cells.Count; i++)
                {
                    datagrid_select_room.Rows[index].Cells[i].Value = selectedRow.Cells[i].Value;
                }

                // Hapus baris yang dipindahkan dari dataGrid_room_availebel
                dataGrid_room_availebel.Rows.RemoveAt(selectedRow.Index);

                // Reset selectedRow ke null setelah pemindahan
                selectedRow = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                // Tambahkan baris baru di datagrid_select_room
                int index = dataGrid_room_availebel.Rows.Add();

                // Setel nilai sel di datagrid_select_room dengan nilai sel yang dipilih di dataGrid_room_availebel
                for (int i = 0; i < selectedRow.Cells.Count; i++)
                {
                    dataGrid_room_availebel.Rows[index].Cells[i].Value = selectedRow.Cells[i].Value;
                }

                // Hapus baris yang dipindahkan dari dataGrid_room_availebel
                datagrid_select_room.Rows.RemoveAt(selectedRow.Index);

                // Reset selectedRow ke null setelah pemindahan
                selectedRow = null;
            }
        }

        private void datagrid_select_room_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = datagrid_select_room.Rows[e.ColumnIndex];
        }


        private void button5_Click(object sender, EventArgs e)
        {
            var customerId = string.Empty;
            foreach (DataGridViewRow item in dataGrid_Customer.Rows)
            {
                var x = (DataGridViewCheckBoxCell)item.Cells[0];
                if (Convert.ToBoolean(item.Cells[0].Value)==true)
                {
                    customerId = item.Cells[1].Value.ToString();
                }
            }

            FReservation reservation = new FReservation();
            reservation.CustomerID = Convert.ToInt32(customerId);
            reservation.EmployeeID = 6;
            reservation.DateTime=DateTime.Now;
            reservation.BookingCode = getItem().ToUpper().Substring(0,6);
            dbcontext.Reservations.InsertOnSubmit(reservation);
            dbcontext.SubmitChanges();


            foreach (DataGridViewRow item in datagrid_select_room.Rows)
            {
               
                var room_number = item.Cells[0].Value.ToString();
                var room_floor = item.Cells[1].Value.ToString();
                var room_price = item.Cells[2].Value;
                var roomid = dbcontext.Rooms.Where(r => r.RoomNumber == room_number).FirstOrDefault();
                var desc = item.Cells[3].Value.ToString();                                
                ReservationRoom room=new ReservationRoom();
                room.ReservationID = reservation.ID;
                room.RoomID = roomid.ID;
                room.CheckInDateTime = dt_checkIn.Value;
                room.CheckOutDateTime=dt_checkOut.Value;
                var date = dt_checkOut.Value - dt_checkIn.Value;
                room.DurationNights=date.Days;
                    room.RoomPrice =Convert.ToInt32( room_price);
                dbcontext.ReservationRooms.InsertOnSubmit(room);
                dbcontext.SubmitChanges();
                MessageBox.Show("Berhassil save");
            }
        }

        private string getItem() { 
              Random rd=new Random();
            var num = rd.Next(3, 5);
            var code = "";
            var i = 0;
            do
            {
                var baytes = rd.Next(48, 123);
                if ((baytes>48&&baytes<57)||(baytes>60&&baytes<90)||(baytes<92&&baytes>123))
                {
                    i++;
                    code = code + (char)baytes;
                    if (baytes == i)
                        break;
                    {

                    }
                }
            } while (true);
            return code;
        }


    }
}
