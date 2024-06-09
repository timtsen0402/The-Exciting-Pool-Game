using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class TCP_Sender : MonoBehaviour
{
    int TCP_Port = 5065;
    string TCP_IP = "127.0.0.1";
    Socket sock = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
    public GameObject[] balls;
    private void Start()
    {
        print("sock init from Unity");
        sock.Connect(TCP_IP, TCP_Port);

        byte[] msg = Encoding.UTF8.GetBytes("This is a test");
        sock.Send(msg ,0, msg.Length, SocketFlags.None);

        sock.Close();
    }


    



}
