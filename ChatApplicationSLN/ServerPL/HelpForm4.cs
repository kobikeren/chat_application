using ServerBLL;
using System;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class HelpForm4 : Form
    {
        ServerBLManager blManager = new ServerBLManager();

        public HelpForm4()
        {
            InitializeComponent();
        }

        private void HelpForm4_Activated(object sender, EventArgs e)
        {
            txtID.Focus();
        }

        public void SetData(string id1, string name1, object source1)
        {
            txtID.Text = id1;
            txtName.Text = name1;
            dataGridView1.DataSource = source1;
        }

        private void btnShowRecords_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = blManager.GetAllClients();
        }

        private void btnFindByID_Click(object sender, EventArgs e)
        {
            int x;
            if (!int.TryParse(txtID.Text, out x))
            {
                MessageBox.Show("please enter an id");
                txtID.Focus();
                txtID.SelectAll();
                return;
            }
            dataGridView1.DataSource = blManager.GetClientById(txtID.Text);
        }

        private void btnFindByName_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("please enter a name");
                txtName.Focus();
                return;
            }
            dataGridView1.DataSource = blManager.GetClientByUserName(txtName.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }        
    }
}
