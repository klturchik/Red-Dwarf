using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVRPlayerInput : MonoBehaviour {

    PlayerShipController shipController;
    private PauseMenuController pmc;

	// Use this for initialization
	void Start () {
        shipController = GetComponent<PlayerShipController>();
        pmc = GetComponentInChildren<PauseMenuController>();

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        float h, v, mouseX, mouseY;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //Make throttle from 0-1 instead of -1 to 1.
        v += 1;
        v /= 2;

        shipController.SetYaw(h);
        shipController.SetThrottle(v);
        shipController.SetPitch(mouseY / (shipController.pitchSpeed / 2f));
        shipController.SetRoll(-mouseX / (shipController.rollSpeed / 2f));

        if (Input.GetButtonDown("Jump"))
        {
            shipController.SwitchPrimaryWeapon();
        }
        if (Input.GetButton("Fire1"))
        {
            shipController.FireMainWeapon();
        }

        if (Input.GetButton("Fire2"))
        {
            shipController.FireSecondaryWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pmc.GamePause();
        }

    }
}
