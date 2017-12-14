using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

    public HierarchyTree ht;
    public MoveControl mc;
    public CreateControl cc;

    SceneNode selectedObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSelectedObject(ref SceneNode newObject)
    {
        ht.UpdateSelected(ref selectedObject);
        selectedObject = newObject;
    }

    public SceneNode GetSelectedObject()
    {
        return selectedObject;
    }
}
