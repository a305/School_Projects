using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAuto : MonoBehaviour
{
    public bool flashLightActivated = false;
    private Color oldColor;
    private FlashLightControl flashLight;
    private bool inBeam;

    // Use this for initialization
    [SerializeField]
    GameObject listener;
    void Start()
    {
        oldColor = GetComponent<SpriteRenderer>().color;

        if (flashLightActivated == true)
            GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

        flashLight = GameObject.FindWithTag("Light").transform.parent.GetComponent<FlashLightControl>();
        inBeam = false;
    }

    private void Update()
    {
        if (flashLightActivated == false || (inBeam == true && flashLight.GetOnStatus() == true))
            GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        else
            GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (listener != null)
                listener.GetComponent<triggerListener>().triggerAction();
        }

        if (other.tag == "Light")
            inBeam = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Light")
            inBeam = false;
    }
}
