using FontAwesome.Sharp;
using MainMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayerUI
{
    public partial class frmUserRoles : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);

        public frmUserRoles()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        public void getall()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [RoleID],[Role],[IsActive]  FROM [Limited_DB].[dbo].[UserRoles]", con);
            adapter.Fill(dt);

            dgvUserDtls.DataSource = dt;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsActive;

            if (CheckboxIsActive.Checked == true)
                IsActive = true;
            else
                IsActive = false;

           

            if (userroleID.Text!="")
            {
                DialogResult dresult = MessageBox.Show("Do you want to Update ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryUPD = string.Empty;
                queryUPD = "UPDATE UserRoles SET Role = '" + txtUserGroup.Text + "', " +
                           "DateLastModified = '" + DateTime.Now + "', " +
                           "UserLastModified = '" + CommonDev.loggedUser + "', " +
                           "IsActive = '" + IsActive + "' WHERE RoleID IN('" + userroleID.Text + "')";
                CommonDev.ExecuteStatement(queryUPD, CommonDev.connstr);

                MessageBox.Show("User Details Updated Successfully.");
                getall();
                return;
            }
            else
            {
                DialogResult dresult = MessageBox.Show("Do you want to Save ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryINS = string.Empty;
                queryINS = "INSERT INTO UserRoles(Role,IsActive,DateCreated,UserCreated) " +
                           "VALUES('" + txtUserGroup.Text.Trim() + "','" + IsActive + "', " +
                           "'" + DateTime.Now + "','" + CommonDev.loggedUser + "')";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);

                MessageBox.Show("User Group Added Successfully.");
                getall();
                return;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserGroup.Text = "";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtEN_Search_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [RoleID],[Role],[IsActive]  FROM [Limited_DB].[dbo].[UserRoles] WHERE Role like  '%" + txtEN_Search.Text + "%'", con);
            adapter.Fill(dt);

            dgvUserDtls.DataSource = dt;
        }

        private void dgvUserDtls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var senderGrid = (DataGridView)sender;
                int selectedrowindex = dgvUserDtls.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvUserDtls.Rows[selectedrowindex];
                string ID = Convert.ToString(selectedRow.Cells["RoleID"].Value);
                userroleID.Text = ID;

                string query = string.Empty;
                query = "SELECT [RoleID],[Role],[IsActive]  FROM [Limited_DB].[dbo].[UserRoles] WHERE RoleID IN('" + ID + "')";
                DataTable dt = new DataTable();
                dt = CommonDev.SelectMultipleString(query, CommonDev.connstr);
                if (dt.Rows.Count > 0)
                {
                    bool activation;

                    txtUserGroup.Text = dt.Rows[0]["Role"].ToString();
                    activation = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    if (activation == true)
                        CheckboxIsActive.Checked = true;
                    else
                        CheckboxIsActive.Checked = false;

                }
            }
        }

        private void frmUserRoles_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            getall();
        }
    }
}
