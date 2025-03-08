using UnityEngine;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class UDPSender : MonoBehaviour
{
    public string remoteIP = "192.168.3.12";
    public int remotePort = 5005;

    public TMP_InputField tMP_Input;
    public GameObject ButtonSend;

    UdpClient udpClient;

    public TMP_Text tMP_Text;

    void Start()
    {
        tMP_Input.text = remoteIP;
    }

    public void StartSending(){
        remoteIP = tMP_Input.text;
        Debug.Log(remoteIP);
        ButtonSend.SetActive(false);
        udpClient = new UdpClient();
        udpClient.Connect(remoteIP, remotePort);

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        if (!SystemInfo.supportsGyroscope) return;

        Quaternion gyroData = Input.gyro.attitude;
        Vector3 gyrorate = Input.gyro.rotationRate;
        Vector3 acc = Input.gyro.userAcceleration;

        string message = $"{gyroData.x};{gyroData.y};{gyroData.z};{gyroData.w};" +
                         $"{gyrorate.x};{gyrorate.y};{gyrorate.z};" +
                         $"{acc.x};{acc.y};{acc.z}";
                         
        tMP_Text.text = message;
        byte[] data = Encoding.UTF8.GetBytes(message);

        udpClient.Send(data, data.Length);
    }

    void OnApplicationQuit()
    {
        if (udpClient != null)
            udpClient.Close();
    }
}
