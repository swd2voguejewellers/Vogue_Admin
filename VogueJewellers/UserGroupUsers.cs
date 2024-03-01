using FontAwesome.Sharp;
using MainMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace VogueJewellers
{
    public partial class UserGroupUsers : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);
        SqlConnection conHR = new SqlConnection(CommonDev.connstrHR);
        DataTable dtnew = new DataTable();

        public UserGroupUsers()
        {
            InitializeComponent();
        }

        public void allusers()
        {
            dgvUsers.DataSource = null;
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT '' as ID,[EmployeeID],[FirstName]+' ' + LastName As Name  FROM [HR_Attendance].[dbo].[Staff] WHERE [Status]=1", conHR);
            adapter.Fill(dt);
            conHR.Open();
            dgvUsers.DataSource = dt;
        }

        public void loadgroups()
        {
            if (cmbUserGroup.SelectedIndex != -1)
            {
                string groupid = cmbUserGroup.SelectedValue.ToString();
                if (groupid != "System.Data.DataRowView")
                {
                    dgvGroupusers.DataSource = null;

                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID],[EPFNo],HR_Attendance.dbo.Staff.FirstName +' '+HR_Attendance.dbo.Staff.LastName AS Name  FROM [Limited_DB].[dbo].[UserGroupUsers] INNER JOIN HR_Attendance.dbo.Staff ON  HR_Attendance.dbo.Staff.EmployeeID = [Limited_DB].[dbo].[UserGroupUsers].[EPFNo] WHERE [Limited_DB].[dbo].[UserGroupUsers].[GroupID]='" + groupid + "' ", con);
                    adapter.Fill(dt);

                    dgvGroupusers.DataSource = dt;
                    dgvGroupusers.Columns[0].Visible = false;

                }
            }
        }

        public void findallusers(string Epfno, string username)
        {
            if (Epfno != null && username == null)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [EmployeeID],[FirstName]+' ' + LastName As Name  FROM [HR_Attendance].[dbo].[Staff] WHERE [Status]=1 AND EmployeeID = '" + Epfno + "'", con);
                adapter.Fill(dt);

                dgvUsers.DataSource = dt;

            }
            if (username != null && Epfno == null)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [EmployeeID],[FirstName]+' ' + LastName As Name  FROM [HR_Attendance].[dbo].[Staff] WHERE [Status]=1 AND  FirstName like  '%" + username + "%'", con);
                adapter.Fill(dt);

                dgvUsers.DataSource = dt;

            }
        }
        public void LoadUserGroups()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID],[UserGroup] FROM [Limited_DB].[dbo].[UserGroups] WHERE [IsActive]=1", con);
            adapter.Fill(dt);
            con.Open();
            cmbUserGroup.DataSource = dt;
            cmbUserGroup.DisplayMember = "UserGroup";
            cmbUserGroup.ValueMember = "ID";
            cmbUserGroup.SelectedIndex = -1;
        }

        private void UserGroupUsers_Load(object sender, EventArgs e)
        {
            LoadUserGroups();
            allusers();
            dgvUsers.Columns[0].Visible = false;

            this.WindowState = FormWindowState.Maximized;
            dtnew.Columns.AddRange(new DataColumn[3] { new DataColumn("ID"), new DataColumn("EPF"), new DataColumn("Name") });
            dtnew.PrimaryKey = new DataColumn[] { dtnew.Columns["EPF"] };
        }

        private void txtUN_Search_TextChanged(object sender, EventArgs e)
        {
            if (txtUN_Search.Text == "")
            {
                conHR.Close();
                allusers();
            }
            else
            {
                findallusers(null, txtUN_Search.Text);
            }
        }

        private void txtEpfNo_Search_TextChanged(object sender, EventArgs e)
        {
            if (txtEpfNo_Search.Text == "")
            {
                conHR.Close();
                allusers();
            }
            else
            {
                findallusers(txtEpfNo_Search.Text, null);
            }
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;
            int selectedrowindex = dgvUsers.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvUsers.Rows[selectedrowindex];
            string ID = "";
            string EPF = Convert.ToString(selectedRow.Cells[1].Value);
            string Name = Convert.ToString(selectedRow.Cells[2].Value);

            for (int i = 0; i < dtnew.Rows.Count; i++)
            {
                if (dtnew.Rows[i][0].ToString() == EPF)
                {
                    MessageBox.Show("Dupplicate user");
                    return;
                }
            }
            DataRow newRow = dtnew.NewRow();
            newRow["ID"] = ID;
            newRow["EPF"] = EPF;
            newRow["Name"] = Name;
            dtnew.Rows.Add(newRow);

            dgvGroupusers.DataSource = dtnew;
            lblNoOfItems.Text = dtnew.Rows.Count.ToString();
        }

        private void dgvGroupusers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            int selectedrowindex = dgvGroupusers.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvGroupusers.Rows[selectedrowindex];
            string ID = Convert.ToString(selectedRow.Cells[0].Value);
            string EPF = Convert.ToString(selectedRow.Cells[1].Value);

            if (ID == "")
            {
                DataRow employeeRow = dtnew.Rows.Find(EPF);
                if (employeeRow != null)
                {
                    employeeRow.Delete();
                    dtnew.AcceptChanges(); // Commit the deletion
                }

                dgvGroupusers.DataSource = dtnew;
            }else
            {
                string queryINS = string.Empty;
                queryINS = "DELETE FROM [Limited_DB].[dbo]. UserGroupUsers WHERE ID ='" + ID +"'";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);
                loadgroups();
            }


            lblNoOfItems.Text = dtnew.Rows.Count.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsActive = true;

            if (cmbUserGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Uger Group");
                return;
            }
            if (dgvGroupusers.Rows.Count == 0)
            {
                MessageBox.Show("Add Group Users");
                return;
            }

            for (int i = 0; i < dgvGroupusers.Rows.Count; i++)
            {
                int groupid = int.Parse(dgvGroupusers.Rows[i].Cells[1].FormattedValue.ToString());


                DialogResult dresult = MessageBox.Show("Do you want to Save ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryINS = string.Empty;
                queryINS = "INSERT INTO UserGroupUsers(GroupID,EPFNo,IsActive,DateCreated,UserCreated) " +
                           "VALUES('" + cmbUserGroup.SelectedValue + "','" + groupid + "','" + IsActive + "', " +
                           "'" + DateTime.Now + "','" + CommonDev.loggedUser + "')";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);
            }
            MessageBox.Show("User Group Added Successfully.");

            dtnew = new DataTable();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvGroupusers.Rows.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cmbUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadgroups();
        }
    }
}
