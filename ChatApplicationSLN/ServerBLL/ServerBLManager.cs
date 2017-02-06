using HelpMessageLib;
using ServerDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerBLL
{
    public delegate void ClientDisconnectedHandler(string userName1, string status1,
        string time1, string date1);
    public delegate void ClientConnectedHandler(string userName1, string status1,
        string time1, string date1);
    public delegate void ActivateS2DaRHandler();

    public class ServerBLManager
    {
        public event ClientDisconnectedHandler GetDisconnected;
        public event ClientConnectedHandler GetConnected;
        public event ActivateS2DaRHandler GetActivateS2DaR;        
        private MyMessage msg1 = new MyMessage();
        private NetworkStream n;
        private List<NetworkStream> netstreams = new List<NetworkStream>();
        private TcpClient client;
        private TcpListener listener;
        private BinaryFormatter f = new BinaryFormatter();
        private bool canContinue = true;
        private ServerDAManager daManager = new ServerDAManager();                                

        public void StartListening(IPAddress myIPAddress, int myPort)
        {
            listener = new TcpListener(myIPAddress, myPort);
            listener.Start();
        }

        public void WaitForClients()
        {
            //an infinite loop for accepting clients
            while (true)
            {
                try
                {
                    //this line, in general, will be on hold for the next client 
                    client = listener.AcceptTcpClient();
                }
                //when this exception happends,
                //this thread will get terminated(the thread that accepts clients)
                catch (Exception)
                {
                    break;
                }
                //a client has been accepted. get a networkstream for that client                  
                n = client.GetStream();
                //receiving messages from each client will be on an infinite loop,
                //which will be on a seperate thread,
                //so that the main thread remains free
                Thread t = new Thread(WaitForMessages);
                t.IsBackground = true;
                t.Start();
            }
        }

        void WaitForMessages()
        {
            TcpClient client1 = client;
            NetworkStream n1 = n;
            //an infinite loop for receiving messages
            while (true)
            {
                //this line, in general, will be on hold for the next message
                msg1 = (MyMessage)f.Deserialize(n1);
                //a message from a specific client has been received
                if (msg1.AType == MessageType.TypeDisconnected)
                {
                    //if a specific client is disconnecting, notify all connected clients,
                    //remove his specific networkstream from the collection,
                    //remove his record from current users list,
                    //update the history file and the history list,
                    //give the disconnecting client an instraction to disconnect from
                    //his side, close the networkstream and the client,
                    //change the 'canContinue' variable to true (in case the server is
                    //disconnecting all clients one by one),
                    //and return to terminate the thread
                    msg1.AType = MessageType.TypeClientDisconnected;
                    foreach (NetworkStream i in netstreams)
                    {
                        f.Serialize(i, msg1);
                    }
                    netstreams.Remove(n1);
                    msg1.AType = MessageType.TypeDisconnected;
                    string time = DateTime.Now.ToLongTimeString();
                    string date = DateTime.Now.ToShortDateString();
                    if (GetDisconnected != null)
                        GetDisconnected(msg1.AUserName, "Disconnected", time, date);
                    f.Serialize(n1, msg1);
                    n1.Close();
                    client1.Close();
                    daManager.UpdateDisconnected(msg1.AUserName);
                    canContinue = true;
                    return;
                }
                if (msg1.AType == MessageType.TypeConnected)
                {
                    //if msg1.AName is null, it means that the client is not registering
                    //at this point, and if his username is not in the db, it means that
                    //he needs to register (if he wants to join the chat)
                    if (msg1.AName == null && (!daManager.UserNameExists(msg1.AUserName)))
                    {
                        msg1.AType = MessageType.TypeNeedToRegister;
                        f.Serialize(n1, msg1);
                        Thread.Sleep(100);
                        n1.Close();
                        client1.Close();
                        return;
                    }
                    //if msg1.AName is null, it means that the client is not registering
                    //at this point, and if his username is in the db, it means that
                    //everything is ok (he can join the chat right now)
                    if (msg1.AName == null && daManager.UserNameExists(msg1.AUserName))
                    {                                                
                        daManager.UpdateConnected(msg1.AUserName,
                            DateTime.Now.ToShortDateString());                                                                            
                    }
                    //if msg1.AName is not null, it means that the client is registering
                    //at this point, and it also means that everything is ok (he can join
                    //the chat right now)
                    if (msg1.AName != null)
                    {                                                
                        daManager.InsertClient(msg1.AName, msg1.AUserName,
                            DateTime.Now.ToShortDateString());
                    }
                    //if a client is connecting, add a record to current users list,
                    //and update the history file and the history list, before sending                    
                    //the message to all connected clients
                    string time = DateTime.Now.ToLongTimeString();
                    string date = DateTime.Now.ToShortDateString();
                    if (GetConnected != null)
                        GetConnected(msg1.AUserName, "Connected", time, date);
                    //add the networkstream to the networkstream list, 'netstreams'
                    netstreams.Add(n1);
                }
                if (msg1.AType == MessageType.TypeNormal)
                {                                        
                    int senderID = daManager.GetClientIdByUserName(msg1.AUserName);                    
                    daManager.InsertMessage(msg1.AMessage, senderID.ToString(),
                        DateTime.Now.ToShortDateString(), "0", "Normal");
                }
                if (msg1.AType == MessageType.TypePrivate)
                {                                        
                    int senderID = daManager.GetClientIdByUserName(msg1.AUserName);
                    int recipientID = daManager.GetClientIdByUserName(msg1.APrivate);
                    daManager.InsertMessage(msg1.AMessage, senderID.ToString(),
                        DateTime.Now.ToShortDateString(), recipientID.ToString(),"Private");
                }
                if (msg1.AType==MessageType.TypePicture)
                {
                    int x = 1;
                    if (!(Directory.Exists("chatpictures")))
                    {
                        Directory.CreateDirectory("chatpictures");
                    }
                    while (File.Exists(@"chatpictures\picture"+x.ToString()+msg1.AMessage))
                    {
                        x++;
                    }
                    msg1.AImage.Save(@"chatpictures\picture" + x.ToString() + msg1.AMessage);                    
                    int senderID = daManager.GetClientIdByUserName(msg1.AUserName);
                    int recipientID = daManager.GetClientIdByUserName(msg1.APrivate);
                    daManager.InsertMessage("picture"+x.ToString()+ msg1.AMessage,
                        senderID.ToString(), DateTime.Now.ToShortDateString(),
                        recipientID.ToString(), "Picture");                        
                }
                foreach (NetworkStream i in netstreams)
                {
                    //send the message to all connected clients
                    f.Serialize(i, msg1);
                }
            }
        }

        public void DisconnectAllClients()
        {
            List<NetworkStream> tmpNetstreams = new List<NetworkStream>();
            tmpNetstreams.Clear();
            foreach (NetworkStream i in netstreams)
            {
                tmpNetstreams.Add(i);
            }
            //this foreach loop will disconnect all clients
            foreach (NetworkStream i in tmpNetstreams)
            {
                msg1.AType = MessageType.TypePleaseSendDisconnectedBackToMe;
                f.Serialize(i, msg1);
                //set the canContinue variable to false
                canContinue = false;
                while (!canContinue)
                {
                    Thread.Sleep(5);
                }
                //once a client has disconnected, the canContinue variable changes back
                //to true, and the foreach loop moves to disconnecting the next client                
            }
            //all clients got disconnected. now the line: listener.Stop() will make an
            //exception that will help to terminate the thread that waits for clients
            //on an infinite loop
            listener.Stop();
            //enable and disable buttons, for the current state
            if (GetActivateS2DaR != null)
                GetActivateS2DaR();
        }        

        public DataTable GetAllMessages()
        {
            return daManager.GetAllMessages();
        }

        public DataTable GetMessagesByWord(string word1)
        {
            return daManager.GetMessagesByWord(word1);
        }

        public DataTable GetMessagesByDate(string date1)
        {
            return daManager.GetMessagesByDate(date1);
        }

        public DataTable GetAllClients()
        {
            return daManager.GetAllClients();
        }

        public DataTable GetClientById(string id1)
        {
            return daManager.GetClientById(id1);
        }

        public DataTable GetClientByUserName(string userName1)
        {
            return daManager.GetClientByUserName(userName1);
        }

        public DataTable GetTableClientsForList()
        {
            return daManager.GetTableClientsForList();
        }

        public void DeleteClient(string id1)
        {
            daManager.DeleteClient(id1);
        }
    }
}
