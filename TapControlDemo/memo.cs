using System;
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
    public partial class memo : Form
    {
        private MySqlCommand cmd;
        private DataSet ds;
        private MySqlDataAdapter da;

        //
        koneksi konn = new koneksi();


        public memo()
        {
            InitializeComponent();
        }


        void panggil()
        {
            MySqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new MySqlCommand("SELECT * FROM memo", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "memo");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "memo";
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void memo_Load(object sender, EventArgs e)
        {
            panggil();
        }
    }
}
