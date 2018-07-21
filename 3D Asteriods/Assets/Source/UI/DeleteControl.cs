using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteControl : MonoBehaviour {

    public HierarchyTree ht;
    public MainController mc;
    public TheWorld world;

    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DeleteObject);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void DeleteObject()
    {
        SceneNode selected = mc.GetSelectedObject();

        if (selected == null)
        {
            Debug.Log("Delete unsuccessful, please select an object before attempting to dete.");
            return;
        }
        world.ForceDeselect();
        ht.Delete(ref selected);
    }
}
