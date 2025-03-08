using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    public int listenPort = 5005;
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = true;
    private string receivedMessage = "";

    public Quaternion attitude;
    public Vector3 rotationRate;
    public Vector3 userAcceleration;

    void Start()
    {
        udpClient = new UdpClient(listenPort);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log($"UDP Receiver запущен на порту {listenPort}");
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);

        while (isRunning)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);
                string[] values = message.Split(';');
                if (values.Length == 10)
                {
                    attitude = new Quaternion(
                        float.Parse(values[0]), float.Parse(values[1]),
                        float.Parse(values[2]), float.Parse(values[3]));

                    rotationRate = new Vector3(
                        float.Parse(values[4]), float.Parse(values[5]), 
                        float.Parse(values[6]));

                    userAcceleration = new Vector3(
                        float.Parse(values[7]), float.Parse(values[8]), 
                        float.Parse(values[9]));
                }
                receivedMessage = $"Получено сообщение от {remoteEndPoint.Address}: {Encoding.UTF8.GetString(data)}";
            }
            catch (Exception e)
            {
                receivedMessage = "Ошибка при приёме данных: " + e.Message;
            }
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(receivedMessage))
        {
            Debug.Log(receivedMessage);
            receivedMessage = "";
        }
        //Debug.Log($"Гироскоп: X={gyroData.x} Y={gyroData.y} Z={gyroData.z}");
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        udpClient?.Close();
        receiveThread?.Abort();
    }
}
