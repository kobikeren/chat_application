using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClientPL
{
    public partial class HelpForm1 : Form
    {
        public HelpForm1()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            txtName.Focus();
            txtName.SelectAll();
        }

        public void SetData(string ipaddress1, int port1, string name1, Color color1)
        {
            txtIPA.Text = ipaddress1;
            txtPort.Text = port1.ToString();
            txtName.Text = name1;
            richTextBox1.ForeColor = color1;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            richTextBox1.ForeColor = colorDialog1.Color;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtIPA.Text == "")
            {
                MessageBox.Show("please enter ip address");
                txtIPA.Focus();
                return;
            }
            int x = 0;
            if (!(int.TryParse(txtPort.Text, out x)))
            {
                MessageBox.Show("please enter port number");
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }
            if (txtName.Text == "" || txtName.Text == "please type user name")
            {
                MessageBox.Show("please enter your name");
                txtName.Focus();
                return;
            }
            //the information in the text boxes is valid. insert that information
            //to the properties
            myIPAddress = txtIPA.Text;
            myPort = int.Parse(txtPort.Text);
            myName = txtName.Text;
            myColor = richTextBox1.ForeColor;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        //ClsProperties are the four properties of this form: ip address, port, name, color
        #region ClsProperties
        private string myIPAddress;
        public string MyIPAddress
        {
            get { return myIPAddress; }
        }
        private int myPort;
        public int MyPort
        {
            get { return myPort; }
        }
        private string myName;
        public string MyName
        {
            get { return myName; }
        }
        private Color myColor;
        public Color MyColor
        {
            get { return myColor; }
        }
        #endregion
    }
}
