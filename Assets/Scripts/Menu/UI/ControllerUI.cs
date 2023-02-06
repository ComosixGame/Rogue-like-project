using UnityEngine;
using UnityEngine.InputSystem;


public class ControllerUI : MonoBehaviour
{
    public RectTransform joystickRectTrans;
    private InputAssets inputs;

    private void Awake() {
        joystickRectTrans.position = new Vector2(9999999, 9999999);
        inputs = new InputAssets();
    }

    private void OnEnable() {
        inputs.Touch.Enable();
        inputs.Touch.StartTouch.performed += ShowJoystick;
        inputs.Touch.HoldTouch.canceled += HideJoystick;

    }

    private void OnDisable() {
        inputs.Touch.Disable();
        inputs.Touch.StartTouch.performed -= ShowJoystick;
        inputs.Touch.HoldTouch.canceled -= HideJoystick;
    }

    private void ShowJoystick(InputAction.CallbackContext ctx) {
        joystickRectTrans.position = ctx.ReadValue<Vector2>();
        
    }

    private void HideJoystick(InputAction.CallbackContext ctx) {
        joystickRectTrans.position = new Vector2(9999999, 9999999);
    }
}
