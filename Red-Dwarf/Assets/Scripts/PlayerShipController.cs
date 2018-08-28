using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/***************************************************************
* file: PlayerShipController.cs
* author: Colin Trotter
* class: CS 470 – Game Development
*
* assignment: class project
* date last modified: 5/12/2017
*
* purpose: This class controls the movement of the player's ship. All input to this class is handled through the VRPlayerInput and NonVRPlayerInput classes.
*
****************************************************************/


[RequireComponent(typeof(Rigidbody))]
public class PlayerShipController : MonoBehaviour {

    [Header("Movement Settings")]
    public float accelerationSpeed = 1;
    public float pitchSpeed = 0.1f;
    public float yawSpeed = 0.1f;
    public float rollSpeed = 0.1f;
    public float maxSpeed = 10;

    [Header("GameObject References")]
    public List<GameObject> mainWeaponsList;
    public GameObject secondaryWeapon;
    public GameObject weaponSelectUI;

    private Rigidbody rb;
    private float throttlePercentage;
    private float pitchPercentage;
    private float yawPercentage;
    private float rollPercentage;

    public GameObject mainWeapon;

    private void Awake()
    {
        mainWeapon = mainWeaponsList[0];
    }

    // method: SetThrottle
    // purpose: sets the percentage of throttle to be applied. Should be called from VRPlayerInput or NonVRPlayerInput
    public void SetThrottle(float percentage)
    {
        throttlePercentage = percentage;
    }

    // method: SetPitch
    // purpose: sets the percentage of pitch to be applied. Should be called from VRPlayerInput or NonVRPlayerInput
    public void SetPitch(float percentage)
    {
        pitchPercentage = percentage;
    }

    // method: SetYaw
    // purpose: sets the percentage of yaw to be applied. Should be called from VRPlayerInput or NonVRPlayerInput
    public void SetYaw(float percentage)
    {
        yawPercentage = percentage;
    }

    // method: SetRoll
    // purpose: sets the percentage of roll to be applied. Should be called from VRPlayerInput or NonVRPlayerInput
    public void SetRoll(float percentage)
    {
        rollPercentage = percentage;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveShip();
	}

    public void MoveShip()
    {
        //Throttle:
        rb.AddForce(transform.forward * throttlePercentage * accelerationSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        //Pitch and Yaw:
        rb.AddRelativeTorque(new Vector3(pitchPercentage * pitchSpeed, yawPercentage * yawSpeed, rollPercentage * rollSpeed));
    }

    public void BarrelRoll()
    {

    }

    public bool FireMainWeapon()
    {
        if (mainWeapon == null)
        {
            Debug.Log("No main weapon found");
            //Quit early if weapon not found
            return false;
        }

        BasePlayerWeapon weaponScript = mainWeapon.GetComponent<BasePlayerWeapon>();
        return weaponScript.Fire();
    }

    public bool FireSecondaryWeapon()
    {
        if (mainWeapon == null)
        {
            Debug.Log("No secondary weapon found");
            //Quit early if weapon not found
            return false;
        }

        BasePlayerWeapon weaponScript = secondaryWeapon.GetComponent<BasePlayerWeapon>();
        return weaponScript.Fire();
    }

    public void SwitchPrimaryWeapon()
    {
        if (mainWeapon == mainWeaponsList[0])
        {
            mainWeapon = mainWeaponsList[1];
            weaponSelectUI.GetComponent<Text>().text = "GATTLING";
        }
        else
        {
            mainWeapon = mainWeaponsList[0];
            weaponSelectUI.GetComponent<Text>().text = "LAZOR MKII";
        }
    }
}
