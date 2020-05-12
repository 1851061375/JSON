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
namespace PhongKham2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dtKH;
        private DataTable taobang1()
        {
            DataTable dt = new DataTable("ThongTin");
            dt.Columns.Add("Hoten");
            dt.Columns.Add("Ngaysinh");
            dt.Columns.Add("SDT");
            dt.Columns.Add("Diachi");
            dt.Columns.Add("Ngaykham");
            dt.Columns.Add("Caovoi");
            dt.Columns.Add("Taytrang");
            dt.Columns.Add("Chuphinh");
            dt.Columns.Add("LayCao");
            dt.Columns.Add("Hanrang");
            dt.Columns.Add("Soluong");
            dt.Columns.Add("Tongtien");
            return dt;
        }

        private int tinhTien()
        {
            int sum = 0;
            if (cbcaovoi.Checked)
                sum += Convert.ToInt32(tbcaovoi.Text);
            if (cbtaytrang.Checked)
                sum += Convert.ToInt32(tbtaytrang.Text);
            if (cbchuphinh.Checked)
                sum += Convert.ToInt32(tbchuphinh.Text);
            if (cblaycao.Checked)
                sum += Convert.ToInt32(tblaycao.Text);
            if (cbhanrang.Checked)
                sum += Convert.ToInt32(numericUpDown1.Value)* 90000;
            return sum;
        }
        private void autoSize(DataGridView dtgv)
        {
            foreach (DataGridViewColumn i in dtgv.Columns)
                i.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void bttong_Click(object sender, EventArgs e)
        {
            if (tbhoten.Text == "" || tbsdt.Text == "" || tbdiachi.Text == "")
            {
                MessageBox.Show("Nhap thieu thong tin!");
            }      
            else
            {
                tbtong.Text = Convert.ToString(tinhTien());
                dtKH.Rows.Add(tbhoten.Text, dtngaysinh.Text,tbsdt.Text, tbdiachi.Text, dtngaykham.Text, (cbcaovoi.Checked) ? "x" : "", (cbtaytrang.Checked) ? "x" : "",
                   (cbchuphinh.Checked) ? "x" : "", (cblaycao.Checked) ? "x" : "", (cbhanrang.Checked) ? "x" : "", numericUpDown1.Value, tbtong.Text);
                datagv1.DataSource = dtKH;
                autoSize(datagv1);
                refresh();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("dulieu.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("dulieu.json");
                string jsonstr = reader.ReadToEnd();
                dtKH = JsonConvert.DeserializeObject<DataTable>(jsonstr);
                datagv1.DataSource = dtKH;
                autoSize(datagv1);
                reader.Close();
            }
            else
            {
                dtKH = taobang1();
            }
        }
        private void refresh()
        {
            tbhoten.Clear();
            tbsdt.Clear();
            tbdiachi.Clear();
            dtngaysinh.Text = "1/1/2000";
            dtngaykham.Text = "5/6/2020";
            cbcaovoi.Checked = false;
            cbtaytrang.Checked = false;
            cbchuphinh.Checked = false;
            cblaycao.Checked = false;
            cbhanrang.Checked = false;
            numericUpDown1.Value = 0;
            tbtong.Clear();
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(dtKH);
            System.IO.File.WriteAllText("dulieu.json", jsonstr);
        }

        private void bttim_Click(object sender, EventArgs e)
        {
            DataTable dttk = taobang1();
            string dieukien = "SDT = '" + tbtimsdt.Text + "'";
            foreach (DataRow x in dtKH.Select(dieukien))
            {
                dttk.Rows.Add(x[0].ToString(), x[1].ToString(), x[2].ToString(), x[3].ToString(), x[4].ToString(), x[5].ToString(), x[6].ToString(),
                    x[7].ToString(), x[8].ToString(), x[9].ToString(), x[10].ToString(), x[11].ToString());
            }
            datagv2.DataSource = dttk;
            autoSize(datagv2);
        }

        private void datagv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = datagv1.CurrentRow.Index;
            tbhoten.Text = datagv1.Rows[rowIndex].Cells[0].Value.ToString();
        }
    }
}
