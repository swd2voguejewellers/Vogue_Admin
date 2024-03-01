using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using User_Creation;
using VogueJewellers;

namespace PlayerUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            hideSubMenu();
        }

        private void hideSubMenu()
        {
            panelMasterSubMenu.Visible = false;
            panelTranSubMenu.Visible = false;
            panelReportsSubMenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(panelMasterSubMenu);
        }

        #region MediaSubMenu
        private void button2_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            UserGroup frm = new UserGroup();
            frm.TopLevel = false;
            panelContent.Controls.Add(frm);
            frm.Show();
            hideSubMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            frmUserRoles frm = new frmUserRoles();
            frm.TopLevel = false;
            panelContent.Controls.Add(frm);
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmSubRoles frm = new frmSubRoles();
            frm.TopLevel = false;
            panelContent.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
            hideSubMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            frmUserLoginCreate frm = new frmUserLoginCreate();
            frm.TopLevel = false;
            frm.WindowState = FormWindowState.Maximized;
            panelContent.Controls.Add(frm);
            frm.Show();
            hideSubMenu();
        }
        #endregion

        private void btnPlaylist_Click(object sender, EventArgs e)
        {
            showSubMenu(panelTranSubMenu);
        }

        #region PlayListManagemetSubMenu
        private void button8_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }
        #endregion

        private void btnTools_Click(object sender, EventArgs e)
        {
            showSubMenu(panelReportsSubMenu);
        }
        #region ToolsSubMenu
        private void button13_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }
        #endregion

        private void btnEqualizer_Click(object sender, EventArgs e)
        {
            openChildForm(new frmUserRoles());
            //..
            //your codes
            //..
            hideSubMenu();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            //..
            //your codes
            //..
            hideSubMenu();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
           // childForm.FormBorderStyle = FormBorderStyle.None;
           // childForm.Dock = DockStyle.Fill;
            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            UserGroupUsers frm = new UserGroupUsers();
            frm.TopLevel = false;
            panelContent.Controls.Add(frm);
            frm.Show();
            hideSubMenu();
        }
    }
}
