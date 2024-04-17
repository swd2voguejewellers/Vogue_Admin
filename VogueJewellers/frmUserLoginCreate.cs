using MainMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace User_Creation
{
    public partial class frmUserLoginCreate : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);
        string type;
        public frmUserLoginCreate()
        {
            InitializeComponent();
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void getallusers()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserDetails.UserID, UserDetails.EPFNo, UserDetails.UserName as UserName, UserDetails.IsActive, UserGroups.UserGroup\r\nFROM UserDetails INNER JOIN  UserGroups ON UserDetails.UserGroupID = UserGroups.ID", con);
            adapter.Fill(dt);

            dgvUserDtls.DataSource = dt;
            dgvUserDtls.Columns[0].Visible = false;

        }
        public void getallaccesbranchres(string empNo)
        {
            DataTable dt1 = new DataTable();
            SqlDataAdapter adapter1 = new SqlDataAdapter("SELECT BranchAccess.BranchID as BranchID, BranchMaster.BranchName, BranchAccess.EmpNo FROM BranchAccess INNER JOIN BranchMaster ON BranchAccess.BranchID = BranchMaster.BranchID\r\nWHERE (BranchAccess.IsActive = 1 AND EmpNo='" + empNo + "')", con);
            adapter1.Fill(dt1);

            if (dgvBranchers.Rows.Count > 0)
            {
                for (int i = 0; i < dgvBranchers.Rows.Count; i++)
                {
                    string id = dgvBranchers.Rows[i].Cells[0].FormattedValue.ToString();

                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (id == dt1.Rows[j]["BranchID"].ToString())
                        {
                            dgvBranchers.Rows[i].Cells["IsAccess"].Value = true;
                        }
                    }

                }

            }

        }

        public void getallBranchers()
        {
            dgvBranchers.DataSource = null;
            dgvBranchers.Rows.Clear();

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [BranchID],[BranchName]  FROM [Limited_DB].[dbo].[BranchMaster] WHERE [IsActive] = 1", con);
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dgvBranchers.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }

        }

        public void findallusers(string Epfno, string username)
        {
            if (Epfno != null && username == null)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserDetails.UserID, UserDetails.EPFNo, UserDetails.UserName as UserName, UserDetails.IsActive, UserGroups.UserGroup\r\nFROM UserDetails INNER JOIN  UserGroups ON UserDetails.UserGroupID = UserGroups.ID WHERE UserDetails.EPFNo = '" + Epfno + "'", con);
                adapter.Fill(dt);

                dgvUserDtls.DataSource = dt;

            }
            if (username != null && Epfno == null)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserDetails.UserID, UserDetails.EPFNo, UserDetails.UserName as UserName, UserDetails.IsActive, UserGroups.UserGroup\r\nFROM UserDetails INNER JOIN  UserGroups ON UserDetails.UserGroupID = UserGroups.ID WHERE UserDetails.UserName like  '%" + username + "%'", con);
                adapter.Fill(dt);

                dgvUserDtls.DataSource = dt;

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

        private void frmUserLoginCreate_Load(object sender, EventArgs e)
        {
            SetItemGridView();
            ClearFields();
            LoadUserGroups();
            getallusers();
            getallBranchers();

        }

        private void SetItemGridView()
        {
            //try
            //{
            //    dgvUserDtls.Columns.Clear();

            //    var col1 = new DataGridViewTextBoxColumn(); var col2 = new DataGridViewTextBoxColumn();
            //    var col3 = new DataGridViewTextBoxColumn(); var col4 = new DataGridViewTextBoxColumn();

            //    col1.HeaderText = "User Name"; col1.Name = "UserName"; col1.Width = 150;
            //    col2.HeaderText = "Level"; col2.Name = "Level"; col2.Width = 100;
            //    col3.HeaderText = "EmpNo"; col3.Name = "EmpNo"; col3.Width = 100;
            //    col4.HeaderText = "Branch"; col4.Name = "Branch"; col4.Width = 150;

            //    dgvUserDtls.Columns.AddRange(new DataGridViewColumn[] { col1, col2, col3, col4 });
            //    foreach (DataGridViewColumn column in dgvUserDtls.Columns)
            //    {
            //        column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //    }
            //    dgvUserDtls.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            //    dgvUserDtls.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //    dgvUserDtls.EnableHeadersVisualStyles = false;

            //    dgvUserDtls.Columns["UserName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgvUserDtls.Columns["Level"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgvUserDtls.Columns["EmpNo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgvUserDtls.Columns["Branch"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //    dgvUserDtls.Columns["UserName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //    dgvUserDtls.Columns["Level"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgvUserDtls.Columns["EmpNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgvUserDtls.Columns["Branch"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //}
            //catch (Exception)
            //{

            //}
        }

        private void ClearFields()
        {
            txtUserName.Text = string.Empty;
            txtPass.Text = string.Empty;
            txtConfirmPass.Text = string.Empty;
            txtEmpNo.Text = string.Empty;
            chkShowPassword.Checked = false;
            cmbUserGroup.SelectedIndex = -1;
            txtFullName.Text = string.Empty;
            getallBranchers();
            checkBoxLtdi.Checked = false;
        }

        //private void LoadAllUsers() 
        //{
        //    string query = string.Empty;
        //    query = "SELECT UserName, Level, EMPNO, Branch FROM Users ORDER BY EMPNO";
        //    DataTable dt = new DataTable();
        //    dt = CommonDev.SelectMultipleString(query, CommonDev.connstrDataCommon);
        //    dgvUserDtls.Rows.Clear();
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            string brname = string.Empty;
        //            if (dt.Rows[i]["Branch"].ToString() == "1")
        //            {
        //                brname = "Kollupitiya";
        //            }
        //            else if (dt.Rows[i]["Branch"].ToString() == "3")
        //            {
        //                brname = "Negombo";
        //            }
        //            else if (dt.Rows[i]["Branch"].ToString() == "4")
        //            {
        //                brname = "Nugegoda";
        //            }
        //            else if (dt.Rows[i]["Branch"].ToString() == "5")
        //            {
        //                brname = "Kurunegala";
        //            }
        //            else if (dt.Rows[i]["Branch"].ToString() == "7")
        //            {
        //                brname = "KCC";
        //            }
        //            else if (dt.Rows[i]["Branch"].ToString() == "2")
        //            {
        //                brname = "KCC";
        //            }

        //            dgvUserDtls.Rows.Add(dt.Rows[i]["UserName"].ToString(),
        //                                 dt.Rows[i]["Level"].ToString(),
        //                                 dt.Rows[i]["EMPNO"].ToString(),
        //                                 brname);

        //            dgvUserDtls.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
        //            dgvUserDtls.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
        //            dgvUserDtls.Rows[i].Cells[2].Style.ForeColor = Color.Blue;
        //            dgvUserDtls.Rows[i].Cells[3].Style.ForeColor = Color.Blue;

        //            DataGridViewCellStyle style = new DataGridViewCellStyle();
        //            style.Font = new Font(dgvUserDtls.Font, FontStyle.Bold);
        //            dgvUserDtls.Rows[i].DefaultCellStyle = style;
        //        }
        //        lblUserCount.Text = dgvUserDtls.Rows.Count.ToString();
        //    }
        //    dgvUserDtls.ClearSelection();
        //}

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtUN_Search.Text = string.Empty;
            txtEpfNo_Search.Text = string.Empty;
            cboxBranch_Search.SelectedIndex = -1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsActive;

            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show("User name is Required.");
                txtUserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPass.Text.Trim()))
            {
                MessageBox.Show("Password is Required.");
                txtPass.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtConfirmPass.Text.Trim()))
            {
                MessageBox.Show("Confirm Password is Required.");
                txtConfirmPass.Focus();
                return;
            }
            if (txtPass.Text.Trim() != txtConfirmPass.Text.Trim())
            {
                MessageBox.Show("Password is not match with Confirm Password.");
                txtConfirmPass.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtFullName.Text.Trim()))
            {
                MessageBox.Show("FullName is Required.");
                return;
            }

            if (cmbUserGroup.SelectedIndex == -1)
            {
                MessageBox.Show("User Group is Required.");
                return;
            }

            if (checkBoxLtd.Checked == true)
                type = "LTD";
            if (checkBoxLtdi.Checked == true)
                type = "LTDi";
            if (checkBoxLtdi.Checked == true && checkBoxLtd.Checked == true)
                type = "LTD/LTDi";

            if (CheckboxIsActive.Checked == true)
                IsActive = true;
            else
                IsActive = false;

            string hash = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, txtConfirmPass.Text.Trim());
            }


            string query = string.Empty;
            query = "SELECT * FROM UserDetails WHERE UserName IN('" + txtUserName.Text.Trim() + "')";
            DataTable dt = new DataTable();
            dt = CommonDev.SelectMultipleString(query, CommonDev.connstr);
            if (dt.Rows.Count > 0)
            {
                DialogResult dresult = MessageBox.Show("Do you want to Update ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryUPD, queryUPD1 = string.Empty;
                queryUPD = "UPDATE UserDetails SET UPassword = '" + hash + "', " +
                           "DateLastModified = '" + DateTime.Now + "', " +
                           "UserCreated = '" + CommonDev.loggedUser + "', " +
                           "EPFNO = '" + txtEmpNo.Text.Trim() + "', " +
                           "IsActive = '" + IsActive + "',Type ='" + type + "', " +
                           "UserGroupID = '" + cmbUserGroup.SelectedValue + "' WHERE UserName IN('" + txtUserName.Text.Trim() + "')";
                CommonDev.ExecuteStatement(queryUPD, CommonDev.connstr);
                CommonDev.ExecuteStatement(" DELETE FROM [Limited_DB].[dbo].[BranchAccess] WHERE [EmpNo]= '" + txtEmpNo.Text.Trim() + "'", CommonDev.connstr);

                for (int i = 0; i < dgvBranchers.Rows.Count; i++)
                {
                    string BranchID = dgvBranchers.Rows[i].Cells[0].FormattedValue.ToString();
                    string ischecked = dgvBranchers.Rows[i].Cells["IsAccess"].FormattedValue.ToString();

                    if (ischecked == "True")
                    {
                        queryUPD1 = "INSERT INTO [dbo].[BranchAccess]([BranchID],[EmpNo],[IsActive],[DateCreated],[UserCreated]) " +
                                                                "VALUES ('" + BranchID + "','" + txtEmpNo.Text + "','TRUE','" + DateTime.Now.ToString() + "','" + CommonDev.loggedUser + "')";
                        CommonDev.ExecuteStatement(queryUPD1, CommonDev.connstr);
                    }

                }
                MessageBox.Show("User Details Updated Successfully.");
                ClearFields();
                getallusers();
                return;
            }
            else
            {
                DialogResult dresult = MessageBox.Show("Do you want to Save ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dresult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string queryINS, queryINS1 = string.Empty;
                queryINS = "INSERT INTO UserDetails(EPFNo,UserName,UPassword,FullName,IsActive,UserGroupID,Type,DateCreated,UserCreated) " +
                           "VALUES('" + txtEmpNo.Text.Trim() + "','" + txtUserName.Text.Trim() + "','" + hash + "','" + txtFullName.Text.Trim() + "','" + IsActive + "','" + cmbUserGroup.SelectedValue + "','" + type + "', " +
                           "'" + DateTime.Now + "','" + CommonDev.loggedUser + "')";
                CommonDev.ExecuteStatement(queryINS, CommonDev.connstr);

                for (int i = 0; i < dgvBranchers.Rows.Count; i++)
                {
                    string BranchID = dgvBranchers.Rows[i].Cells[0].FormattedValue.ToString();
                    string ischecked = dgvBranchers.Rows[i].Cells["IsAccess"].FormattedValue.ToString();

                    if (ischecked == "True")
                    {
                        queryINS1 = "INSERT INTO [dbo].[BranchAccess]([BranchID],[EmpNo],[IsActive],[DateCreated],[UserCreated]) " +
                                                                "VALUES ('" + BranchID + "','" + txtEmpNo.Text + "','TRUE','" + DateTime.Now.ToString() + "','" + CommonDev.loggedUser + "')";
                        CommonDev.ExecuteStatement(queryINS1, CommonDev.connstr);
                    }

                }

                MessageBox.Show("User Details Added Successfully.", "User_Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                getallusers();
                return;
            }
        }

        private void txtUN_Search_TextChanged(object sender, EventArgs e)
        {
            findallusers(null, txtUN_Search.Text);
        }

        private void LoadAllUsers_Search(string empno, string username, string branchname)
        {
            //string brcode = string.Empty;
            //if (branchname == "Kollupitiya")
            //{
            //    brcode = "1";
            //}
            //else if (branchname == "Negombo")
            //{
            //    brcode = "3";
            //}
            //else if (branchname == "Nugegoda")
            //{
            //    brcode = "4";
            //}
            //else if (branchname == "Kurunegala")
            //{
            //    brcode = "5";
            //}
            //else if (branchname == "KCC")
            //{
            //    brcode = "7";
            //}
            //else if (branchname == "Kandy")
            //{
            //    brcode = "2";
            //}

            //string query = string.Empty;
            //if (!string.IsNullOrEmpty(empno) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(brcode))
            //{
            //    query = "SELECT UserName, Level, EMPNO, Branch FROM Users WHERE EMPNO LIKE '%" + empno + "%' AND UserName LIKE '%" + username + "%' AND Branch LIKE '%" + brcode + "%' ORDER BY EMPNO";
            //}
            //else if (string.IsNullOrEmpty(empno))
            //{
            //    query = "SELECT UserName, Level, EMPNO, Branch FROM Users WHERE UserName LIKE '%" + username + "%' AND Branch LIKE '%" + brcode + "%' ORDER BY EMPNO";
            //}
            //else if (string.IsNullOrEmpty(username))
            //{
            //    query = "SELECT UserName, Level, EMPNO, Branch FROM Users WHERE EMPNO LIKE '%" + empno + "%' AND Branch LIKE '%" + brcode + "%' ORDER BY EMPNO";  
            //}
            //else if (string.IsNullOrEmpty(brcode))
            //{
            //    query = "SELECT UserName, Level, EMPNO, Branch FROM Users WHERE EMPNO LIKE '%" + empno + "%' AND UserName LIKE '%" + username + "%' ORDER BY EMPNO";
            //}
            //DataTable dt = new DataTable();
            //dt = CommonDev.SelectMultipleString(query, CommonDev.connstrDataCommon);
            //dgvUserDtls.Rows.Clear();
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string brname = string.Empty;
            //        if (dt.Rows[i]["Branch"].ToString() == "1")
            //        {
            //            brname = "Kollupitiya";
            //        }
            //        else if (dt.Rows[i]["Branch"].ToString() == "3")
            //        {
            //            brname = "Negombo";
            //        }
            //        else if (dt.Rows[i]["Branch"].ToString() == "4")
            //        {
            //            brname = "Nugegoda";
            //        }
            //        else if (dt.Rows[i]["Branch"].ToString() == "5")
            //        {
            //            brname = "Kurunegala";
            //        }
            //        else if (dt.Rows[i]["Branch"].ToString() == "7")
            //        {
            //            brname = "KCC";
            //        }
            //        else if (dt.Rows[i]["Branch"].ToString() == "2")
            //        {
            //            brname = "KCC";
            //        }

            //        dgvUserDtls.Rows.Add(dt.Rows[i]["UserName"].ToString(),
            //                             dt.Rows[i]["Level"].ToString(),
            //                             dt.Rows[i]["EMPNO"].ToString(),
            //                             brname);

            //        dgvUserDtls.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
            //        dgvUserDtls.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
            //        dgvUserDtls.Rows[i].Cells[2].Style.ForeColor = Color.Blue;
            //        dgvUserDtls.Rows[i].Cells[3].Style.ForeColor = Color.Blue;

            //        DataGridViewCellStyle style = new DataGridViewCellStyle();
            //        style.Font = new Font(dgvUserDtls.Font, FontStyle.Bold);
            //        dgvUserDtls.Rows[i].DefaultCellStyle = style;
            //    }
            //    lblUserCount.Text = dgvUserDtls.Rows.Count.ToString();
            //}
            //dgvUserDtls.ClearSelection();
        }

        private void dgvUserDtls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getallBranchers();

            checkBoxLtd.Checked = false;
            checkBoxLtdi.Checked = false;

            if (e.RowIndex >= 0)
            {
                var senderGrid = (DataGridView)sender;
                int selectedrowindex = dgvUserDtls.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvUserDtls.Rows[selectedrowindex];
                string UserName = Convert.ToString(selectedRow.Cells["UserName"].Value);

                string query = string.Empty;
                query = "SELECT  UserID, EPFNo, UserName, UPassword, FullName, IsActive, UserGroupID,Type, DateCreated, DateLastModified, UserCreated\r\nFROM UserDetails WHERE UserName IN('" + UserName + "')";
                DataTable dt = new DataTable();
                dt = CommonDev.SelectMultipleString(query, CommonDev.connstr);
                if (dt.Rows.Count > 0)
                {
                    txtUserName.Text = UserName;
                    txtPass.Text = dt.Rows[0]["UPassword"].ToString();
                    txtConfirmPass.Text = dt.Rows[0]["UPassword"].ToString();
                    txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                    txtEmpNo.Text = dt.Rows[0]["EPFNo"].ToString();
                    cmbUserGroup.SelectedValue = dt.Rows[0]["UserGroupID"];

                    if (dt.Rows[0]["Type"].ToString() == "LTD")
                        checkBoxLtd.Checked = true;
                    if (dt.Rows[0]["Type"].ToString() == "LTDi")
                        checkBoxLtdi.Checked = true;

                    if (dt.Rows[0]["Type"].ToString() == "LTD/LTDi")
                    {
                        checkBoxLtd.Checked = true;
                        checkBoxLtdi.Checked = true;
                    }
                    bool activation;

                    activation = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    if (activation == true)
                        CheckboxIsActive.Checked = true;
                    else
                        CheckboxIsActive.Checked = false;

                    getallaccesbranchres(txtEmpNo.Text);
                }
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {

        }

        private void cboxBranch_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();

            //LoadAllUsers_Search(txtEN_Search.Text.Trim(), txtUN_Search.Text.Trim(), cboxBranch_Search.Text.Trim());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dgvUserDtls.SelectedRows.Count == 0)
            //    {
            //        MessageBox.Show("Please select the User to De-Activate.", "User_Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    DialogResult dresult = MessageBox.Show("Do you want to De-Activate ?", "User_Permission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dresult == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        string query = string.Empty;
            //        query = "SELECT * FROM Users WHERE EMPNO IN('" + txtEmpNo.Text.Trim() + "') AND UserName IN('" + txtUserName.Text.Trim() + "')";
            //        DataTable dt = new DataTable();
            //        dt = CommonDev.SelectMultipleString(query, CommonDev.connstrDataCommon);
            //        if (dt.Rows.Count > 0)
            //        {
            //            //string queryUPD = string.Empty;
            //            //queryUPD = "DELETE FROM Users WHERE UserName IN('" + txtUserName.Text.Trim() + "')";
            //            //CommonDev.ExecuteStatement(queryUPD, CommonDev.connstrDataCommon);

            //            string queryUPD = string.Empty;
            //            queryUPD = "UPDATE Users SET UserName = 'xxx', PreviousName = '" + txtUserName.Text.Trim() + "' " +
            //                       "WHERE EMPNO IN('" + txtEmpNo.Text.Trim() + "') AND UserName IN('" + txtUserName.Text.Trim() + "')";
            //            CommonDev.ExecuteStatement(queryUPD, CommonDev.connstrDataCommon);

            //            MessageBox.Show(txtUserName.Text.Trim() + " User Details De-Activated Successfully.", "User_Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            ClearFields();
            //            //LoadAllUsers();
            //            //txtUN_Search.Text = string.Empty;
            //            //txtEN_Search.Text = string.Empty;
            //            //cboxBranch_Search.SelectedIndex = -1;
            //            LoadAllUsers_Search(txtEN_Search.Text.Trim(), txtUN_Search.Text.Trim(), cboxBranch_Search.Text.Trim());
            //            return;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "User_Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = this.chkShowPassword.Checked ? char.MinValue : '*';
            txtConfirmPass.PasswordChar = this.chkShowPassword.Checked ? char.MinValue : '*';
        }

        private void txtEN_Search_TextChanged(object sender, EventArgs e)
        {
            // ClearFields();

            // LoadAllUsers_Search(txtEN_Search.Text.Trim(), txtUN_Search.Text.Trim(), cboxBranch_Search.Text.Trim());
        }

        private void frmUserLoginCreate_FormClosed(object sender, FormClosedEventArgs e)
        {
            //try
            //{
            //    Application.Exit();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "User_Creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
        }

        private void txtEpfNo_Search_TextChanged(object sender, EventArgs e)
        {
            findallusers(txtEpfNo_Search.Text, null);
        }
    }
}
