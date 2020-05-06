using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace QLSV_JSON
{
    public partial class Form1 : Form
    {
        DataSet dsSinhVien = new DataSet();
        int vt, id = 0;
        public Form1()
        {
            InitializeComponent();
        }
        public DataTable taobang1()
        {
            DataTable dt = new DataTable("SinhVien");
            dt.Columns.Add("Hoten");
            dt.Columns.Add("MaSV");
            dt.Columns.Add("Ngaysinh");
            dt.Columns.Add("Quequan");
            dt.Columns.Add("Gioitinh");
            return dt;
        }
        public DataTable taobang2()
        {
            DataTable dt = new DataTable("Diem");
            dt.Columns.Add("Hoten");
            dt.Columns.Add("Toan", typeof(double));
            dt.Columns.Add("Van", typeof(double));
            dt.Columns.Add("Anh", typeof(double));
            dt.Columns.Add("TB", typeof(double));
            return dt;
        }
        public DataTable taobang3()
        {
            DataTable dt = new DataTable("Tinh");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Tentinh");
            return dt;
        }
        //auto size columns datagirdview
        private void autoSize(DataGridView dtgv)
        {
            foreach (DataGridViewColumn i in dtgv.Columns)
            {
                i.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("dulieu.json"))
            {
                string jsonstr;
                System.IO.StreamReader reader = new System.IO.StreamReader("dulieu.json");
                jsonstr = reader.ReadToEnd();
                reader.Close();
                dsSinhVien = JsonConvert.DeserializeObject<DataSet>(jsonstr); 
                datagv1.DataSource = dsSinhVien.Tables["SinhVien"];
                autoSize(datagv1);
                datagv2.DataSource = dsSinhVien.Tables["SinhVien"];
                autoSize(datagv2);
                datagv3.DataSource = dsSinhVien.Tables["Diem"];
                autoSize(datagv3);
                
            }
            else
            {
                DataTable dtSV = taobang1();
                DataTable dtDiem = taobang2();
                DataTable dtTinh = taobang3();

                dsSinhVien.Tables.Add(dtSV);
                dsSinhVien.Tables.Add(dtDiem);
                dsSinhVien.Tables.Add(dtTinh);
            }
            // an panel cap nhat
            panel4.Enabled = false;
        }
        private void refresh()
        {
            tbhoten.Clear();
            tbmasv.Clear();
            dtngaysinh.Text = "1/1/2000";
            tbquequan.Clear();
        }
        //them Sv
        private void btthem_Click(object sender, EventArgs e)
        {
            
            if (tbhoten.Text == "" || tbmasv.Text == "" || tbquequan.Text == "")
            {
                MessageBox.Show("Ban nhap thieu thong tin!");
            }
            else
            {
                id++;
                string gt = "Nam";
                if (rbnu.Checked) gt = "Nu";
                dsSinhVien.Tables["SinhVien"].Rows.Add(tbhoten.Text, tbmasv.Text, dtngaysinh.Value.ToShortDateString(), tbquequan.Text, gt);
                dsSinhVien.Tables["Diem"].Rows.Add(tbhoten.Text, 0, 0, 0, 0);
                dsSinhVien.Tables["Tinh"].Rows.Add(id, tbquequan.Text);
                datagv1.DataSource = dsSinhVien.Tables["SinhVien"];
                
                datagv2.DataSource = dsSinhVien.Tables["SinhVien"];
 
                datagv3.DataSource = dsSinhVien.Tables["Diem"];
                if(id == 1)
                {
                    autoSize(datagv1);
                    autoSize(datagv2);
                    autoSize(datagv3);
                }
                refresh();
            }
        }
        //sua sv
        private void btsua_Click(object sender, EventArgs e)
        {
            //dung cach nay ko thong ke ngay dc
            /*datagv2.CurrentRow.Cells[0].Value = tbhoten.Text;
            datagv2.CurrentRow.Cells[1].Value = tbmasv.Text;
            datagv2.CurrentRow.Cells[2].Value = dtngaysinh.Text;
            datagv2.CurrentRow.Cells[3].Value = tbquequan.Text;*/
            int rowIndex = datagv2.CurrentRow.Index;
            dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(0, tbhoten.Text);
            dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(1, tbmasv.Text);
            dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(2, dtngaysinh.Value.ToShortDateString());
            dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(3, tbquequan.Text);
            dsSinhVien.Tables["Tinh"].Rows[rowIndex].SetField(1, tbquequan.Text);
            if (rbnam.Checked)
            {
                dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(4, rbnam.Text);
            }
            else dsSinhVien.Tables["SinhVien"].Rows[rowIndex].SetField(4,rbnu.Text);
        }
        private void datagv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = datagv2.CurrentRow.Index;
            tbhoten.Text = datagv2.Rows[rowIndex].Cells[0].Value.ToString();
            tbmasv.Text = datagv2.Rows[rowIndex].Cells[1].Value.ToString();
            dtngaysinh.Text = datagv2.Rows[rowIndex].Cells[2].Value.ToString();
            tbquequan.Text = datagv2.Rows[rowIndex].Cells[3].Value.ToString();
            if (datagv2.Rows[rowIndex].Cells[4].Value.ToString() == "Nu")
            {
                rbnu.Checked = true;
            }
            else rbnam.Checked = true;
        }

        //Xoa SV
        private void btxoa_Click(object sender, EventArgs e)
        {
            dsSinhVien.Tables["SinhVien"].Rows.RemoveAt(vt);
        }
        private void datagv2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) vt = e.RowIndex;
        }

        //luu thong tin sv
        private void btluu_Click(object sender, EventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(dsSinhVien);
            System.IO.File.WriteAllText("dulieu.json", jsonstr);
        }
        //sua diem
        private void btcapnhat_Click(object sender, EventArgs e)
        {

            double toan, van, anh, tb;
            //ktra diem nhap vao
            if (!double.TryParse(tbtoan.Text, out toan))
            {
                MessageBox.Show("Nhap sai diem toan!");
                tbtoan.Focus();
            }
            if (!double.TryParse(tbvan.Text, out van))
            {
                MessageBox.Show("Nhap sai diem van!");
                tbvan.Focus();
            }
            if (!double.TryParse(tbanh.Text, out anh))
            {
                MessageBox.Show("Nhap sai diem anh!");
                tbanh.Focus();
            }
            tb = (toan + van + anh) / 3;
            // dung cach nay ko thong ke ngay duoc
            /*datagv3.CurrentRow.Cells[1].Value = toan;
            datagv3.CurrentRow.Cells[2].Value = van;
            datagv3.CurrentRow.Cells[3].Value = anh;
            datagv3.CurrentRow.Cells[4].Value = tb;*/

            int rowIndex = datagv3.CurrentRow.Index;
            dsSinhVien.Tables["Diem"].Rows[rowIndex].SetField(0, datagv3.CurrentRow.Cells[0].Value.ToString());
            dsSinhVien.Tables["Diem"].Rows[rowIndex].SetField(1, toan);
            dsSinhVien.Tables["Diem"].Rows[rowIndex].SetField(2, van);
            dsSinhVien.Tables["Diem"].Rows[rowIndex].SetField(3, anh);
            dsSinhVien.Tables["Diem"].Rows[rowIndex].SetField(4, tb);
        }

        private void datagv3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel4.Enabled = true;
            int rowIndex = datagv3.CurrentRow.Index;
            tbtoan.Text = datagv3.Rows[rowIndex].Cells[1].Value.ToString();
            tbvan.Text = datagv3.Rows[rowIndex].Cells[2].Value.ToString();
            tbanh.Text = datagv3.Rows[rowIndex].Cells[3].Value.ToString();
        }
        //xoa diem
        private void btxoa2_Click(object sender, EventArgs e)
        {
            dsSinhVien.Tables["Diem"].Rows.RemoveAt(vt);
        }       
        private void datagv3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) vt = e.RowIndex;
        }

        private void btluu2_Click(object sender, EventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(dsSinhVien);
            System.IO.File.WriteAllText("dulieu.json", jsonstr);
        }

        private void xeploai_Click(object sender, EventArgs e)
        {
            cbxeploai.Visible = true;
            cbgioitinh.Visible = false;
            cbquequan.Visible = false;
            dttk = taobang2();
            tim = "Diem";
        }

        private void gioitinh_Click(object sender, EventArgs e)
        {
            cbxeploai.Visible = false;
            cbgioitinh.Visible = true;
            cbquequan.Visible = false;
            dttk = taobang1();
            tim = "GT";
        }

        private void quequan_Click(object sender, EventArgs e)
        {
            cbxeploai.Visible = false;
            cbgioitinh.Visible = false;
            cbquequan.Visible = true;
            tim = "QQ";
            dttk = taobang1();
            cbquequan.DataSource = dsSinhVien.Tables["Tinh"];
            cbquequan.DisplayMember = dsSinhVien.Tables["Tinh"].Columns["Tentinh"].ToString();
        }
        DataTable dttk ;
        string tim;
        private void bttim_Click(object sender, EventArgs e)
        {
            dttk.Rows.Clear();
            if (tim == "GT")
            {
                string dieukien = "Gioitinh = '" + cbgioitinh.Text + "' ";
                foreach (DataRow x in dsSinhVien.Tables["SinhVien"].Select(dieukien))
                {
                    dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString());
                }
            }
            if (tim == "Diem")
            {
                string dieukien = null;
                if (cbxeploai.Text == "Hoc Bong") dieukien = "TB > 8";
                if (cbxeploai.Text == "Canh Cao") dieukien = "TB < 4";
                foreach (DataRow x in dsSinhVien.Tables["Diem"].Select(dieukien))
                {
                    dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString());
                }
            }
            if (tim == "QQ")
            {
                string dieukien = "Quequan = '" + cbquequan.Text + "'";
                foreach (DataRow x in dsSinhVien.Tables["SinhVien"].Select(dieukien))
                {
                    dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString());
                }
            }
            datagv4.DataSource = dttk;
            autoSize(datagv4);
        }
    }
}
