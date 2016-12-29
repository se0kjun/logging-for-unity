using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Collections;

// State object for reading client data asynchronously
public class StateObject
{
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 8142;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class CommandArgs : EventArgs
{
    public byte[] rawData;
    public string stringData;
    public bool state;
}

public class DataReceiveManager : MonoBehaviour
{
    public int port;

    public event EventHandler CommandReceiveHandler;

    private Socket serverSocket;
    private byte[] _recieveBuffer = new byte[8142];

    // Use this for initialization
    void Awake()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        serverSocket.Bind(ipep);
        serverSocket.Listen(10);
        serverSocket.BeginAccept(new AsyncCallback(acceptCallback), serverSocket);
    }

    private void acceptCallback(IAsyncResult ar)
    {
        // accept a client 
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;

        // start receiving data 
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(receiveCallback), state);
        // call acceptCallback repeatedly
        listener.BeginAccept(new AsyncCallback(acceptCallback), listener);
    }

    private void receiveCallback(IAsyncResult ar)
    {
        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;
        int recieved = handler.EndReceive(ar);

        if (recieved <= 0)
        {
            Debug.Log("not received");
            return;
        }

        string rawReceiveData = Encoding.UTF8.GetString(state.buffer, 0, recieved);
        try
        {
            if (CommandReceiveHandler != null)
            {
                CommandReceiveHandler(this, new CommandArgs()
                {
                    rawData = state.buffer,
                    stringData = rawReceiveData,
                    state = ar.IsCompleted
                });
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(receiveCallback), state);
    }

    public bool sendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        return serverSocket.SendAsync(socketAsyncData);
    }

    public bool sendData(string data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
        return serverSocket.SendAsync(socketAsyncData);
    }

    void OnApplicationQuit()
    {
        try
        {
            serverSocket.Close();
        }
        catch
        {
            Debug.Log("소켓과 쓰레드 종료때 오류가 발생");
        }
    }
}
