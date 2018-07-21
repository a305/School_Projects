using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameListUpdater : MonoBehaviour {

    public GameObject orgGameInstance;
    public GameObject orgMessage;
    public GameObject content;
    public GameObject chatWindow;
    public GameObject chatContent;
    public GameObject errorMessage;

    private ClientConnection connection;
    private Queue<string> messageBuffer;
    private Queue<string[]> instanceBuffer;
    private bool run;

	// Use this for initialization
	void Start () {
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
        messageBuffer = new Queue<string>();
        instanceBuffer = new Queue<string[]>();
        run = true;
    }
	
	// Update is called once per frame
	void Update () {
        // Continue to try to get connection object if null
        if (connection == null)
        {
            connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();

            if (connection != null)
            {
                // Reconnect to server and reauthenticate
                connection.ConnectTCP();
                connection.SendServerMessage("login " + connection.GetUserName() + " " + connection.GetPassword());
                string response = connection.ReceiveServerMessage();
                Debug.Log(response);
                connection.SendServerMessage("joinlobby " + connection.GetUserName());

                // Let server know player is now in lobby
                Debug.Log("Sent joinlobby");
                response = connection.ReceiveServerMessage();
                Debug.Log(response);

                string[] additionalInfo = connection.GetAdditionalInfo().Split(' ');

                // Check for and display errors from last scene
                if (additionalInfo[0] != "" && additionalInfo[0] == "error")
                {
                    if (additionalInfo[2] == "create")
                        errorMessage.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nCreate game failed.";
                    else if (additionalInfo[2] == "join")
                        errorMessage.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nFailed to join game.";

                    connection.SetAdditionalInfo("");
                    errorMessage.SetActive(true);
                }

                // Start listening thread
                Thread t = new Thread(ReceiveServer);
                t.Start();
            }
        }

        if (messageBuffer.Count > 0)
        {
            // Create new message object by dequeueing from the messageBuffer
            GameObject message = Instantiate(orgMessage, chatContent.transform);
            message.GetComponent<Text>().text = messageBuffer.Dequeue();

            Debug.Log(message.GetComponent<Text>().text);

            // Move all other messages up
            for (int i = 0; i < chatContent.transform.childCount; i++)
                chatContent.transform.GetChild(i).localPosition += new Vector3(0, 30, 0);

            message.transform.localPosition = new Vector3(5, 20, 0);

            // Extend size of content window to update scroll bar
            //chatContent.GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 20 + (30 * (chatContent.transform.childCount - 1)));
        }

        if (instanceBuffer.Count > 0)
        {
            string[] segments = instanceBuffer.Dequeue();
            Transform gameIns = content.transform.Find(segments[2]);

            // Update game instance if it already exists, else create new one
            if (gameIns != null && gameIns.gameObject.name == segments[2])
            {
                GameObject gameInstance = gameIns.gameObject;
                gameInstance.transform.Find("HostName").GetComponent<Text>().text = segments[3];
                gameInstance.transform.Find("GameName").GetComponent<Text>().text = segments[4];
                gameInstance.transform.Find("GameStatus").GetComponent<Text>().text = segments[5] + "/2";
            }
            else
            {
                // Create new game instance object from instance buffer's data
                GameObject gameInstance = Instantiate(orgGameInstance, content.transform); 

                Debug.Log("Created game instance");

                // Set game instance's values
                gameInstance.name = segments[2];
                gameInstance.transform.Find("HostName").GetComponent<Text>().text = segments[3];
                gameInstance.transform.Find("GameName").GetComponent<Text>().text = segments[4];
                gameInstance.transform.Find("GameStatus").GetComponent<Text>().text = segments[5] + "/2";
                gameInstance.transform.SetParent(content.transform);

                // Move all other game instances down
                for (int i = 0; i < content.transform.childCount; i++)
                    content.transform.GetChild(i).localPosition += new Vector3(0, -140, 0);

                gameInstance.transform.localPosition = new Vector3(10, -10, 0);

                // Extend size of content window to update scroll bar
                //content.GetComponent<RectTransform>().sizeDelta = new Vector2(795, 10 + (140 * (content.transform.childCount - 1)));
            }
        }
    }

    public void SetRun(bool r)
    {
        run = r;
    }

    private void ReceiveServer()
    {
        while (run == true)
        {
            // Get server's message
            string response = connection.ReceiveServerMessage();

            Debug.Log("From lobby: " + response);

            // Split server's message into segments in order to get parameters
            string[] segments = response.Split(' ');

            // Case if segment communicates operation on game list
            if (segments[0] == "gamelist")
            {
                if (segments[1] == "update")
                {
                    instanceBuffer.Enqueue(segments);
                    connection.SendServerMessage("1");
                }
                else if (segments[1] == "delete")
                {
                    // If specified game instance exists, delete it
                    if (content.transform.Find(segments[2]) != null)
                        Destroy(content.transform.Find(segments[2]).gameObject);
                    connection.SendServerMessage("1");
                }
                else
                    connection.SendServerMessage("0");
            }
            // Case if segment communicates operation on global chat
            else if (segments[0] == "globalchat")
            {
                // Concatenate entire message
                string message = "";
                for (int i = 2; i < segments.Length; i++)
                    message += segments[i];

                // Add message to messageBuffer queue and send server response
                messageBuffer.Enqueue(segments[1] + ": " + message);
                connection.SendServerMessage("Acked: " + message);
            }
            else if (segments[0] == "0" || segments[0] == "1")
            { }
            else
                // Exit if segment doesn't match these cases
                connection.SendServerMessage("0");
        }
    }
}
