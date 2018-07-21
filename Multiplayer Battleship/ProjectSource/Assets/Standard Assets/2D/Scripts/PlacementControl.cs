using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementControl : MonoBehaviour {

    public GameObject board; // Opponent's game board
    public GameObject ships; // Object containing ships
    public GameObject resetButton;
    public MainController controller;

    private ClientConnection connection;

	// Use this for initialization
	void Start () {
        connection = GameObject.FindWithTag("IPAddress").GetComponent<ClientConnection>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Helper method to enable change of an object and all its childrens' layer
    void ChangeLayerRecursively(GameObject obj, int layer)
    {
        // Change obj's layer
        obj.layer = layer;

        // Cycle through each of obj's children and recursively call this method on them
        for (int i = 0; i < obj.transform.childCount; i++)
            ChangeLayerRecursively(obj.transform.GetChild(i).gameObject, layer);
    }

    // Exits placement mode on button click
    public void OnClick()
    {
        // Don't allow game to proceed without all ships having been placed
        for (int i = 0; i < ships.transform.childCount; i++)
        {
            if (ships.transform.GetChild(i).GetComponent<ShipSettings>().GetPlaced() == false)
                return;
        }

        // Change board to default layer and ships to ignore raycast layer.
        ChangeLayerRecursively(board, 0);
        ChangeLayerRecursively(ships, 2);

        controller.SetPlacing(true);

        // Remove this button.
        Destroy(resetButton);
        Destroy(gameObject);
    }
}
