namespace VRTK
{
    using System.Collections;
    using UnityEngine;

    public class VRJoystickController : VRTK_InteractableObject
    {
        public PlayerShipController shipController;
        private bool curHapticFeedback = false;
        int usingControllerIndex;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            //Get index of using controller when start using.
            var actualController = VRTK_DeviceFinder.GetActualController(usingObject);
            usingControllerIndex = (int)actualController.GetComponent<SteamVR_TrackedObject>().index;
        }

        private void Update()
        {
            //Keep firing every frame if they keep using the joystick
            if (IsUsing())
            {
                if (shipController.FireMainWeapon())
                {
                    //if successfully fired, pulse the controller's haptic feedback
                    TriggerHapticPulse(0.2f, 1, true, usingControllerIndex);
                }
            }

            //Secondary Weapon firing
            if (IsGrabbed())
            {
                GameObject controller = GetGrabbingObject();
                VRTK_ControllerEvents events = controller.GetComponent<VRTK_ControllerEvents>();
                if (events.touchpadPressed)
                {
                    //If the touchpad is pressed while they are holding the joystick.
                    shipController.FireSecondaryWeapon();
                }
            }
        }

        public void TriggerHapticPulse(float length, float strength, bool force, int controllerIndex)
        {
            if (!curHapticFeedback || force)
            {
                StartCoroutine(LongVibration(length, strength, controllerIndex));
            }
        }

        private IEnumerator LongVibration(float length, float strength, int controllerIndex)
        {
            curHapticFeedback = true;
            for (float i = 0; i < length; i += Time.deltaTime)
            {
                SteamVR_Controller.Input(controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
            curHapticFeedback = false;
        }

    }
}