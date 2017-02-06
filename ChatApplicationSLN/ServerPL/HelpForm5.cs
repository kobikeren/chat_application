using ServerBLL;
using System;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class HelpForm5 : Form
    {
        ServerBLManager blManager = new ServerBLManager();

        public HelpForm5()
        {
            InitializeComponent();            
        }

        private void HelpForm5_Activated(object sender, EventArgs e)
        {
            listBox1.DataSource = blManager.GetTableClientsForList();
            listBox1.DisplayMember = "displayMember1";
            listBox1.ValueMember = "clientID1";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int idToDelete = (int)listBox1.SelectedValue;
                blManager.DeleteClient(idToDelete.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Operation failed");
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
