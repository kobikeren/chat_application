using System;
using System.Drawing;

namespace HelpMessageLib
{
    [Serializable]
    public class MyMessage
    {
        public string AUserName { get; set; }
        public string AMessage { get; set; }
        public Color AColor { get; set; }
        public MessageType AType { get; set; }
        public string APrivate { get; set; }
        public Image AImage { get; set; }
        public string AName { get; set; }
    }

    public enum MessageType
    {
        TypeConnected = 0,
        TypeNormal = 1,
        TypeDisconnected = 2,
        TypeClientDisconnected = 3,
        TypePrivate = 4,
        TypePicture = 5,
        TypePleaseSendDisconnectedBackToMe = 6,
        TypeNeedToRegister = 7
    }
}
