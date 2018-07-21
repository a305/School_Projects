using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInstanceController : MonoBehaviour {

    private GameListUpdater updater;
    private ClientConnection connection;

    // Use this for initialization
    void Start () {
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
        updater = transform.parent.parent.Find("GameList").GetComponent<GameListUpdater>();
    }
	
	// Update is called once per frame
	void Update () {
        // Continue to try to get connection object if null
        if (connection == null)
            connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }

    public void OnClick()
    {
        // If game is available, join game, else display error message
        if (transform.Find("GameStatus").GetComponent<Text>().text == "1/2")
        {
            updater.SetRun(false);
            connection.SetAdditionalInfo("nothost " + name);
 
            StartCoroutine(SendObject());
        }
        else
        {
            Debug.Log("An error has occured: Game is likely full.");
        }
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
