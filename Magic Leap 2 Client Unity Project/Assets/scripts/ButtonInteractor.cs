using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonInteractor : MonoBehaviour
{
    public CameraManager cameraManager;

    private MagicLeapInputs _magicLeapInputs;
    private MagicLeapInputs.ControllerActions _controllerActions;

    private float triggerValue = 0; 
    private bool bumperDown = false;

    private bool triggerPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the InputActionAsset
        _magicLeapInputs = new MagicLeapInputs();
        _magicLeapInputs.Enable();

        //Initialize the ControllerActions using the Magic Leap Input
        _controllerActions = new MagicLeapInputs.ControllerActions(_magicLeapInputs);

        //Subscribe to your choice of the controller events
        _controllerActions.IsTracked.performed += IsTrackedOnPerformed;
        _controllerActions.Trigger.performed += HandleOnTrigger;
        _controllerActions.Bumper.performed += HandleOnBumper;
    }

    // Update is called once per frame
    void Update()
    {


        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (triggerValue > .3 && !triggerPressed){
                triggerPressed = true;
                if (hit.transform.name == "increase") {
                    Debug.Log("Did Hit Button");
                    cameraManager.IncreaseCameraIndex();
                }
                if (hit.transform.name == "decrease") {
                    Debug.Log("Did Hit Button");
                    cameraManager.DecreaseCameraIndex();
                }
                Debug.Log(hit.transform.name);
            }
        }

        if (triggerValue < .3) {
            triggerPressed = false;
        }
    }

    // Handles the event to determine if the controller is tracking.
    private void IsTrackedOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("The Controller Is tracking");
    }

    // Handles the event for the Trigger.
    private void HandleOnTrigger(InputAction.CallbackContext obj)
    {
        triggerValue = obj.ReadValue<float>();
        //Debug.Log("The Trigger value is : " +  triggerValue);
    }

    // Handles the event for the Bumper.
    private void HandleOnBumper(InputAction.CallbackContext obj)
    {
        bumperDown = obj.ReadValueAsButton();
        //Debug.Log("The Bumper is pressed down " + bumperDown);
        //Debug.Log("The Bumper was released this frame: " + obj.action.WasReleasedThisFrame());
    }

    // Handles the disposing all of the input events.
    void OnDestroy()
    {
        _controllerActions.IsTracked.performed -= IsTrackedOnPerformed;
        _controllerActions.Trigger.performed -= HandleOnTrigger;
        _controllerActions.Bumper.performed -= HandleOnBumper;
        _magicLeapInputs.Dispose();
    }
}
