using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MyServer : MonoBehaviour
{
    byte[] udata;
    UdpClient newsock;
    IPEndPoint sender;

    void Start() {
        udata = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        newsock = new UdpClient(ipep);

        Debug.Log("Waiting for a client...");

        sender = new IPEndPoint(IPAddress.Any, 0);

                udata = newsock.Receive(ref sender);

        Debug.Log("Message received from {0}:" + sender.ToString());
        Debug.Log(Encoding.ASCII.GetString(udata, 0, udata.Length));

        string welcome = "Welcome to my test server";
        udata = Encoding.ASCII.GetBytes(welcome);
        newsock.Send(udata, udata.Length, sender);

    }

}
