using MainMenu;
using PlayerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VogueJewellers
{
    public partial class frmLogin : Form
    {
        SqlConnection con = new SqlConnection(CommonDev.connstr);

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
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string hash = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, txtPass.Text.Trim());
            }

            if (txtUserName.Text != "" && txtPass.Text != "")
            {
                string query = string.Empty;
                query = "SELECT UserName,UPassword FROM UserDetails WHERE UserName IN('" + txtUserName.Text.Trim() + "')";
                DataTable dt = new DataTable();
                dt = CommonDev.SelectMultipleString(query, CommonDev.connstr);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid User");
                    return;
                }
                else
                {
                    if (hash == dt.Rows[0]["UPassword"].ToString())
                    {
                        CommonDev.loggedUser = txtUserName.Text;
                        frmMain frm = new frmMain();
                        frm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Password is Incorrect");
                        return;
                    }
                }
                txtPass.Text = "";
                txtUserName.Text = "";
            }
        }
    }
}
