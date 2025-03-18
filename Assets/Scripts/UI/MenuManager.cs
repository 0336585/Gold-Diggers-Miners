using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Define events
    public event Action MenusToClose;

    public void CloseAllWindows()
    {
        MenusToClose();
    }


}
