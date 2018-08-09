using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace XGameFramework
{
    public class XDebugger
    {
        public static bool EnableLog = true;

        private static Socket _udpSocket;
        private static string ipAddress = "192.168.1.100"; //本机的IP地址
        private static Socket UdpSocket
        {
            get
            {
                if (_udpSocket == null)
                {
                    _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                }
                return _udpSocket;
            }
        }

        public static void Log(object message)
        {
            if (EnableLog)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Debug.Log(message);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(message.ToString());
                    IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 7788);
                    UdpSocket.SendTo(data, sendToPoint);
                }
            }
        }

        public static void Log(object message, Object context)
        {
            if (EnableLog)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Debug.Log(message, context);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(message.ToString());
                    IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 7788);
                    UdpSocket.SendTo(data, sendToPoint);
                }
            }
        }

        public static void LogFormat(string format, params object[] args)
        {
            if (EnableLog)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Debug.LogFormat(format, args);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(string.Format(format, args));
                    IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 7788);
                    UdpSocket.SendTo(data, sendToPoint);
                }
            }
        }

        public static void LogError(object message)
        {
            if (EnableLog)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Debug.LogError(message);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(message.ToString());
                    IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 7788);
                    UdpSocket.SendTo(data, sendToPoint);
                }
            }
        }

        public static void LogError(object message, Object context)
        {
            if (EnableLog)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Debug.LogError(message, context);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(message.ToString());
                    IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 7788);
                    UdpSocket.SendTo(data, sendToPoint);
                }
            }
        }
    }
}
