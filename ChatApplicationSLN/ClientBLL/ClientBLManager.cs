using HelpMessageLib;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClientBLL
{
    public delegate void PictureHandler(MyMessage m1);
    public delegate void PrivateHandler(MyMessage m1);
    public delegate void ConnectedHandler(MyMessage m1);
    public delegate void NormalHandler(MyMessage m1);
    public delegate void DisconnectedHandler(MyMessage m1);
    public delegate void ActivateS2DaRHandler();

    public class ClientBLManager
    {
        public event PictureHandler GetPicture;
        public event PrivateHandler GetPrivate;
        public event ConnectedHandler GetConnected;
        public event NormalHandler GetNormal;
        public event DisconnectedHandler GetDisconnected;
        public event ActivateS2DaRHandler GetActivateS2DaR;
        private MyMessage msgGet;
        private MyMessage msgSend = new MyMessage();
        private BinaryFormatter f = new BinaryFormatter();
        private NetworkStream n;
        private TcpClient client;

        public ClientBLManager ()
    	{
            moveOn = false;
            needToRegister = false;
	    }
        
        private bool needToRegister;
        public bool NeedToRegister
        {
            get { return needToRegister; }
            set { needToRegister = value; }
        }

        private bool moveOn;
        public bool MoveOn
        {
            get { return moveOn; }
            set { moveOn = value; }
        }

        //SetSendMessage (overload 1 of 2), that takes string and color, sets msgSend.AName
        //to null (that indicates to the server that there will not be a registration)        
        public void SetSendMessage(string myUserName, Color myColor)
        {
            msgSend.AName = null;
            msgSend.AUserName = myUserName;
            msgSend.AColor = myColor;
        }

        //SetSendMessage (overload 2 of 2), that takes string, set msgSend.AName to a
        //string (that indicates to the server that there will be a registration)
        public void SetSendMessage(string myFullName)
        {
            msgSend.AName = myFullName;
        }

        public void ConnectToServer(string myIPAddress, int myPort)
        {
            client = new TcpClient();
            client.Connect(myIPAddress, myPort);
            n = client.GetStream();
            msgSend.AType = MessageType.TypeConnected;
            f.Serialize(n, msgSend);
        }

        public void Send(string msg1)
        {
            msgSend.AMessage = msg1;
            msgSend.AType = MessageType.TypeNormal;
            f.Serialize(n, msgSend);
        }

        public void SendPrivate(string msg1, string name1)
        {
            msgSend.AMessage = msg1;
            msgSend.AType = MessageType.TypePrivate;
            msgSend.APrivate = name1;
            f.Serialize(n, msgSend);
        }

        public void SendPicture(string name1, Image img1,string extension1)
        {
            msgSend.APrivate = name1;
            msgSend.AImage = img1;
            msgSend.AMessage = extension1;
            msgSend.AType = MessageType.TypePicture;
            f.Serialize(n, msgSend);
        }

        public void SendDisconnected()
        {
            msgSend.AType = MessageType.TypeDisconnected;
            f.Serialize(n, msgSend);
        }

        public void WaitForMessages()
        {
            //an infinite loop for receiving messages
            while (true)
            {
                //this line, in general, will be on hold for the next message
                msgGet = (MyMessage)f.Deserialize(n);
                //a message from the server has been received
                switch (msgGet.AType)
                {
                    case MessageType.TypeNeedToRegister:
                        n.Close();
                        client.Close();
                        needToRegister = true;
                        moveOn = true;
                        return;
                    //a message of type 'TypePleaseSendDisconnectedBackToMe', represent
                    //a request from the server, that the client application will act
                    //like the client (the user), clicked on the disconnect button
                    case MessageType.TypePleaseSendDisconnectedBackToMe:
                        //send to the server a disconnected message, the disconnection will
                        //be executed when the server will send a disconnected message back
                        msgSend.AType = MessageType.TypeDisconnected;
                        f.Serialize(n, msgSend);
                        break;
                    case MessageType.TypePicture:
                        if (msgGet.APrivate == msgSend.AUserName)
                        {
                            if (GetPicture != null)
                                GetPicture(msgGet);
                        }
                        break;
                    case MessageType.TypePrivate:
                        if (msgGet.AUserName == msgSend.AUserName ||
                            msgGet.APrivate == msgSend.AUserName)
                        {
                            if (GetPrivate != null)
                                GetPrivate(msgGet);
                        }
                        break;
                    case MessageType.TypeConnected:
                        needToRegister = false;
                        moveOn = true;
                        if (GetConnected != null)
                            GetConnected(msgGet);
                        break;
                    case MessageType.TypeNormal:
                        if (GetNormal != null)
                            GetNormal(msgGet);
                        break;
                    case MessageType.TypeDisconnected:
                        n.Close();
                        client.Close();
                        //enable and disable buttons, for the current state
                        if (GetActivateS2DaR != null)
                            GetActivateS2DaR();
                        return;
                    case MessageType.TypeClientDisconnected:
                        if (GetDisconnected != null)
                            GetDisconnected(msgGet);
                        break;
                }
            }
        }
    }
}
