using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LKS_SMK_JATI_HOTEL
{
    public partial class FTestReservation : Form
    {
        AppDbContextDataContext dbcontext;
        DataGridViewRow row;
        string status=string.Empty;
        public FTestReservation()
        {
            dbcontext = new AppDbContextDataContext();
            InitializeComponent();
        }

        private void loadRooms() {
            datagrid_1.AllowUserToAddRows = false;
            datagrid_1.RowHeadersVisible = false;
            datagrid_1.Rows.Clear();
            var room = (from r in dbcontext.Rooms
                        join rt in dbcontext.RoomTypes
                        on r.RoomTypeID equals rt.ID
                        select new
                        {
                            id=r.ID,
                            number=r.RoomNumber,
                            floor=r.RoomFloor,
                            price=rt.RoomPrice,
                            desc=r.Description
                        }).ToList();
            datagrid_1.Columns[3].DefaultCellStyle.Format = "#,##";
            foreach (var item in room)
            {
                var num = datagrid_1.Rows.Add();
                datagrid_1.Rows[num].Cells[0].Value = item.id;
                datagrid_1.Rows[num].Cells[1].Value= item.number;
                datagrid_1.Rows[num].Cells[2].Value = item.floor;
                datagrid_1.Rows[num].Cells[3].Value= item.price;
                datagrid_1.Rows[num].Cells[4].Value = item.desc;
            }
        
        }

        private void logicrb() { 
            if (rb_addCustomer.Checked==true) {
                panel1.Visible = true;
                rd_search_Customer.Checked = false;
                tb_searchName.Enabled= false;   
            }
            if (rd_search_Customer.Checked==true)
            {
                panel1.Visible = false;
                rb_addCustomer.Checked = false;
                tb_searchName.Enabled = true;
            }
        }
        private void loadDataCustomer() { 
            datagrid_customer.AllowUserToAddRows = false;
            datagrid_customer.RowHeadersVisible=false;
            datagrid_customer.Rows.Clear();
            var data=(from c in dbcontext.Customers
                      select c).ToList();

            if (tb_searchName.Text.Length!=0)
            {
                data = data.Where(d => d.Name.Contains(tb_searchName.Text)).ToList();
            }
            foreach (var c in data)
            {
                var num = datagrid_customer.Rows.Add();
                datagrid_customer.Rows[num].Cells[1].Value=c.ID;
                datagrid_customer.Rows[num].Cells[2].Value=c.Name;
                datagrid_customer.Rows[num].Cells[3].Value=c.Email;
                datagrid_customer.Rows[num].Cells[4].Value=c.Gender==0?"Female":"Male";
            }
        }

        private void loadCbItem() {
            var data = (from i in dbcontext.Items
                        select i).ToList();
            foreach (var c in data) { 
                cb_item.Items.Add(c.Name);
            }

        }

        private void loadDataRoomInformation() { 
            var data=(from ry in dbcontext.RoomTypes
                      select ry).ToList();
            foreach(var r in data)
            {
                cb_room_type.Items.Add(r.Name);
            }
        }
        private void rb_addCustomer_CheckedChanged(object sender, EventArgs e)
        {
            logicrb();
        }

        private void FTestReservation_Load(object sender, EventArgs e)
        {
            validasiItem();
            loadCbItem();
            loadDataRoomInformation();
            loadRooms();
            logicrb();
            loadDataCustomer();
        }

        private void rd_search_Customer_CheckedChanged(object sender, EventArgs e)
        {
            logicrb();
        }

        private void dataGrid_room_availebel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            status = "1";
            row = datagrid_1.Rows[e.RowIndex];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (status!="1")
            {
                return;
            }
            if (row!=null)
            {
                int index = datagrid_2.Rows.Add();
                for (int i = 0;i<row.Cells.Count;i++) {
                    datagrid_2.Rows[index].Cells[i].Value = row.Cells[i].Value;
                }

                datagrid_1.Rows.RemoveAt(row.Index);
                row = null;
            }
        }

        private void datagrid_2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            status = "2";
            row = datagrid_2.Rows[e.RowIndex];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (status != "2")
            {
                return;
            }
            if (row!=null)
            {
                int index = datagrid_1.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    datagrid_1.Rows[index].Cells[i].Value = row.Cells[i].Value;
                }

                datagrid_2.Rows.RemoveAt(row.Index);
                row = null;
            }
        }

        private void dt_checkIn_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dt_checkOut_ValueChanged(object sender, EventArgs e)
        {
            var date = dt_checkOut.Value - dt_checkIn.Value;
            tb_stay.Text=date.Days.ToString() + " Days";
        }

    
        private void tb_stay_Validated(object sender, EventArgs e)
        {          
                if (!string.IsNullOrEmpty(tb_stay.Text) && int.TryParse(tb_stay.Text.Replace(" Days",""), out int stayDays))
                {
                    DateTime newCheckOutDate = DateTime.Now.AddDays(stayDays);
                    dt_checkOut.Value = newCheckOutDate;
                }
                else
                {
                    // Penanganan jika input tidak valid, misalnya kosongkan dt_checkOut atau berikan pesan kesalahan kepada pengguna.
                }           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data=dbcontext.RoomTypes.Where(rt=>rt.Name==cb_room_type.Text).FirstOrDefault();

            datagrid_1.AllowUserToAddRows = false;
            datagrid_1.RowHeadersVisible = false;
            datagrid_1.Rows.Clear();
            var room = (from r in dbcontext.Rooms
                        join rt in dbcontext.RoomTypes
                        on r.RoomTypeID equals rt.ID
                        select new
                        {
                            id = r.ID,
                            idtype=rt.ID,
                            number = r.RoomNumber,
                            floor = r.RoomFloor,
                            price = rt.RoomPrice,
                            desc = r.Description
                        }).ToList();
            if (data!=null)
            {
                room=room.Where(r=>r.idtype.ToString().Contains(data.ID.ToString())).ToList();
            }
            datagrid_1.Columns[3].DefaultCellStyle.Format = "#,##";
            foreach (var item in room)
            {
                var num = datagrid_1.Rows.Add();
                datagrid_1.Rows[num].Cells[0].Value = item.id;
                datagrid_1.Rows[num].Cells[1].Value = item.number;
                datagrid_1.Rows[num].Cells[2].Value = item.floor;
                datagrid_1.Rows[num].Cells[3].Value = item.price;
                datagrid_1.Rows[num].Cells[4].Value = item.desc;
            }
        }

        private void tb_searchName_TextChanged(object sender, EventArgs e)
        {
            loadDataCustomer();
        }

        private void loadPrice() {
            var data = dbcontext.Items.Where(i => i.Name == cb_item.Text).FirstOrDefault();
            if (data != null)
            {
                var price = int.Parse(up_quantityItem.Value.ToString()) * data.RequestPrice;
                tb_subTotal.Text = price.ToString();
            }
        }

        private void cb_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = dbcontext.Items.Where(i => i.Name == cb_item.Text).FirstOrDefault();
            tb_price_item.Text = data.RequestPrice.ToString();
            validasiItem();
        }

        private void up_quantityItem_ValueChanged(object sender, EventArgs e)
        {
            loadPrice();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string item = cb_item.Text;
            string  quantity=up_quantityItem.Value.ToString() ;
            string price = tb_price_item.Text ;
            string subprice=tb_subTotal.Text ;
            datagrid_listItem.Rows.Add(item, quantity,price,subprice);
        }

        private void datagrid_listItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                DataGridViewRow nameRow = datagrid_listItem.Rows[e.RowIndex];
                datagrid_listItem.Rows.RemoveAt(nameRow.Index);
            }
        }

        private void validasiItem() {
            if (cb_item.Text.Length==0)
            {
                up_quantityItem.Enabled = false;
                tb_price_item.Enabled=false;
                tb_subTotal.Enabled=false;
            }
            else
            {
                up_quantityItem.Enabled = true;
                tb_price_item.Enabled = true;
                tb_subTotal.Enabled = true;
            }
        }
    }
}
