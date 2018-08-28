using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModeMenuSwither : MonoBehaviour
{

    public GameObject canvasVRObject;
    public GameObject canvasObject;
    public GameObject bg;
    public GameObject cameraRigVR;
    public GameObject cameraRig;
    public GameObject VRTK;


    public bool forceNonVR = false;

    // Use this for initialization
    void OnEnable()
    {
        if (UnityEngine.XR.XRDevice.isPresent && !forceNonVR)
        {
            Debug.Log("Starting in VR Mode");
            SetVRObjectsEnabled(true);
        }
        else
        {
            Debug.Log("Starting in Non-VR Mode");
            SetVRObjectsEnabled(false);
        }
    }

    void SetVRObjectsEnabled(bool state)
    {
        //VR components:
        UnityEngine.XR.XRSettings.enabled = state;
        cameraRigVR.SetActive(state);
        cameraRig.SetActive(!state);
        canvasVRObject.SetActive(state);
        canvasObject.SetActive(!state);
        bg.SetActive(!state);
        VRTK.SetActive(state);
        
    }

}
