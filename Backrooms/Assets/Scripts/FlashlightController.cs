using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight")]
    public Light spotlight;

    [Header("Arm UI")]
    public Image armImage;
    public Sprite armWithFlashlight;
    public Sprite armWithoutFlashlight;

    [Header("Toggle")]
    public KeyCode flashlightKey = KeyCode.F;

    private bool isOn = false;

    void Start()
    {
        // ✅ Flashlight off by default
        spotlight.enabled = false;
        armImage.sprite = armWithoutFlashlight;
    }

    void Update()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            isOn = !isOn;
            spotlight.enabled = isOn;
            armImage.sprite = isOn ? armWithFlashlight : armWithoutFlashlight;
        }
    }
}