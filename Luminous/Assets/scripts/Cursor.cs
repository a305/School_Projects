using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {

    [SerializeField]
    Texture2D cursorTexture;
	// Use this for initialization
	void Start ()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(0,0), CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
