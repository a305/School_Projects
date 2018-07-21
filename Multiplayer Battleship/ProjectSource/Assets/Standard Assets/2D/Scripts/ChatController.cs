using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    public GameObject errorMessage;

    private ClientConnection connection;
    private InputField inputObject;
    private Text message;

	// Use this for initialization
	void Start () {
        inputObject = transform.Find("InputField").GetComponent<InputField>();
        message = transform.Find("InputField").Find("Text").GetComponent<Text>();
        Debug.Log(message);
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }
	
	// Update is called once per frame
	void Update () {
        // Continue to try to get connection object if null
        if (connection == null)
            connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }

    public void OnEndInput()
    {
        // If message is not invalid send to server, else display error message
        if (message.text != "" && message.text.Contains("\\") == false)
        {
            connection.SendServerMessage("globalchat " + connection.GetUserName() + " " + message.text);
            message.text = "";
        }
        else
        {
            errorMessage.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nMessage invalid (cannot be empty or contain a '\').";
            errorMessage.SetActive(true);
        }
    }
}
