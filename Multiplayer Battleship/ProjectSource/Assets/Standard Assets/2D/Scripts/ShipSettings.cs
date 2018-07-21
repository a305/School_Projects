using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSettings : MonoBehaviour {

    public int width;
    public int height;

    private bool placed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlaced(bool p)
    {
        placed = p;
    }

    public bool GetPlaced()
    {
        return placed;
    }
}
