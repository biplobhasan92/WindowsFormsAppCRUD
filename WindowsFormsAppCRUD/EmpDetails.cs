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
    public partial class EmpDetails : Form
    {
        DBConnection db = new DBConnection();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        int getSelIndex = 0;

        public EmpDetails()
        {
            InitializeComponent();
        }

        private void EmpDetails_Load(object sender, EventArgs e)
        {
            LoadDepartment();
            LoadGrid();
        }


        void LoadDepartment()
        {
            conn = new SqlConnection(db.connString);
            string s = " select * from Department";
            da = new SqlDataAdapter(s, conn);
            ds = new DataSet();
            dt = new DataTable();
            da.Fill(ds);
            dt = ds.Tables[0];

            DataRow dr = dt.NewRow();
            dr[0] = 0;
            dr[1] = " -- Select -- ";
            dt.Rows.InsertAt(dr,0);

            cbxDept.DisplayMember = "dept_name";
            cbxDept.ValueMember = "dept_id";
            cbxDept.DataSource = dt;
        }



        private void btn_save_Click(object sender, EventArgs e)
        {
            string gender = "N/A";
            conn = new SqlConnection(db.connString);
            if (string.IsNullOrEmpty(txtName.Text.Trim())) { MessageBox.Show(" Please Write Name "); return;}
            if (Convert.ToInt32(cbxDept.SelectedValue)==0) { MessageBox.Show(" Please select Department "); return;}
            if (rdoMale.Checked){gender = "Male";}else{gender = "Female";}

            try
            {
                if (getSelIndex==0)
                {
                    string s = @" insert into EmpDetails values('" + txtName.Text.Trim() + "', " + (cbxDept.SelectedValue) + ", '" + gender + "' )";
                    cmd = new SqlCommand(s, conn);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    string s = @" Update EmpDetails set emp_name ='"+txtName.Text.Trim()+ "', emp_department= "+(cbxDept.SelectedValue)+", emp_gender = '"+gender+"' " +
                        " where id ="+getSelIndex;
                    cmd = new SqlCommand(s, conn);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                LoadGrid();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Exception "+ex.Message);
            }
        }


        void LoadGrid()
        {
            conn = new SqlConnection(db.connString);
            try
            {
                string s = @" Select 
                                e.id, 
                                e.emp_name,
                                (select d.dept_name from Department d where d.dept_id=e.emp_department) as emp_department,
                                e.emp_gender
                            from EmpDetails e ";
                da = new SqlDataAdapter(s, conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Exception " + ex.Message);
            }
        }


        void Clear()
        {
            txtName.Text = cbxDept.Text = "";
            getSelIndex = 0;
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            getSelIndex  = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cbxDept.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()=="Male")
            {rdoMale.Checked = true;}else{rdoFemale.Checked = true;}
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (DialogResult.No==MessageBox.Show("Do you want to Delete it ? ", " Confirmation ", MessageBoxButtons.YesNo)) { return; }
            try
            {
                conn = new SqlConnection(db.connString);
                string s = " delete from EmpDetails where id ="+getSelIndex;
                cmd = new SqlCommand(s, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Exception : "+ex.Message);
            }
            Clear();
            LoadGrid();
        }
    }
}
