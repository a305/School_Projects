using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateGameController : MonoBehaviour {

    public GameObject gameList;
    public GameObject errorMessage;
    public GameListUpdater updater;

    private Text gameName;
    private ClientConnection connection;

	// Use this for initialization
	void Start () {
        gameName = transform.Find("GameName").Find("Text").GetComponent<Text>();
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }
	
	// Update is called once per frame
	void Update () {
        // Continue to try to get connection object if null
        if (connection == null)
            connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }

    public void OnClick()
    {
        // If gameName is empty, display error message and return
        if (gameName.text == "" || gameName.text.Contains("\\"))
        {
            errorMessage.transform.Find("Text").GetComponent<Text>().text = "An error has occured:\nGame name invalid (cannot be nothing or contain a '\').";
            errorMessage.SetActive(true);
            return;
        }

        updater.SetRun(false);
        connection.SetAdditionalInfo("host");
        //string response = connection.RecieveServerMessage();
        //Debug.Log(response);

        //connection.DisconnectTCP();
        StartCoroutine(SendObject());
    }

    public void OnExitClick()
    {
        connection.SendServerMessage("exitlobby " + connection.GetUserName());
        string response = connection.ReceiveServerMessage();

        Application.Quit();
    }

    IEnumerator SendObject()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(connection.gameObject, SceneManager.GetSceneByName("Main"));
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
