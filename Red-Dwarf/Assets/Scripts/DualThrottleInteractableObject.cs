using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DualThrottleInteractableObject : VRTK_InteractableObject {

    public VRJoystickGrabAttach otherThrottle;

    private GameObject usingController;

    public override void StartUsing (GameObject currentUsingObject) {
        base.StartUsing(currentUsingObject);
        usingController = currentUsingObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsUsing())
        {
            Vector3 targetPos = usingController.transform.position;
            otherThrottle.RotateTowardsTarget(targetPos);
        }
	}
}
