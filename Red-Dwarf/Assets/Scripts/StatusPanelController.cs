using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanelController : MonoBehaviour {

    private PlayerEvents playerEvents;
    private PlayerShipController shipController;

    public GameObject primaryWeaponLight;
    public GameObject secondaryWeaponLight;

    void OnEnable()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
        shipController = GetComponentInParent<PlayerShipController>();
        initListeners(true);
    }

    void OnDisable()
    {
        initListeners(false);
    }

    private void initListeners(bool state)
    {
        if (state)
        {
            playerEvents.OnWeaponReady += OnWeaponReady;
        }
        else
        {
            playerEvents.OnWeaponReady -= OnWeaponReady;
        }
    }

    private void OnWeaponReady(GameObject weapon, bool ready)
    {
        if (weapon == shipController.mainWeapon)
        {
            primaryWeaponLight.SetActive(ready);
        }
        else if(weapon == shipController.secondaryWeapon)
        {
            secondaryWeaponLight.SetActive(ready);
        }
    }


}
