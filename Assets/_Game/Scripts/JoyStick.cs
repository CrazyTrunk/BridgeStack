using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class JoyStick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform joyStickObj;
    public RectTransform Knob;

    private void Awake()
    {
        joyStickObj = GetComponent<RectTransform>();
    }
}
