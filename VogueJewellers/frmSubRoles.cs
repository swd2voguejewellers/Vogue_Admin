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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace VogueJewellers
{
    public partial class frmSubRoles : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);
        bool IsActive = true;
        public frmSubRoles()
        {
            InitializeComponent();
        }

        public void creategv()
        {

        }

        public void getall()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [RoleID],[Role]  FROM [Limited_DB].[dbo].[UserRoles] WHERE [IsActive]=1", con);
            adapter.Fill(dt);

            con.Open();
            cmbUserRoles.DataSource = dt;
            cmbUserRoles.DisplayMember = "Role";
            cmbUserRoles.ValueMember = "RoleID";
            cmbUserRoles.SelectedIndex = -1;

        }

        public void LoadUserGroups()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID],[UserGroup] FROM [Limited_DB].[dbo].[UserGroups] WHERE [IsActive]=1", con);
            adapter.Fill(dt);
            con.Open();
            cmbuserGroup.DataSource = dt;
            cmbuserGroup.DisplayMember = "UserGroup";
            cmbuserGroup.ValueMember = "ID";
            cmbuserGroup.SelectedIndex = -1;
        }

        private void frmSubRoles_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            getall();
            con.Close();
            LoadUserGroups();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvUserDtls.Rows.Count; i++)
            {
                int groupid = int.Parse(dgvUserDtls.Rows[i].Cells[0].FormattedValue.ToString());
                int roleid = int.Parse(dgvUserDtls.Rows[i].Cells[2].FormattedValue.ToString());
                string subrole = dgvUserDtls.Rows[i].Cells[4].FormattedValue.ToString();

                DialogResult dresult = MessageBox.Show("Do you want to Save ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string queryINS = string.Empty;
                queryINS = "INSERT INTO UserSubRoles(SubRoll,RoleID,UserGroupID,IsActive,DateCreated,UserCreated) " +
                           "VALUES('" + subrole + "','" + roleid + "','" + groupid + "','" + IsActive + "', " +
                           "'" + DateTime.Now + "','" + CommonDev.loggedUser + "')";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);
            }
            MessageBox.Show("Save Succesfully");
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (cmbuserGroup.Text == "")
            {
                MessageBox.Show("Select User Group");
                return;
            }

            if (cmbUserRoles.Text == "")
            {
                MessageBox.Show("Select User Role");
                return;
            }

            if (cmbUserRoles.Text == "")
            {
                MessageBox.Show("Select User sub role");
                return;
            }

            dgvUserDtls.Rows.Add(cmbuserGroup.SelectedValue, cmbuserGroup.Text, cmbUserRoles.SelectedValue, cmbUserRoles.Text, cmbUserRoles.Text + " " + cmbSubRoles.Text);

        }

        private void cmbuserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbuserGroup.SelectedIndex != -1)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT  [SubRID],[SubRoll],[UserGroupID],[RoleID]  FROM [Limited_DB].[dbo].[UserSubRoles] WHERE [IsActive]=1", con);
                adapter.Fill(dt);

                dgvUserDtls.DataSource = dt;
            }


        }
    }
}
