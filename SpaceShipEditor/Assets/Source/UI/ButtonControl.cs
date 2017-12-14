using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {

    public MainController mainController;
    public Text txt;

    SceneNode target;
    bool selected = false;
    string nameText;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(SetSelectedObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSelectedObject()
    {
        mainController.SetSelectedObject(ref target);
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (selected)
        {
            GetComponent<Image>().color = new Color(0, 192, 255, 0);
            selected = false;
        }
        else
        {
            GetComponent<Image>().color = new Color(0, 192, 255, .5f);
            selected = true;
        }
    }

    public void SetSpacing(int spaces)
    {
        string spaceText = "";
        for (int i = 0; i < spaces; i++)
            spaceText += "\t";

        txt.text = spaceText + nameText;
    }

    public void SetSceneNode(ref SceneNode sn, int num)
    {
        target = sn;
        sn.name = sn.name.Remove(sn.name.Length - 7);
        name = sn.name;
        nameText = sn.name;
        txt.text = sn.name;
    }

    public void SetSceneParent(ref SceneNode parent)
    {
        target.transform.SetParent(parent.transform, false);
    }

    public SceneNode GetSceneNode()
    {
        return target;
    }

    public void Delete()
    {
        Destroy(target.gameObject);
        Destroy(this.gameObject);
    }
}
