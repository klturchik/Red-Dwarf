using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRModeSwitcher : MonoBehaviour {

    public GameObject player;

    public bool forceNonVR = false;
    public GameObject NonVRMode;
    public GameObject VRMode;
    private PauseMenuController pmc;
    public GameObject VRTK;

    // Use this for initialization
    void Awake () {
        pmc = player.GetComponentInChildren<PauseMenuController>();
		if (UnityEngine.XR.XRDevice.isPresent && !forceNonVR)
        {
            Debug.Log("Starting in VR Mode");
            SetVRObjectsEnabled(true);
            pmc.PauseMenuCanvas = VRMode;
            StartCoroutine(DelaySetController());
        }
        else
        {
            Debug.Log("Starting in Non-VR Mode");
            SetVRObjectsEnabled(false);
            pmc.PauseMenuCanvas = NonVRMode;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
	}

    void SetVRObjectsEnabled(bool state)
    {
        //VR components:
        UnityEngine.XR.XRSettings.enabled = state;
        player.transform.Find("PlayerShip").Find("[CameraRig]").gameObject.SetActive(state);
        player.GetComponent<VRPlayerInput>().enabled = state;
        VRTK.SetActive(state);
        //VRTK_DeviceFinder.GetControllerLeftHand().transform.parent.gameObject.SetActive(state);
        //VRTK_DeviceFinder.GetControllerRightHand().transform.parent.gameObject.SetActive(state);

        //Non-VR components:
        player.transform.Find("PlayerShip").Find("NonVRCamera").gameObject.SetActive(!state);
        player.GetComponent<NonVRPlayerInput>().enabled = !state;
    }

    private IEnumerator DelaySetController()
    {
        yield return new WaitForSeconds(1f);
        VRTK_DeviceFinder.GetControllerLeftHand().transform.parent.gameObject.SetActive(true);
        VRTK_DeviceFinder.GetControllerRightHand().transform.parent.gameObject.SetActive(true);
    }

}
