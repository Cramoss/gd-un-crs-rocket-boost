using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        // Q key quits the application (only works in builds, not in editor)
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }
}
