using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public ThirdPersonCamera thirdPerson;
    public TopDownCamera topDown;

    void Start()
    {
        SetThirdPerson();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (thirdPerson.enabled)
                SetTopDown();
            else
                SetThirdPerson();
        }
    }

    void SetThirdPerson()
    {
        thirdPerson.enabled = true;
        topDown.enabled = false;
    }

    void SetTopDown()
    {
        thirdPerson.enabled = false;
        topDown.enabled = true;
    }
}
