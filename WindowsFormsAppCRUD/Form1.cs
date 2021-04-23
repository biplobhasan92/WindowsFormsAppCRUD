using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WindowsFormsAppCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        DBConnection db = new DBConnection();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        int getSelID = 0;

        private void btn_reset_Click(object sender, EventArgs e)
        {
            Clear();
        }


        public void Clear()
        {
            txtName.Text  ="";
            txtEmail.Text ="";
            getSelID = 0;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlConnection(db.connString);
                if (getSelID==0)
                {
                    string s = @" insert into Employee values('" + txtName.Text.Trim() + "', '" + txtEmail.Text.Trim() + "')";
                    cmd = new SqlCommand(s, conn);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (i > 0)
                        MessageBox.Show(" data inserted ");
                }
                else
                {
                    string s = @" update Employee set emp_name = '"+txtName.Text+"', emp_email = '"+txtEmail.Text+"' " +
                        "where emp_id = "+getSelID+" ";
                    cmd = new SqlCommand(s, conn);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (i > 0)
                        MessageBox.Show(" data updated ");
                }
                LoadData();
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(" ex "+ex.Message);
            }
        }


        public void LoadData()
        {
            try
            {
                conn = new SqlConnection(db.connString);
                string s = " select * from Employee";
                da = new SqlDataAdapter(s, conn);
                ds = new DataSet();
                da.Fill(ds);
                dgvEmployee.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(" ex " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvEmployee_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            getSelID = Convert.ToInt32(dgvEmployee.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = dgvEmployee.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtEmail.Text = dgvEmployee.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(" do you wane to delete ? "," click yes or no ", MessageBoxButtons.YesNo)==DialogResult.No) { Clear();  return; }
                conn = new SqlConnection(db.connString);
                string s = @"Delete from Employee where emp_id = "+getSelID;
                cmd = new SqlCommand(s, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(" " + ex.Message);
            }
        }
    }
}
