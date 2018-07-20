using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCursor : MonoBehaviour
{

    [SerializeField]
    Texture2D textureWhite;

    [SerializeField]
    Texture2D textureRed;
    // Use this for initialization
    void Start()
    {
        Cursor.SetCursor(textureWhite, new Vector2(0, 0), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
