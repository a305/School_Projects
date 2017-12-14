using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateControl : MonoBehaviour {

    public GameObject[] objects;
    public Transform theWorld;
    public HierarchyTree ht;
    public MainController mc;
    public Text warning;
 
	// Use this for initialization
	void Start () {
        GetComponent<Dropdown>().onValueChanged.AddListener(CreateObject);

        GetComponent<Dropdown>().ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData("Create new object"));
        for (int i = 0; i < objects.Length; i++)
            options.Add(new Dropdown.OptionData(objects[i].name));

        GetComponent<Dropdown>().AddOptions(options);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateObject(int value)
    {
        if (value == 0)
            return;

        SceneNode parentObject = mc.GetSelectedObject();
        Transform parent;
        if (parentObject == null)
            parent = theWorld;
        else
            parent = parentObject.transform;

        GameObject newObject = Instantiate(objects[value - 1], parent);
        SceneNode newScene = newObject.GetComponent<SceneNode>();
        bool success = ht.Insert(ref newScene, mc.GetSelectedObject());

        if (success == false)
        {
            warning.text = "Insert unsuccessful, please select parent object before attempting to insert.";
            Destroy(newObject);
        }
        else
            warning.text = "";

        GetComponent<Dropdown>().value = 0;
    }
}
