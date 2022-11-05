﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TapControlDemo
{
    public partial class frm2 : Form
    {
        private MySqlCommand cmd;
        private DataSet ds;
        private MySqlDataAdapter da;

        //
        koneksi konn = new koneksi();

        public frm2()
        {
            InitializeComponent();
            radioButton1.Checked = true;

        }


        void hitungRow()
        {
            MySqlConnection conn = konn.GetConn();
            conn.Open();
            string query = @"SELECT COUNT(id) FROM route";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            label1.Text = "ROW ID: " + dr[0].ToString();
            conn.Close();
        }
        void panggil()
        {
            MySqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new MySqlCommand("SELECT * FROM route", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "route");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "route";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                conn.Close();


            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        void dgv()
        {
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.Columns[0].HeaderText = "ID";
        dataGridView1.Columns[1].HeaderText = "ShipToCompany";
        dataGridView1.Columns[2].HeaderText = "Address";
        dataGridView1.Columns[3].HeaderText = "Reason/Remarks";
        dataGridView1.Columns[4].HeaderText = "Area";
        }
        void otomatis()
        {
        long hitung;
        string urutan;
        MySqlDataReader rd;
        MySqlConnection conn = konn.GetConn();
        conn.Open();
        cmd = new MySqlCommand("select id from route where id in (select max(id) from route) order by id desc", conn);
        rd = cmd.ExecuteReader();
        rd.Read();
        if (rd.HasRows)
         {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["id"].ToString().Length - 3, 3)) + 1;
                string kodeurutan = "000" + hitung;
                urutan = "KLS" + kodeurutan.Substring(kodeurutan.Length - 3, 3);
            }
            else
            {
                urutan = "KLS001";
            }
            rd.Close();
            textBox5.Text = urutan;
            conn.Close();
        }

        void bersihkan()
        {
        textBox1.Text = "";
        textBox2.Text = "";
        textBox3.Text = "";
        textBox4.Text = "";
        comboBox1.Text = "";
        otomatis();
        }

            void carialamat()
            {
            MySqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new MySqlCommand("SELECT * FROM route where id like '%" + textBox1.Text + "%' or shiptoname like '%" + textBox1.Text + "%' or address like '%" + textBox1.Text + "%' or area like '%" + textBox1.Text + "%' or remarks like '%" + textBox1.Text + "%' or address like '%" + textBox1.Text + "%'", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "route");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "route";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
            
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            carialamat();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //CREATE
            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                MySqlConnection conn = konn.GetConn();
                try
                {
                    cmd = new MySqlCommand("INSERT INTO route values ('" + textBox5.Text + "', '" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + comboBox1.Text + "')", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Save Completed");
                    panggil();
                    hitungRow();

                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DELETE
            if (MessageBox.Show("Apakah yakin akan menghapus data " + textBox2.Text + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
            {
                MySqlConnection conn = konn.GetConn();
                {
                    cmd = new MySqlCommand("DELETE FROM route WHERE id ='" + textBox5.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Delete Data Berhasil");
                    panggil();
                    hitungRow();

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {   
            //UPDATE
            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                MySqlConnection conn = konn.GetConn();
                try
                {
                    cmd = new MySqlCommand("UPDATE route Set shiptoname='" + textBox2.Text + "',address= '" + textBox3.Text + "',remarks='" + textBox4.Text + "',area='" + comboBox1.Text + "' where id='" + textBox5.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update/Edit Data Berhasil");
                    panggil();
                    bersihkan();
                    hitungRow();
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());


                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Enabled = false;
            button3.ForeColor = Color.LightBlue;
            button3.Visible = false;
            

            try
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox5.Text = row.Cells["id"].Value.ToString();
                textBox2.Text = row.Cells["shiptoname"].Value.ToString();
                textBox3.Text = row.Cells["address"].Value.ToString();
                textBox4.Text = row.Cells["remarks"].Value.ToString();
                comboBox1.Text = row.Cells["area"].Value.ToString();

            }
            catch (Exception X)
            {
                MessageBox.Show(X.ToString());

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new MySqlCommand("SELECT * FROM route", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "route");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "route";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                conn.Close();


            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void frm2_Load(object sender, EventArgs e)
        {
            otomatis();
            button1.BackColor = Color.LightBlue;
            button2.BackColor = Color.LightBlue;
            button3.BackColor = Color.LightBlue;
            button4.BackColor = Color.LightBlue;
            button5.BackColor = Color.LightBlue;
            textBox1.BackColor = Color.LightBlue;
            textBox2.BackColor = Color.LightBlue;
            textBox3.BackColor = Color.LightBlue;
            textBox4.BackColor = Color.LightBlue;
            textBox5.BackColor = Color.LightBlue;
            comboBox1.BackColor = Color.LightBlue;

            textBox5.Enabled = false;
            button1.Text = "Update";
            label2.Text = "ID";
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.Text = "ShipToName";
            label3.ForeColor = Color.Black;
            label4.Text = "Address";
            label4.ForeColor = Color.Black;
            label5.Text = "Remarks";
            label5.ForeColor = Color.Black;
            label6.Text = "Area";
            label6.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridView1.ColumnHeadersHeight = 20;

            this.BackColor = Color.WhiteSmoke;
            dgv();
            this.dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            // Get the hostname


            string myHost = System.Net.Dns.GetHostName();

            // Show the hostname

            label7.Text = myHost;
            label7.ForeColor = Color.Black;

            // Get the IP from the host name

            string myIP = System.Net.Dns.GetHostByName(myHost).AddressList[0].ToString();

            // Show the IP

            label8.Text = myIP;
            label8.ForeColor = Color.Black;
            hitungRow();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            button3.Enabled = true;
            button3.Visible = true;
            button3.ForeColor = Color.Black;
            bersihkan();

            otomatis();
            
        }
    }
}
