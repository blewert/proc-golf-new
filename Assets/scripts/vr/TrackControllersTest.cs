using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackControllersTest : MonoBehaviour
{
    public Transform controllerLeft;
    public Transform controllerRight;

    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // var localLeft = OVRInput.GetLocalControllerPosition(leftController);
        // var localRight = OVRInput.GetLocalControllerPosition(rightController);

        // controllerLeft.localPosition = (localLeft);
        // controllerRight.localPosition = (localRight);

        // Debug.Log(controllerLeft.position);
    }
}
