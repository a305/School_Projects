using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class MainController : MonoBehaviour {

    public float startingTime;
    public GameObject timeoutDisplay;
    public GameObject errorMessage;
    public RaycastTest gameControl;

    private ClientConnection connection;
    private bool placing;
    private bool takeTurn;
    private float timer;
    private bool continueGame;
    private bool host;
    private string[] additionalInfo;
    private string roomNum;
    private bool showTimeout;

    private GameObject test;

	// Use this for initialization
	void Start () {
        // Create TCP connection
        takeTurn = false;
        placing = true;
        showTimeout = false;
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
        SetContinueGame(true);

        additionalInfo = connection.GetAdditionalInfo().Split();

        connection.ConnectTCP();
        connection.SendServerMessage("login " + connection.GetUserName() + " " + connection.GetPassword() + " game");
        connection.SendServerMessage("login " + connection.GetUserName() + " " + connection.GetPassword() + " game");
        string res = connection.ReceiveServerMessage();
        Debug.Log(res);

        if (connection.GetAdditionalInfo() == "host")
        {
            host = true;
   
            // Send create game message to server
            connection.SendServerMessage("creategame " + connection.GetUserName() + " ");
            roomNum = connection.ReceiveServerMessage();

            Debug.Log(roomNum);

            if (roomNum == "0")
            {
                connection.SetAdditionalInfo("error create");
                StartCoroutine(SendObject());
            }
            else
                connection.SetAdditionalInfo("");
        }
        else
        {
            host = false;
            roomNum = additionalInfo[1];

            connection.SendServerMessage("joingame " + connection.GetUserName() + " " + roomNum);
            string response = connection.ReceiveServerMessage();

            if (response == "0")
            {
                connection.SetAdditionalInfo("error join");
                StartCoroutine(SendObject());
            }
            else
                connection.SetAdditionalInfo("");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (timer > 0 && takeTurn)
            timer -= Time.deltaTime;

        // With half of remaining turn time left, display continuation prompt
        if (timer <= startingTime / 2 && takeTurn)
        {
            // Activate prompt if necessary
            showTimeout = true;

            // Update time readout
            timeoutDisplay.transform.Find("TimeIndicator").GetComponent<Text>().text = string.Format("Disconnecting in: {0:0.00}", timer);

            // If time expires, discontinue game
            if (timer <= 0)
            {
                connection.SendServerMessage("move " + connection.GetUserName() + " " + roomNum +  " hit lose");
                string response = connection.ReceiveServerMessage(); // Global score

                SetContinueGame(false);
            }
        }

        if (showTimeout == false && timeoutDisplay.activeInHierarchy == true)
            timeoutDisplay.SetActive(false);
        else if (showTimeout == true && timeoutDisplay.activeInHierarchy == false)
            timeoutDisplay.SetActive(true);
	}

    public void SetPlacing(bool set)
    {
        placing = set;

        // Check if host or not and if ships have been placed
        if (host == true && set == true)
        {
            // Send ready message to server
            connection.SendServerMessage("gameready " + connection.GetUserName() + " " + roomNum);
            //string response = connection.ReceiveServerMessage();

            Thread t = new Thread(ReceiveServer);
            t.Start();
        }
        else if (set == true)
        {
            // Send ready message to server
            connection.SendServerMessage("gameready " + connection.GetUserName() + " " + roomNum);
            //string response = connection.ReceiveServerMessage();

            Thread t = new Thread(ReceiveServer);
            t.Start();
        }
        else
            SetTurn(false);
    }

    public void SetTurn(bool turn)
    {
        takeTurn = turn;

        // Reset turn timer if player's turn, else (on completion of turn) deactivate tiemoutDisplay if necessary
        if (turn == true)
            SetContinueGame(true);
    }

    public bool GetTurn()
    {
        return takeTurn;
    }

    public void SetContinueGame(bool contin)
    {
        continueGame = contin;

        // If continue game is set to true, stop displaying timeout display and reset timer
        if (continueGame == true)
        {
            showTimeout = false;
            timer = startingTime;
        }
        else
        {
            // End game message
            connection.DisconnectTCP();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public string GetRoomNumber()
    {
        return roomNum;
    }

    private void ReceiveServer()
    {
        // Continuously read from server
        while (true)
        {
            string response = connection.ReceiveServerMessage();
            string[] segments = response.Split(' ');

            Debug.Log(response);

            // Call function in RaycastTest based on packet header
            if (segments[0] == "fire")
            {
                gameControl.SetFireBuffer(segments);
            }
            else if (segments[0] == "move")
            {
                connection.SendServerMessage("1");
                gameControl.SetMoveBuffer(segments);
            }
            else if (segments[0] == "makemove")
            {
                SetTurn(true);
                connection.SendServerMessage("1");
            }
        }
    }

    IEnumerator SendObject()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameList", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(connection.gameObject, SceneManager.GetSceneByName("GameList"));
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
