using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveControl : MonoBehaviour {

    public HierarchyTree ht;
    public MainController mc;

    // Use this for initialization
    void Start () {
        GetComponent<Button>().onClick.AddListener(MoveObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void MoveObject()
    {
        SceneNode selected = mc.GetSelectedObject();

        if (selected == null)
        {
            Debug.Log("Move unsuccessful, please select an object before attempting to move.");
            return;
        }

        ht.Move(ref selected);
    }
}
