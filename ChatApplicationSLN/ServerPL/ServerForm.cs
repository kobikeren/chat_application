using ServerBLL;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class ServerForm : Form
    {
        private ServerBLManager blManager;
        private HelpForm1 frm1;
        private HelpForm2 frm2;                                   

        public ServerForm()
        {            
            InitializeComponent();
            frm1 = new HelpForm1();
            frm2 = new HelpForm2();
            blManager = new ServerBLManager();
            blManager.GetDisconnected += S1_GetDisconnected;
            blManager.GetConnected += S1_GetConnected;
            blManager.GetActivateS2DaR += S1_GetActivateS2DaR;
            //if this is the first time using the application (or for other reason),
            //then the chat history file 'a.txt' will not exist
            if (File.Exists("a.txt"))
            {
                //create a reader, and copy the history information to the history listview
                StreamReader r = new StreamReader("a.txt");
                while (!(r.EndOfStream))
                {
                    string name = r.ReadLine();
                    string status = r.ReadLine();
                    string time = r.ReadLine();
                    string date = r.ReadLine();
                    AddRecordToHistoryList(name, status, time, date);
                }
                r.Close();
            }
            //enable and disable buttons, for the current state
            ActivateState1DisconnectedAndNotReady();                       
        }

        private void S1_GetActivateS2DaR()
        {
            ActivateState2DisconnectedAndReady();
        }

        private void S1_GetConnected(string userName1, string status1,
            string time1, string date1)
        {
            AddRecordToUsersList(userName1, time1, date1);
            AddRecordToHistoryFile(userName1, status1, time1, date1);
            AddRecordToHistoryList(userName1, status1, time1, date1);
        }

        private void S1_GetDisconnected(string userName1, string status1,
            string time1, string date1)
        {
            RemoveRecordFromUsersList(userName1);
            AddRecordToHistoryFile(userName1, status1, time1, date1);
            AddRecordToHistoryList(userName1, status1, time1, date1);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            //create a TcpListener, and start listening
            try
            {
                blManager.StartListening(IPAddress.Parse(lblIPA.Text),
                    int.Parse(lblPort.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed");
                return;
            }
            //accepting clients will be on an infinite loop, which will be on a
            //seperate thread, so that the main thread remains free
            Thread t = new Thread(blManager.WaitForClients);
            t.IsBackground = true;
            t.Start();
            //enable and disable buttons, for the current state
            ActivateState3Connected();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //all clients will get disconnected, the listener will stop listening, all
            //threads will get terminated, and the 'exit' button will get enabled
            //this function will run on a new thread so the main thread remains free
            Thread t = new Thread(blManager.DisconnectAllClients);
            t.IsBackground = true;
            t.Start();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (lblIPA.Text != "")
            {
                frm1.SetData(lblIPA.Text, int.Parse(lblPort.Text));
            }
            else
            {
                frm1.SetData("127.0.0.1", 5000);
            }
            DialogResult dr = frm1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            lblIPA.Text = frm1.MyIPAddress;
            lblPort.Text = frm1.MyPort.ToString();
            //enable and disable buttons, for the current state
            ActivateState2DisconnectedAndReady();
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
            btnSet.Enabled = true;
            btnOpen.Enabled = false;
            btnStop.Enabled = false;
            btnExit.Enabled = true;
        }

        private void ActivateState2DisconnectedAndReady()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ActivateState2DisconnectedAndReady));
                return;
            }
            btnSet.Enabled = true;
            btnOpen.Enabled = true;
            btnStop.Enabled = false;
            btnExit.Enabled = true;
        }

        private void ActivateState3Connected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ActivateState3Connected));
                return;
            }
            btnSet.Enabled = false;
            btnOpen.Enabled = false;
            btnStop.Enabled = true;
            btnExit.Enabled = false;
        }
        #endregion

        //a function for adding a record to the current users list,
        //and a second function for removing a record from the current users list
        #region UsersListFunctions
        private void AddRecordToUsersList(string name, string time, string date)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string, string>(AddRecordToUsersList), name,
                    time, date);
                return;
            }
            ListViewItem lvi = new ListViewItem();
            ListViewItem.ListViewSubItem lvsi1 = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem();
            lvi.Text = name;
            lvsi1.Text = time;
            lvsi2.Text = date;
            lvi.SubItems.Add(lvsi1);
            lvi.SubItems.Add(lvsi2);
            listView1.Items.Add(lvi);
        }

        private void RemoveRecordFromUsersList(string name)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(RemoveRecordFromUsersList), name);
                return;
            }
            foreach (ListViewItem i in listView1.Items)
            {
                if (i.Text == name)
                {
                    i.Remove();
                    break;
                }
            }
        }
        #endregion

        //a function for adding a record to the history file,
        //and a second function for adding a record to the history list
        #region HistoryListFunctions
        private void AddRecordToHistoryFile(string name, string status,
            string time, string date)
        {
            StreamWriter w = new StreamWriter("a.txt", true);
            w.WriteLine(name);
            w.WriteLine(status);
            w.WriteLine(time);
            w.WriteLine(date);
            w.Close();
        }

        private void AddRecordToHistoryList(string name, string status,
            string time, string date)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string, string, string>(AddRecordToHistoryList),
                    name, status, time, date);
                return;
            }
            ListViewItem lvi = new ListViewItem();
            ListViewItem.ListViewSubItem lvsi1 = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsi3 = new ListViewItem.ListViewSubItem();
            lvi.Text = name;
            lvsi1.Text = status;
            lvsi2.Text = time;
            lvsi3.Text = date;
            lvi.SubItems.Add(lvsi1);
            lvi.SubItems.Add(lvsi2);
            lvi.SubItems.Add(lvsi3);
            listView2.Items.Add(lvi);
        }
        #endregion

        private void btnDB_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(ShowDbForm);
            t.IsBackground = true;
            t.Start();
        }

        private void ShowDbForm()
        {
            try
            {
                frm2.ShowDialog();
            }
            catch (Exception)
            {
                //the user made a mistake, by asking to show a form which is shown
            }
        }
    }
}
