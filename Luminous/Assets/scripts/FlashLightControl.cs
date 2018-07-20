using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightControl : MonoBehaviour {

    public Camera mainCamera;
    public Text timeDisplay;

    public const float MAX_BATTERY = 5 * 60;
    public float startingBattery = 2 * 60;
    private float battery;
    private GameObject flashLight;
    private bool powerOn;

    [SerializeField]
    Light light;

    private void Start()
    {
        light.enabled = false;
        battery = startingBattery;
        timeDisplay.text = string.Format("Flashlight time remaining: {0:0.00}", battery);
        flashLight = transform.Find("flashLight").gameObject;
        flashLight.tag = "Light";
        powerOn = false;
    }

    // Update is called once per frame
    void Update () {
        // Used https://youtu.be/m-J5sCRipA0?t=23m27s for some of this code.
        Vector3 lookPosition = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position).normalized;

        float rotZ = Mathf.Atan2(lookPosition.y, lookPosition.x) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (Input.GetButtonDown("Light") && battery > 0)
            EnableLight(!light.enabled);

        // If light is enabled, subtract from battery
        if (light.enabled)
        {
            battery = Mathf.Clamp(battery - Time.deltaTime, 0, MAX_BATTERY);

            // If battery runs out, disable light
            if (battery <= 0)
                EnableLight(false);
        }

        timeDisplay.text = string.Format("Flashlight time remaining: {0:0.00}", battery);
    }

    public void SetBattery(float increase)
    {
        battery = Mathf.Clamp(battery + increase, 0, MAX_BATTERY);
    }

    public float GetBattery()
    {
        return battery;
    }

    public bool GetOnStatus()
    {
        return powerOn;
    }

    private void EnableLight(bool enable)
    {
        if (enable == true)
        {
            light.enabled = true;
            //flashLight.tag = "Light";
            powerOn = true;
            
        }
        else
        {
            light.enabled = false;
            //flashLight.tag = "Untagged";
            powerOn = false;
        }
    }
}
