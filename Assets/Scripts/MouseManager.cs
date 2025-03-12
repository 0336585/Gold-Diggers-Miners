using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_EDITOR
        LockMouse();
#else
        UnlockMouse();
#endif
    }

    private void Update()
    {
#if !UNITY_EDITOR
        // Lock and hide mouse when the player starts the game (in build)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Escape key will unlock the cursor in the build version
            UnlockMouse();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Lock cursor again after clicking  
            LockMouse();
        }
#endif
    }

    // Function to lock the mouse cursor inside the game window
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Confined; // Locks the cursor to the game window
        Cursor.visible = false;  
    }

    // Function to unlock the mouse cursor  
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor from the game window
        Cursor.visible = true;  
    }
}
