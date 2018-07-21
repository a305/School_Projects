using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class ClientConnection : MonoBehaviour {

    // Private variables for TCP connection to server
    TcpClient clientSocket;
    NetworkStream serverStream;
    string userName;
    string password;
    string additionalInfo;
    string ip;
    string key;

    // Use this for initialization
    void Start () {
        additionalInfo = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int ConnectTCP()
    {
        // If client already connected return 1
        if (clientSocket != null && clientSocket.Connected == true)
            return 1;

        try
        {
            // Assign TCP variables (take in IP)
            clientSocket = new TcpClient();
            clientSocket.Connect(ip, 8888);
        }
        catch(Exception e)
        {
            Debug.Log("Exception thrown: " + e.StackTrace);
            return 0;
        }

        // Return 0 if error occured
        if (clientSocket.Connected == false)
            return 0;

        // Initialize serverStream with client's network stream
        serverStream = clientSocket.GetStream();

        // Return 1 for success;
        return 1;
    }

    public void DisconnectTCP()
    {
        // Remove TCP connection
        clientSocket.Close();
    }

    public int SendServerMessage(string message)
    {
        // If there is no connection return 0 for failure
        if (clientSocket.Connected == false)
            return 0;

        // Write to server
        byte[] outStream = Encoding.ASCII.GetBytes(message + "\r\n");

        // Encrypt message
        byte k = 3;
        for (int i = 0; i < outStream.Length; i++)
            outStream[i] = (byte)(outStream[i] + k);

        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();

        // Return 1 for success
        return 1;
    }

    public string ReceiveServerMessage()
    {
        // If there is no connection return error message
        if (clientSocket.Connected == false)
            return "error - no connection";

        // Read server's response
        byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
        serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
        string returndata = Encoding.ASCII.GetString(inStream);

        if (returndata == "")
            return "error - no data";

        // Return response
        if (returndata.Contains("\r\n"))
            return returndata.Substring(0, returndata.IndexOf("\r\n"));
        else
            return "error - doesn't contain \r\n on: " + returndata;
    }

    public void SetUserName(string name)
    {
        userName = name;
    }

    public string GetUserName()
    {
        return userName;
    }

    public void SetPassword(string p)
    {
        password = p;
    }

    public string GetPassword()
    {
        return password;
    }

    public void SetAdditionalInfo(string s)
    {
        additionalInfo = s;
    }

    public string GetAdditionalInfo()
    {
        return additionalInfo;
    }

    public void SetIP(string i)
    {
        ip = i;
    }

    public string GetIP()
    {
        return ip;
    }

    public void SetKey(string k)
    {
        key = k;
    }
}
