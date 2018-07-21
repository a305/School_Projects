using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {

    public GameObject connectionObject;
    public Text userName;
    public Text password;
    public Text ip;
    public GameObject errorDisplay;

    private ClientConnection client;

	// Use this for initialization
	void Start () {
        client = connectionObject.GetComponent<ClientConnection>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnRegisterClick()
    {
        // Confirm not null username and password, if not display error message and return
        if (userName.text == "" || userName.text.Contains("\\") || password.text == "" || password.text.Contains("\\"))
        {
            errorDisplay.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nInvalid username or password.";
            errorDisplay.SetActive(true);
            return;
        }

        // Establish TCP connection with IP
        client.SetIP(ip.text);
        int success = client.ConnectTCP();

        if (success == 0)
        {
            Debug.Log("Client unable to connect");
            return;
        }

        // Wait for key from server, decrypt it

        // Send username and password to server for registration (make sure to encrypt password)
        client.SendServerMessage("reg " + userName.text + " " + password.text);
        string response = client.ReceiveServerMessage();

        Debug.Log(response);

        // Confirm valid username (not taken), if no error move on to next scene
        if (response == "0")
        {
            // Display error message and return
            errorDisplay.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nUsername taken, please select a new one and try again.";
            errorDisplay.SetActive(true);
            return;
        }
        else
        {
            client.SetUserName(userName.text);
            client.SetPassword(password.text);

            client.DisconnectTCP();
            StartCoroutine(SendObject());
        }
    }

    public void OnLoginClick()
    {
        // Establish TCP connection with IP
        client.SetIP(ip.text);
        int success = client.ConnectTCP();

        if (success == 0)
        {
            Debug.Log("Client unable to connect");
            return;
        }

        // Wait for key from server, decrypt it
        //string key = client.RecieveServerMessage();

        // Send username and encrypted password to server to confirm login
        client.SendServerMessage("login " + userName.text + " " + password.text);
        string response = client.ReceiveServerMessage();

        Debug.Log(response);

        // If not valid display error message to screen and return, else move on to next screen
        if (response == "0")
        {
            // Display error message and return
            errorDisplay.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nInvalid username or password.";
            errorDisplay.SetActive(true);
            return;
        }
        else
        {
            client.SetUserName(userName.text);
            client.SetPassword(password.text);

            client.DisconnectTCP();
            StartCoroutine(SendObject());
        }
    }

    IEnumerator SendObject()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameList", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(connectionObject, SceneManager.GetSceneByName("GameList"));
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
