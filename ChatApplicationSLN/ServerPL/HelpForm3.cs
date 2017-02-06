using ServerBLL;
using System;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class HelpForm3 : Form
    {
        ServerBLManager blManager = new ServerBLManager();

        public HelpForm3()
        {
            InitializeComponent();
        }

        private void HelpForm3_Activated(object sender, EventArgs e)
        {
            txtWord.Focus();
        }

        public void SetData(string word1, string day1, string month1,
            string year1, object source1)
        {
            txtWord.Text = word1;
            txtDay.Text = day1;
            txtMonth.Text = month1;
            txtYear.Text = year1;
            dataGridView1.DataSource = source1;
        }

        private void btnShowRecords_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = blManager.GetAllMessages();
        }

        private void btnFindByWord_Click(object sender, EventArgs e)
        {
            if (txtWord.Text == "")
            {
                MessageBox.Show("please enter a word");
                txtWord.Focus();
                return;
            }
            dataGridView1.DataSource = blManager.GetMessagesByWord(txtWord.Text);
        }

        private void btnFindByDate_Click(object sender, EventArgs e)
        {
            DateTime dTime;
            try
            {
                int day = int.Parse(txtDay.Text);
                int month = int.Parse(txtMonth.Text);
                int year = int.Parse(txtYear.Text);
                dTime = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                MessageBox.Show("please enter a valid date");
                txtDay.Focus();
                txtDay.SelectAll();
                return;
            }
            dataGridView1.DataSource = blManager.GetMessagesByDate(dTime.ToShortDateString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }        
    }
}
