using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

/***************************************************************
* file: VRPlayerInput.cs
* author: Colin Trotter
* class: CS 470 – Game Development
*
* assignment: class project
* date last modified: 4/30/2017
*
* purpose: This class gets input from the VR player via a virtual joystick and throttle. The input is sent to PlayerShipController which controls the actual movement of the ship.
*
****************************************************************/

public class VRPlayerInput : MonoBehaviour {

    public VRJoystickGrabAttach lThrottle;
    public VRJoystickGrabAttach rThrottle;
    public VRJoystickGrabAttach joystick;

    public Text hud;

    PlayerShipController shipController;

    float throttlePercentage;
    float xAngle;
    float zAngle;

    // Use this for initialization
    void Start () {
        shipController = GetComponent<PlayerShipController>();
	}
	
	// Update is called once per frame
	void Update () {
        GetThrottle();
        GetJoystick();

        if (hud != null)
        {
            UpdateHUD();
        }
	}

    private float GetThrottlePercentage(VRJoystickGrabAttach throttle)
    {
        float throttlePercentage = throttle.angleX / 65;     //throttle.limits.max;

        //Set throttle to 0 if approximately 0.
        if (Mathf.Abs(throttlePercentage) < 0.1)
        {
            throttlePercentage = 0;
        }

        
        return throttlePercentage;
    }

    private void GetThrottle()
    {
        //No pod racing
        /*
        throttlePercentage = GetThrottlePercentage(throttle);

        //Make throttle from 0-1 instead of -1 to 1.
        throttlePercentage += 1;
        throttlePercentage /= 2;

        shipController.SetThrottle(throttlePercentage);
        */


        //Pod racing
        float lThrottlePercentage = GetThrottlePercentage(lThrottle);
        float rThrottlePercentage = GetThrottlePercentage(rThrottle);

        //Set throttle to average of both
        throttlePercentage = (lThrottlePercentage + rThrottlePercentage) / 2;
        //Make throttle from 0-1 instead of -1 to 1.
        throttlePercentage += 1;
        throttlePercentage /= 2;

        shipController.SetThrottle(throttlePercentage);

        float yawPercentage = (lThrottlePercentage - rThrottlePercentage);

        //Set yaw to 0 if approximately 0.
        if (Mathf.Abs(yawPercentage) < 0.1)
        {
            yawPercentage = 0;
        }

        shipController.SetYaw(yawPercentage);

    }

    private void GetJoystick()
    {
        xAngle = joystick.angleX;
        zAngle = joystick.angleZ;

        if (Mathf.Abs(xAngle) < 20f)
        {
            xAngle = 0;
        }
        if (Mathf.Abs(zAngle) < 20f)
        {
            zAngle = 0;
        }

        float pitchPercentage = xAngle / 65;    //max angle of joystick is 65
        float rollPercentage = zAngle / 65;

        //Set pitch to 0 if approximately 0.
        if (Mathf.Abs(pitchPercentage) < 0.1)
        {
            pitchPercentage = 0;
        }
        //Set yaw to 0 if approximately 0.
        if (Mathf.Abs(rollPercentage) < 0.1)
        {
            rollPercentage = 0;
        }

        shipController.SetPitch(pitchPercentage);
        shipController.SetRoll(rollPercentage);
    }

    private void UpdateHUD()
    {
        Debug.Log("hud update");
        hud.text = ("Throttle: " + throttlePercentage + "\nxAngle: " + xAngle + "\nzAngle: " + zAngle);
    }

}
