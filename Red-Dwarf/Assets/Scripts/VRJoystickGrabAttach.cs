using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{
    using GrabAttachMechanics;
    using UnityEngine;

    public class VRJoystickGrabAttach : VRTK_BaseJointGrabAttach
    {

        [Header("Joystick Settings")]
        public float angleX;
        public float angleZ;
        public bool snapToOrigin = false;
        public bool enableHapticFeedback = true;

        private float heightFromBase;
        private int grabbingControllerIndex;
        private VRJoystickController joystickController;

        public enum Axis
        {
            none,
            xAxis,
            zAxis
        };

        public Axis lockedAxis = Axis.none;

        private GameObject joystickBase;
        private VRTK_InteractableObject eventScript;

        //Required to fool VRTK_InteractableObject into thinking a Unity joint was created
        protected override void CreateJoint(GameObject obj)
        {
            //This space intentionally left blank
        }

        // Use this for initialization
        void Start()
        {
            joystickBase = transform.parent.gameObject;
            heightFromBase = transform.position.y - joystickBase.transform.position.y;  //this line will cause issues if the ship is not perfectly flat at Start()
            joystickController = GetComponent<VRJoystickController>();
        }

        private void Update()
        {
            //Debug.DrawLine(joystickBase.transform.position, joystickBase.transform.position + xVector);
            //Debug.DrawLine(joystickBase.transform.position, joystickBase.transform.position + zVector);
            //Debug.DrawLine(joystickBase.transform.position, targetPos);
        }

        private void OnGrabbed(object sender, InteractableObjectEventArgs e)
        {
            var controller = ((VRTK_InteractableObject)sender).GetGrabbingObject();
            var actualController = VRTK_DeviceFinder.GetActualController(controller);

            grabbingControllerIndex = (int)actualController.GetComponent<SteamVR_TrackedObject>().index;
            StartCoroutine(JointToController(actualController));
        }

        private void OnReleased(object sender, InteractableObjectEventArgs e)
        {
            if (snapToOrigin)
            {
                StartCoroutine(SnapToCenter());
                angleX = 0;
                angleZ = 0;
            }

        }

        private IEnumerator SnapToCenter()
        {
            //Efficiency could be improved. Keeps running after it hits center.
            while (!eventScript.IsGrabbed())
            {
                RotateTowardsTarget(joystickBase.transform.position + joystickBase.transform.up);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator JointToController(GameObject grabbingController)
        {
            //Every frame that the joystick is being grabbed...
            while (eventScript.IsGrabbed())
            {
                Vector3 targetPos = grabbingController.transform.position;
                RotateTowardsTarget(targetPos);

                if (enableHapticFeedback)
                {
                    //strength of haptic feedback is relative to distance between joystick and center
                    float hapticStrength = Mathf.Abs((angleX / 90) + (angleZ / 90)) / 10;

                    if (joystickController != null)
                    {
                        joystickController.TriggerHapticPulse(0.1f, hapticStrength, false, grabbingControllerIndex);
                    }
                    else
                    {
                        SteamVR_Controller.Input(grabbingControllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, hapticStrength));
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void RotateTowardsTarget(Vector3 targetPos)
        {
            //if (targetPos.y < joystickBase.transform.position.y){
            //    targetPos.y = joystickBase.transform.position.y;
            //}
            targetPos = ClampAngleHeight(targetPos);

            Vector3 relativePos = targetPos - transform.position;

            //Project the relative position along a plane to lock other axis
            if (lockedAxis == Axis.zAxis)
            {
                relativePos = Vector3.ProjectOnPlane(relativePos, joystickBase.transform.right);
                relativePos += Vector3.ProjectOnPlane(transform.up, joystickBase.transform.right);
            }
            else if (lockedAxis == Axis.xAxis)
            {
                relativePos = Vector3.ProjectOnPlane(relativePos, joystickBase.transform.forward);
                relativePos += Vector3.ProjectOnPlane(transform.up, joystickBase.transform.forward);
            }
            else
            {
                relativePos += transform.up;
            }


            //Make the up face of the joystick face the direction of the relative Pos (tilt the joystick to face the controller)
            transform.up = relativePos;

            //Scootch the stick upwards so it pivots around the joystick base
            transform.position = joystickBase.transform.position + transform.up * heightFromBase;

            //Get projection of vector along each required axis
            Vector3 xVector = Vector3.ProjectOnPlane(transform.up, joystickBase.transform.right);
            Vector3 yVector = Vector3.ProjectOnPlane(transform.forward, joystickBase.transform.up);
            Vector3 zVector = Vector3.ProjectOnPlane(transform.up, joystickBase.transform.forward);


            //Get the angle the joystick makes with each axis.
            angleX = AngleSigned(joystickBase.transform.up, xVector, joystickBase.transform.right);
            float angleY = AngleSigned(joystickBase.transform.forward, yVector, joystickBase.transform.up);
            angleZ = AngleSigned(joystickBase.transform.up, zVector, joystickBase.transform.forward);

            transform.Rotate(0, -angleY, 0);    //correct for weird rotation around y axis

        }

        //Make sure the targetPos isn't too low to give an invalid angle.
        private Vector3 ClampAngleHeight(Vector3 targetPos)
        {
            Vector3 localPoint = joystickBase.transform.InverseTransformPoint(targetPos);
            if (localPoint.y <= 0.2f)
            {
                localPoint.y = 0.2f;
            }
            return joystickBase.transform.TransformPoint(localPoint);
        }

        //Returns a signed angle between the two given vectors and the given normal
        public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }


        protected virtual void OnEnable()
        {
            eventScript = GetComponent<VRTK_InteractableObject>();
            InitListeners(true);
        }

        protected virtual void OnDisable()
        {
            InitListeners(false);
        }

        private void InitListeners(bool state)
        {
            InitGrabListeners(this.gameObject, state);
        }

        private void InitGrabListeners(GameObject controller, bool state)
        {

            if (controller)
            {

                if (eventScript)
                {
                    if (state)
                    {
                        eventScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(OnGrabbed);
                        eventScript.InteractableObjectUngrabbed += new InteractableObjectEventHandler(OnReleased);
                    }
                    else
                    {
                        eventScript.InteractableObjectGrabbed -= new InteractableObjectEventHandler(OnGrabbed);
                        eventScript.InteractableObjectUngrabbed -= new InteractableObjectEventHandler(OnReleased);
                    }
                }
            }
        }
    }
}
