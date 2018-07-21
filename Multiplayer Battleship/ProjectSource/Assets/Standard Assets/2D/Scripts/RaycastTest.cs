using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaycastTest : MonoBehaviour {

    public GameObject marker; // Prefab grid marker
    public GameObject gameBoard; // Local Player's game board
    public MainController controller;
    public GameObject ships;
    public GameObject leaderBoard;
    public GameObject leaderObject;

    private GameObject selectedShip; // Currently selected ship
    private int selectedWidth; // Currently slected ship's width
    private int selectedHeight; // Currently selected ship's height
    private bool[][] markers;
    private Vector3[] orgPositions;
    private ClientConnection connection;
    private Queue<string> leaderBuffer;
    private Queue<string[]> fireBuffer;
    private Queue<string[]> moveBuffer;

    private int localScore;
    private int row;
    private int column;
    private GameObject square;

	// Use this for initialization
	void Start () {
        markers = new bool[10][];

        for (int i = 0; i < 10; i++)
        {
            markers[i] = new bool[10];

            for (int a = 0; a < 10; a++)
                markers[i][a] = false;
        }

        orgPositions = new Vector3[ships.transform.childCount];

        for (int i = 0; i < ships.transform.childCount; i++)
            orgPositions[i] = ships.transform.GetChild(i).position;

        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
        leaderBuffer = new Queue<string>();
        fireBuffer = new Queue<string[]>();
        moveBuffer = new Queue<string[]>();
    }

    // Update is called once per frame
    void Update()
    {

        // Triggers on left mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            // Create raycast at mouse position
            RaycastHit2D hit = Physics2D.Raycast(GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Ship")
                {
                    // Set selectedShip to ship clicked
                    selectedShip = hit.collider.gameObject;
                    selectedWidth = selectedShip.GetComponent<ShipSettings>().width;
                    selectedHeight = selectedShip.GetComponent<ShipSettings>().height;

                    selectedShip.GetComponent<ShipSettings>().SetPlaced(true);
                }
                else if (hit.collider.transform.parent.parent.tag == "OpponentBoard" && controller.GetTurn() == true)
                {
                    // Get hit grid square and update layer to ignore raycast
                    GameObject hitObject = hit.collider.gameObject;
                    hitObject.layer = 2;

                    // Create a marker on top of the selected grid square
                    square = PlaceSquare(hitObject.transform.position);

                    // Update main controller's turn indicator
                    controller.SetTurn(false);

                    // Send message to server for fire
                    Debug.Log("Sending fire");
                    connection.SendServerMessage("fire " + connection.GetUserName() + " " + controller.GetRoomNumber() + " " + hitObject.transform.parent.name + " " + hitObject.name);
                    Debug.Log("Sent fire");
                }
            }

            if (leaderBuffer.Count > 0)
            {
                // Create new leaderboard object
                GameObject leaderObj = Instantiate(leaderObject);
                leaderObj.transform.parent = leaderBoard.transform.Find("Scroll View").Find("Viewport").Find("Content");
                string[] segments = leaderBuffer.Dequeue().Split(' ');

                // Display a player score or current player's score
                if (connection.GetUserName() != segments[0])
                    leaderObj.GetComponent<Text>().text = segments[0] + ": " + segments[1];
                else
                    leaderObj.GetComponent<Text>().text = segments[0] + ": " + (int.Parse(segments[1]) + localScore);

                // Set placement
                leaderObj.transform.position = new Vector3(5, -5 - (55 * leaderBoard.transform.Find("Scroll View").Find("Viewport").Find("Content").childCount), 0);
                leaderBoard.transform.Find("Scroll View").Find("Viewport").Find("Content").GetComponent<RectTransform>().sizeDelta = new Vector2(800, -5 - (55 * leaderBoard.transform.Find("Scroll View").Find("Viewport").Find("Content").childCount));
            }

        }

        if (fireBuffer.Count > 0)
            SetFire(fireBuffer.Dequeue());

        if (moveBuffer.Count > 0)
            SetMove(moveBuffer.Dequeue());

        // Triggers on left mouse button being held down with a ship selected
        if (Input.GetMouseButton(0) && selectedShip != null)
        {
            // Get mouse position
            Vector3 mousePosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            // Find closest letter list of grid numbers (e.g., 'A', 'B', etc.)
            float closestDistance = Mathf.Infinity;
            string closestChild = "";
            int closestListNum = -1;
            for (int i = selectedHeight - 1; i < gameBoard.transform.childCount; i++)
            {
                if (ObstructedList(i) == false && (gameBoard.transform.GetChild(i).position - mousePosition).magnitude < closestDistance)
                {
                    closestDistance = (gameBoard.transform.GetChild(i).position - mousePosition).magnitude;
                    closestChild = gameBoard.transform.GetChild(i).gameObject.name;
                    closestListNum = i;
                }
            }

            // Store closest list once found
            GameObject closestList = gameBoard.transform.Find(closestChild).gameObject;

            // Find closest grid square in list
            closestDistance = Mathf.Infinity;
            int closestChildNum = -1;
            for (int i = 0; i < closestList.transform.childCount; i++)
            {
                if (11 - float.Parse(closestList.transform.GetChild(i).name) >= selectedWidth 
                    && Obstructed(closestListNum, int.Parse(closestList.transform.GetChild(i).name) - 1) == false
                    && (closestList.transform.GetChild(i).position - mousePosition).magnitude < closestDistance)
                {
                    closestDistance = (closestList.transform.GetChild(i).position - mousePosition).magnitude;
                    closestChild = closestList.transform.GetChild(i).gameObject.name;
                    closestChildNum = int.Parse(closestList.transform.GetChild(i).name) - 1;
                }
            }

            //Debug.Log(closestList.name + closestList.transform.Find(closestChild).gameObject.name);

            row = closestListNum;
            column = closestChildNum;

            // Update selectedShip's position to be the same as the closest grid square
            selectedShip.transform.position = closestList.transform.Find(closestChild).position;
        }
        // If left mouse button released and a selected ship exists, set selected ship to null.
        else if (selectedShip != null)
        {
            selectedShip = null;

            // Mark grid squares ship will occupy as occupied
            for (int i = 0; i < selectedHeight; i++)
            {
                for (int a = 0; a < selectedWidth; a++)
                    markers[row - i][column + a] = true;
            }
        }
    }

    public void OnRestClick()
    {
        // Reset ship positions
        for (int i = 0; i < ships.transform.childCount; i++)
        {
            ships.transform.GetChild(i).position = orgPositions[i];
            ships.transform.GetChild(i).GetComponent<ShipSettings>().SetPlaced(false);
        }

        // Reset grid squares
        for (int i = 0; i < 10; i++)
        {
            for (int a = 0; a < 10; a++)
                markers[i][a] = false;
        }
    }

    public void OnLeaderBoardClose()
    {
        // End game message
        connection.DisconnectTCP();
        SceneManager.LoadScene("MainMenu");
    }

    private void SetFire(string[] args)
    {
        GameObject grid = gameBoard.transform.Find(args[1]).Find(args[2]).gameObject;
        GameObject opHit = PlaceSquare(grid.transform.position);

        Debug.Log("SetFire");

        if (markers[gameBoard.transform.GetSiblingIndex()][int.Parse(args[2]) - 1] == true)
        {
            Debug.Log("Found hit");
            // Set grid color to hit
            //grid.GetComponent<Renderer>().material.SetColor("_SpecColor", new Color(1, 0, 0));
            opHit.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

            // Mark grid as hit
            markers[gameBoard.transform.GetSiblingIndex()][int.Parse(args[2]) - 1] = false;

            // Check if opposing player has won
            bool won = true;
            for (int i = 0; i < 10; i++)
            {
                for (int a = 0; a < 10; a++)
                {
                    if (markers[i][a] == true)
                    {
                        won = false;
                        break;
                    }
                }
            }

            if (won == false)
            {
                Debug.Log("move " + connection.GetUserName() + " hit continue");
                //connection.SendServerMessage("this should be read by the opp. thread");
                connection.SendServerMessage("move " + connection.GetUserName() + " " + controller.GetRoomNumber() + " hit continue");
                //controller.SetTurn(true);
            }
            else
            {
                connection.SendServerMessage("move " + connection.GetUserName() + " " + controller.GetRoomNumber() + " hit lose");
                string response = connection.ReceiveServerMessage(); // Global score

                // Display score on leaderboard
                leaderBoard.transform.Find("LocalScore").GetComponent<Text>().text = "Your new score: " + (localScore + int.Parse(response));

                for (int i = 0; i < int.Parse(response); i++)
                {
                    connection.SendServerMessage("getleaderboard " + i);
                    response = connection.ReceiveServerMessage();
                    leaderBuffer.Enqueue(response);
                }
                //connection.SendServerMessage("getleaderboard");
                //response = connection.ReceiveServerMessage(); // Number of leader board results
                //connection.SendServerMessage("1");

                //// For each leader board result read it in and display it to leaderboard
                //for (int i = 0; i < int.Parse(response); i++)
                //{
                //    response = connection.ReceiveServerMessage();
                //    connection.SendServerMessage("1");

                //    // Add leaderboard object to buffer for creation
                //    leaderBuffer.Enqueue(response);
                //}

                // Display leaderboard
                leaderBoard.transform.Find("EndGameMessage").GetComponent<Text>().text = "Better luck next time, you lost.";
                leaderBoard.SetActive(true);
                // On close of leaderboard(in seperate on click funciton) set controller's continue game to false
            }
        }
        else
        {
            connection.SendServerMessage("move " + connection.GetUserName() + " " + controller.GetRoomNumber() + " miss continue");
            opHit.GetComponent<SpriteRenderer>().color =  new Color(1, 1, 1);
            //controller.SetTurn(true);
        }
    }

    public void SetFireBuffer(string[] args)
    {
        fireBuffer.Enqueue(args);
        Debug.Log("Enqueued fire");
    }

    public void SetMoveBuffer(string[] args)
    {
        moveBuffer.Enqueue(args);
    }

    private void SetMove(string[] args)
    {
        Debug.Log("Received move");

        //if (args[2] == "continue")
        //    connection.SendMessage("1");

        if (args[1] == "hit")
        {
            //square.GetComponent<Renderer>().material.SetColor("_SpecColor", new Color(1, 0, 0));
            Debug.Log("Display hit");
            square.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            localScore++;
        }
        else
            //square.GetComponent<Renderer>().material.SetColor("_SpecColor", new Color(1, 1, 1));
            square.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        if (args[2] == "win")
        {
            // Send acknowledgment
            connection.SendServerMessage("localscore " + connection.GetUserName() + " " + localScore.ToString());
            string response = connection.ReceiveServerMessage(); // Global score

            // Display score on leaderboard
            leaderBoard.transform.Find("LocalScore").GetComponent<Text>().text = "Your new score: " + (localScore + int.Parse(response));

            connection.SendServerMessage("getleaderboard");
            response = connection.ReceiveServerMessage(); // Number of leader board results
            connection.SendServerMessage("1");

            // For each leader board result read it in and display it to leaderboard
            for (int i = 0; i < int.Parse(response); i++)
            {
                response = connection.ReceiveServerMessage();
                connection.SendServerMessage("1");

                // Add leaderboard object to buffer for creation
                leaderBuffer.Enqueue(response);
            }

            // Display leaderboard
            leaderBoard.transform.Find("EndGameMessage").GetComponent<Text>().text = "Congradulations, you won!";
            leaderBoard.SetActive(true);
            // On close of leaderboard (in seperate on click funciton) set controller's continue game to false
        }
    }

    private GameObject PlaceSquare(Vector3 gridPosition)
    {
        // Create marker on top of selected grid square
        GameObject hitMarker = Instantiate(marker);
        hitMarker.transform.position = gridPosition + new Vector3(.12f, .12f, 0);

        return hitMarker;
    }

    private bool Obstructed(int list, int slot)
    {
        // Check dimensions of ship for already occupied grid squares
        for (int i = 0; i < selectedHeight; i++)
        {
            for (int a = 0; a < selectedWidth; a++)
            {
                if (markers[list - i][slot + a] == true)
                    return true;
            }
        }

        return false;
    }

    private bool ObstructedList(int list)
    {
        bool obs = false;

        for (int i = 0; i < 10 - selectedWidth; i++)
        {
            obs = false;

            for (int a = i; a < selectedWidth + i; a++)
            {
                if (markers[list][a] == true)
                {
                    obs = true;
                    break;
                }
            }

            if (obs == false)
                return false;

        }

        return true;
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
