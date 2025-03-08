using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;

public class GyroDataSender : MonoBehaviour
{
    public string targetIP = "192.168.3.1";
    public int targetPort = 12345;
    private UdpClient udpClient;

    public TMP_Text oktext;
    public TMP_Text gyrotext;

    void Start()
    {
        Input.gyro.enabled = true;
        udpClient = new UdpClient();
    }

    void Update()
    {
        oktext.text = "no";
        Debug.Log("new iter");
        Quaternion gyroAttitude = Input.gyro.attitude;
        Vector3 euler = gyroAttitude.eulerAngles;
        string dataToSend = $"{euler.x},{euler.y},{euler.z}";
        gyrotext.text = dataToSend;
        byte[] data = Encoding.UTF8.GetBytes(dataToSend);
        udpClient.Send(data, data.Length, targetIP, targetPort);
        Debug.Log("DATA SENT: " + dataToSend);
        oktext.text = "ok";
    }

    void OnDestroy()
    {
        if (udpClient != null)
            udpClient.Close();
    }
}
