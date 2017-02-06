using ClientBLL;
using HelpMessageLib;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ClientPL
{
    public partial class MainForm : Form
    {
        private HelpForm1 frm1;
        private HelpForm2 frm2;
        private HelpForm3 frm3;
        private ClientBLManager blManager;

        public MainForm()
        {            
            InitializeComponent();
            blManager = new ClientBLManager();
            blManager.GetActivateS2DaR += C1_GetActivateS2DaR;
            blManager.GetConnected += C1_GetConnected;
            blManager.GetDisconnected += C1_GetDisconnected;
            blManager.GetNormal += C1_GetNormal;
            blManager.GetPicture += C1_GetPicture;
            blManager.GetPrivate += C1_GetPrivate;
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            frm1 = new HelpForm1();
            frm2 = new HelpForm2();
            frm3 = new HelpForm3();
            //enable and disable buttons, for the current state
            ActivateState1DisconnectedAndNotReady();
        }

        private void C1_GetPrivate(MyMessage m1)
        {
            DisplayPrivateMessage(m1);
        }

        private void C1_GetPicture(MyMessage m1)
        {
            DisplayPictureMessage(m1);
        }

        private void C1_GetNormal(MyMessage m1)
        {
            DisplayNormalMessage(m1);
        }

        private void C1_GetDisconnected(MyMessage m1)
        {
            DisplayDisconnectedMessage(m1);
        }

        private void C1_GetConnected(MyMessage m1)
        {
            DisplayConnectedMessage(m1);
        }

        private void C1_GetActivateS2DaR()
        {
            ActivateState2DisconnectedAndReady();
        }

        private void btnShowHelpForm_Click(object sender, EventArgs e)
        {
            if (lblIPA.Text != "")
            {
                frm1.SetData(lblIPA.Text, int.Parse(lblPort.Text),
                    lblName.Text, richTextBox2.ForeColor);
            }
            else
            {
                frm1.SetData("127.0.0.1", 5000, "please type user name", Color.Black);
            }
            DialogResult dr = frm1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            //the user clicked on the ok button (on the sub form), now update the main form
            lblIPA.Text = frm1.MyIPAddress;
            lblPort.Text = frm1.MyPort.ToString();
            lblName.Text = frm1.MyName;
            richTextBox2.ForeColor = frm1.MyColor;
            //enable and disable buttons, for the current state
            ActivateState2DisconnectedAndReady();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //prepare the message, connect to the server and send a 'connected' message
            //SetSendMessage (overload 1 of 2), that takes string and color, set
            //msgSend.AName to null (that indicates to the server that there will
            //not be a registration)
            blManager.SetSendMessage(lblName.Text, richTextBox2.ForeColor);
            try
            {
                blManager.ConnectToServer(lblIPA.Text, int.Parse(lblPort.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed");
                return;
            }
            blManager.MoveOn = false;
            //receiving messages from the server will be on an infinite loop, which will
            //be on a seperate thread, so that the main thread remains free
            Thread t = new Thread(blManager.WaitForMessages);
            t.IsBackground = true;
            t.Start();
            Thread.Sleep(20);
            while (!blManager.MoveOn)
            {
                Thread.Sleep(5);
            }
            if (blManager.NeedToRegister)
            {
                frm3.SetData("", "", lblName.Text);
                DialogResult dr = frm3.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                //SetSendMessage (overload 2 of 2), that takes string, set msgSend.AName to
                //a string (that indicates to the server that there will be a registration)
                blManager.SetSendMessage(frm3.FullName);
                try
                {
                    blManager.ConnectToServer(lblIPA.Text, int.Parse(lblPort.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Connection failed");
                    return;
                }
                Thread t1 = new Thread(blManager.WaitForMessages);
                t1.IsBackground = true;
                t1.Start();
            }
            //enable and disable buttons, for the current state
            ActivateState3Connected();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //send to the server a disconnected message, the disconnection will be
            //executed when the server will send a disconnected message back
            blManager.SendDisconnected();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //send to the server a normal message
            blManager.Send(txtMessage.Text);
            txtMessage.Clear();
        }

        private void btnSendPrivate_Click(object sender, EventArgs e)
        {
            //send to the server a private message
            frm2.SetData("");
            DialogResult dr = frm2.ShowDialog();
            if (dr != DialogResult.OK)
            {
                MessageBox.Show("the private send was cancelled");
                return;
            }
            //the user clicked on the ok button (on the sub form)
            blManager.SendPrivate(txtMessage.Text, frm2.NamePrivate);
            txtMessage.Clear();
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            string extension1 = "";
            //send to the server a picture
            frm2.SetData("");
            DialogResult dr = frm2.ShowDialog();
            if (dr != DialogResult.OK)
            {
                MessageBox.Show("the picture send was cancelled");
                return;
            }
            //the user clicked on the ok button (on the sub form)
            DialogResult dr2 = openFileDialog1.ShowDialog();
            if (dr2 != DialogResult.OK)
            {
                MessageBox.Show("the picture send was cancelled");
                return;
            }
            //the user clicked on the ok button (on the open file dialog)
            //send the picture the user selected
            FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
            extension1 = Path.GetExtension(openFileDialog1.FileName);
            blManager.SendPicture(frm2.NamePrivate, Image.FromStream(fs), extension1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //the ActivateState functions will enable and disable buttons
        #region ActivateStateFunctions
        private void ActivateState1DisconnectedAndNotReady()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ActivateState1DisconnectedAndNotReady));
                return;
            }
            btnShowHelpForm.Enabled = true;
            btnConnect.Enabled = false;
            btnStop.Enabled = false;
            btnExit.Enabled = true;
            btnSend.Enabled = false;
            btnSendPrivate.Enabled = false;
            btnPicture.Enabled = false;
        }

        private void ActivateState2DisconnectedAndReady()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ActivateState2DisconnectedAndReady));
                return;
            }
            btnShowHelpForm.Enabled = true;
            btnConnect.Enabled = true;
            btnStop.Enabled = false;
            btnExit.Enabled = true;
            btnSend.Enabled = false;
            btnSendPrivate.Enabled = false;
            btnPicture.Enabled = false;
        }

        private void ActivateState3Connected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ActivateState3Connected));
                return;
            }
            btnShowHelpForm.Enabled = false;
            btnConnect.Enabled = false;
            btnStop.Enabled = true;
            btnExit.Enabled = false;
            btnSend.Enabled = true;
            btnSendPrivate.Enabled = true;
            btnPicture.Enabled = true;
        }
        #endregion

        //the DisplayMessage functions will display messages on the form
        #region DisplayMessageFunctions
        private void DisplayPictureMessage(MyMessage msg1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MyMessage>(DisplayPictureMessage), msg1);
                return;
            }
            pictureBox1.Image = msg1.AImage;
        }

        private void DisplayPrivateMessage(MyMessage msg1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MyMessage>(DisplayPrivateMessage), msg1);
                return;
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = msg1.AColor;
            richTextBox1.AppendText("(private to " + msg1.APrivate + ")" + "\n"
                + msg1.AUserName + " said: " + msg1.AMessage + "\n");
        }

        private void DisplayConnectedMessage(MyMessage msg1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MyMessage>(DisplayConnectedMessage), msg1);
                return;
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = msg1.AColor;
            richTextBox1.AppendText("SYSTEM: " + msg1.AUserName + " Connected" + "\n");
        }

        private void DisplayDisconnectedMessage(MyMessage msg1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MyMessage>(DisplayDisconnectedMessage), msg1);
                return;
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = msg1.AColor;
            richTextBox1.AppendText("SYSTEM: " + msg1.AUserName + " Disconnected" + "\n");
        }

        private void DisplayNormalMessage(MyMessage msg1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MyMessage>(DisplayNormalMessage), msg1);
                return;
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = msg1.AColor;
            richTextBox1.AppendText(msg1.AUserName + " said: " + msg1.AMessage + "\n");
        }
        #endregion
    }
}
