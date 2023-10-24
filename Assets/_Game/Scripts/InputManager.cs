using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class InputManager : MonoBehaviour
{
    [SerializeField] private Vector2 joyStickSize = new Vector2(250, 250);
    [SerializeField] private JoyStick joyStick;

    private Finger movementFinger;
    private Vector2 movementAmount;

    public Vector2 MovementAmount { get => movementAmount; set => movementAmount = value; }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;

    }
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();

    }
    private void HandleFingerUp(Finger finger)
    {
        if (finger == movementFinger)
        {
            movementFinger = null;
            joyStick.Knob.anchoredPosition = Vector2.zero;
            joyStick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerMove(Finger finger)
    {
        if (finger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joyStickSize.x / 2f;
            ETouch.Touch currentTouch = movementFinger.currentTouch;
            if (Vector2.Distance(currentTouch.screenPosition, joyStick.joyStickObj.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joyStick.joyStickObj.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joyStick.joyStickObj.anchoredPosition;
            }
            joyStick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;

        }
    }

    private void HandleFingerDown(Finger finger)
    {
        //neu ngon tay di chuyen nua man hinh+
        if (movementFinger == null && finger.screenPosition.x <= Screen.width)
        {
            movementFinger = finger;
            MovementAmount = Vector2.zero;
            joyStick.gameObject.SetActive(true);
            joyStick.joyStickObj.sizeDelta = joyStickSize;
            joyStick.joyStickObj.anchoredPosition = ClampStartPosition(finger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 screenPosition)
    {
        if (screenPosition.x < joyStickSize.x / 2)
        {
            screenPosition.x = joyStickSize.x / 2;
        }
        if (screenPosition.y < joyStickSize.y / 2)
        {
            screenPosition.y = joyStickSize.y / 2;
        }
        else if (screenPosition.y > Screen.height - joyStickSize.y / 2)
        {
            screenPosition.y = Screen.height - joyStickSize.y / 2;
        }
        return screenPosition;
    }
}
