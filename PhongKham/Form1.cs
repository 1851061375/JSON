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

namespace PhongKham
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet dsKH = new DataSet();
        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("dulieu.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("dulieu.json");
                dsKH = JsonConvert.DeserializeObject<DataSet>(reader.ReadToEnd());
                reader.Close();
                datagv1.DataSource = dsKH.Tables["KhachHang"];
                autoSize(datagv1);
                datagv2.DataSource = dsKH.Tables["KhachHang"];
                autoSize(datagv2);
                datagv3.DataSource = dsKH.Tables["DichVu"];
                autoSize(datagv3);
                cbhoten.DataSource = dsKH.Tables["KhachHang"];
                cbhoten.DisplayMember = dsKH.Tables["KhachHang"].Columns["Hoten"].ToString();
            }
            else
            {
                DataTable dtKH = taobang1();
                DataTable dtDV = taobang2();


                dsKH.Tables.Add(dtKH);
                dsKH.Tables.Add(dtDV);
            }
            
        }
        private DataTable taobang1()
        {
            DataTable dt = new DataTable("KhachHang");
            dt.Columns.Add("Hoten");
            dt.Columns.Add("Sdt", typeof(int));
            dt.Columns.Add("Ngaysinh");
            dt.Columns.Add("Diachi");
            return dt;
        }
        private DataTable taobang2()
        {
            DataTable dt = new DataTable("DichVu");
            dt.Columns.Add("Hoten");
            dt.Columns.Add("Caovoi");
            dt.Columns.Add("Taytrang");
            dt.Columns.Add("Chuphinh");
            dt.Columns.Add("Laycao");
            dt.Columns.Add("Hanrang");
            dt.Columns.Add("Soluong");
            dt.Columns.Add("Ngaykham");
            dt.Columns.Add("Tongtien");
            return dt;
        }

        
        private void autoSize (DataGridView dtgv)
        {
            foreach (DataGridViewColumn i in dtgv.Columns)
            {
                i.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void refresh()
        {
            tbhoten.Clear();
            tbsdt.Clear();
            dtngaysinh.Text = "1/1/2000";
            tbdiachi.Clear();
        }
        int id = 0;
        private void btthem_Click(object sender, EventArgs e)
        {
            id++;
            dsKH.Tables["KhachHang"].Rows.Add(tbhoten.Text, tbsdt.Text, dtngaysinh.Value.ToShortDateString(), tbdiachi.Text);
            dsKH.Tables["DichVu"].Rows.Add(tbhoten.Text, "", "", "", "", "", "", "", "");
            datagv1.DataSource = dsKH.Tables["KhachHang"]; 
            datagv2.DataSource = dsKH.Tables["KhachHang"]; 
            datagv3.DataSource = dsKH.Tables["DichVu"];
            //
            cbhoten.DataSource = dsKH.Tables["KhachHang"];
            cbhoten.DisplayMember = dsKH.Tables["KhachHang"].Columns["Hoten"].ToString();
            if (id == 1)
            {
                autoSize(datagv1);
                autoSize(datagv2);
                autoSize(datagv3);
            }
            refresh();
        }
        private void datagv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = datagv2.CurrentRow.Index;
            tbhoten.Text = datagv2.Rows[rowIndex].Cells[0].Value.ToString();
            tbsdt.Text = datagv2.Rows[rowIndex].Cells[1].Value.ToString();
            dtngaysinh.Text = datagv2.Rows[rowIndex].Cells[2].Value.ToString();
            tbdiachi.Text = datagv2.Rows[rowIndex].Cells[3].Value.ToString();
        }
        private void btsua_Click(object sender, EventArgs e)
        {
            int rowIndex = datagv2.CurrentRow.Index;
            dsKH.Tables["KhachHang"].Rows[rowIndex].SetField(0, tbhoten.Text);
            dsKH.Tables["KhachHang"].Rows[rowIndex].SetField(1, tbsdt.Text);
            dsKH.Tables["KhachHang"].Rows[rowIndex].SetField(2, dtngaysinh.Value.ToShortDateString());
            dsKH.Tables["KhachHang"].Rows[rowIndex].SetField(3, tbdiachi.Text);
            dsKH.Tables["DichVu"].Rows[rowIndex].SetField(0, tbhoten.Text);
        }
        int vt;
        private void datagv2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) vt = e.RowIndex;
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            dsKH.Tables["KhachHang"].Rows.RemoveAt(vt);
            dsKH.Tables["DichVu"].Rows.RemoveAt(vt);
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(dsKH);
            System.IO.File.WriteAllText("dulieu.json", jsonstr);
        }

        private void bttong_Click(object sender, EventArgs e)
        {
            int sum = 0;
            int rowIndex = cbhoten.SelectedIndex;
            if (cbcaovoi.Checked)
            {
                sum += Convert.ToInt32(tbcaovoi.Text);
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(1, "x");
            }
            if (cbtaytrang.Checked)
            {
                sum += Convert.ToInt32(tbtaytrang.Text);
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(2, "x");
            }
            if (cbchuphinh.Checked)
            {
                sum += Convert.ToInt32(tbchuphinh.Text);
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(3, "x");
            }
            if (cblaycao.Checked)
            {
                sum += Convert.ToInt32(tblaycao.Text);
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(4, "x");
            }
            if (cbhanrang.Checked)
            {
                sum += Convert.ToInt32(numericUpDown1.Value * 90000);
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(5, "x");
                dsKH.Tables["DichVu"].Rows[rowIndex].SetField(6, numericUpDown1.Value);
            }
            tbtong.Text = Convert.ToString(sum);
            dsKH.Tables["DichVu"].Rows[rowIndex].SetField(7, dtngaykham.Value.ToShortDateString());
            dsKH.Tables["DichVu"].Rows[rowIndex].SetField(8, tbtong.Text);
        }

        private void btluu2_Click(object sender, EventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(dsKH);
            System.IO.File.WriteAllText("dulieu.json",jsonstr);
        }

        DataTable dttk = new DataTable();
        int tim;

        private void ngay_Click(object sender, EventArgs e)
        {
            tim = 1;
            cbngay.Visible = true;
            cbdichvu.Visible = false;
            cbsdt.Visible = false;
            cbngay.DataSource = dsKH.Tables["DichVu"];
            cbngay.DisplayMember = dsKH.Tables["DichVu"].Columns["Ngaykham"].ToString();
            dttk = taobang2();
        }

        private void loaiDV_Click(object sender, EventArgs e)
        {
            tim = 2;
            cbngay.Visible = false;
            cbdichvu.Visible = true;
            cbsdt.Visible = false;
            dttk = taobang2();
        }

        private void sdt_Click(object sender, EventArgs e)
        {
            tim = 3;
            cbngay.Visible = false;
            cbdichvu.Visible = false;
            cbsdt.Visible = true;
            cbsdt.DataSource = dsKH.Tables["KhachHang"];
            cbsdt.DisplayMember = dsKH.Tables["KhachHang"].Columns["Sdt"].ToString();
            dttk = taobang1();
        }
        private void bttim_Click(object sender, EventArgs e)
        {
            dttk.Clear();
            if (tim == 1)
            {
                string dieukien = "Ngaykham = '" + cbngay.Text + "'";
                foreach(DataRow x in dsKH.Tables["DichVu"].Select(dieukien))
                {
                    dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString(), x[5].ToString(), x[6].ToString(), x[7].ToString(),x[8].ToString());
                }
            }
            // chua dung
            if (tim == 2)
            {
                if (cbdichvu.Text == "Cao voi")
                {
                    string dieukien = "Caovoi = 'x'";
                    foreach (DataRow x in dsKH.Tables["DichVu"].Select(dieukien))
                    {
                        dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString(), x[5].ToString(), x[6].ToString(), x[7].ToString(), x[8].ToString());
                    }
                }
                
            }
            if (tim == 3)
            {
                string dieukien = "Sdt = '" + cbsdt.Text + "'";
                foreach(DataRow x in dsKH.Tables["KhachHang"].Select(dieukien))
                    {
                    dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString());
                    }
            }
            datagv4.DataSource = dttk;
            autoSize(datagv4);
        }
    }
}
