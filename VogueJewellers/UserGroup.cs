using FontAwesome.Sharp;
using MainMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlayerUI
{
    public partial class UserGroup : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);

        public UserGroup()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void getallusers()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID],[UserGroup],[IsActive] FROM [Limited_DB].[dbo].[UserGroups]", con);
            adapter.Fill(dt);

            dgvUserDtls.DataSource = dt;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            getallusers();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsActive;

            if (CheckboxIsActive.Checked == true)
                IsActive = true;
            else
                IsActive = false;
            
            if (usergroupID.Text == "")
            {

                DialogResult dresult = MessageBox.Show("Do you want to Save ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryINS = string.Empty;
                queryINS = "INSERT INTO UserGroups(UserGroup,IsActive,DateCreated,UserCreated) " +
                           "VALUES('" + txtUserGroup.Text.Trim() + "','" + IsActive + "', " +
                           "'" + DateTime.Now + "','" + CommonDev.loggedUser + "')";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);

                MessageBox.Show("User Group Added Successfully.");
                getallusers();
                return;
            }
            else
            {

                DialogResult dresult = MessageBox.Show("Do you want to Update ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryUPD = string.Empty;
                queryUPD = "UPDATE UserGroups SET UserGroup = '" + txtUserGroup.Text + "', " +
                           "DateLastModified = '" + DateTime.Now + "', " +
                           "UserLastModified = '" + CommonDev.loggedUser + "', " +
                           "IsActive = '" + IsActive + "' WHERE ID IN('" + usergroupID.Text + "')";
                CommonDev.ExecuteStatement(queryUPD, CommonDev.connstr);

                MessageBox.Show("User Details Updated Successfully.");
                getallusers();
                return;


            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserGroup.Text = string.Empty;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvUserDtls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var senderGrid = (DataGridView)sender;
                int selectedrowindex = dgvUserDtls.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvUserDtls.Rows[selectedrowindex];
                string ID = Convert.ToString(selectedRow.Cells["ID"].Value);
                usergroupID.Text = ID;

                string query = string.Empty;
                query = "SELECT [ID],[UserGroup],[IsActive] FROM [Limited_DB].[dbo].[UserGroups] WHERE ID IN('" + ID + "')";
                DataTable dt = new DataTable();
                dt = CommonDev.SelectMultipleString(query, CommonDev.connstr);
                if (dt.Rows.Count > 0)
                {
                    bool activation;

                    txtUserGroup.Text = dt.Rows[0]["UserGroup"].ToString();
                    activation = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    if (activation == true)
                        CheckboxIsActive.Checked = true;
                    else
                        CheckboxIsActive.Checked = false;

                }
            }
        }

        private void txtEN_Search_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID],[UserGroup],[IsActive] FROM [Limited_DB].[dbo].[UserGroups] Where UserGroup like  '%" + txtEN_Search.Text + "%'", con);
            adapter.Fill(dt);

            dgvUserDtls.DataSource = dt;
        }
    }
}
