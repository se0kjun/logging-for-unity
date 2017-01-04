using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;

public class DataSendManager : MonoBehaviour
{
    private ManualResetEvent connectDone = new ManualResetEvent(false);
    private ManualResetEvent sendDone = new ManualResetEvent(false);
    private Socket client;

    public string ipAddress;
    public int port;

    public void SendEvent()
    {
        if (client == null)
            // Create a TCP/IP socket.
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        if (!client.Connected)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            // Connect to the remote endpoint.
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();
        }

        // message
        // Send test data to the remote device.
        Send(client, "This is a test");
        sendDone.WaitOne();
        //Debug.Log(GetComponent<CommandManager>().sendData("testetststet"));
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket lclient = (Socket)ar.AsyncState;

            // Complete the connection.
            lclient.EndConnect(ar);

            Debug.Log("Socket connected to" + lclient.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void Send(Socket client, String data)
    {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket lclient = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = lclient.EndSend(ar);
            Debug.Log("Sent {0} bytes to server." + bytesSent);

            // Signal that all bytes have been sent.
            sendDone.Set();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

}
