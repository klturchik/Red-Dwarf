using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WeaponSwitchInteractableObject : VRTK_InteractableObject {

    private Animator anim;
    public PlayerShipController shipController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void StartUsing(GameObject usingObject)
    {
        anim.SetTrigger("Flip");
        shipController.SwitchPrimaryWeapon();
	}
	
}
