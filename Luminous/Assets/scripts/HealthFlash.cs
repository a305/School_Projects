using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthFlash : MonoBehaviour {

    Image image; // assign your logo here


    // Use this for initialization
    void Start ()
    {
        image = GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update ()
    {
       
    }

    public void flash()
    {
        StartCoroutine(flashInput(image));
    }

    public IEnumerator flashInput(Image input)
    {
        Color imageColor = image.color;
        imageColor.a = .3f;
        image.color = imageColor;
        yield return new WaitForSeconds(.2f);
        imageColor.a = 0;
        image.color = imageColor;
        
    }

}
