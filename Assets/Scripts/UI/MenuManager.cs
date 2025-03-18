using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public bool inMenu { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Define events
    public event Action OnMenuOpen;

    public void MenuEvent()
    {
        OnMenuOpen();

        inMenu = true;
        Time.timeScale = 0;
    }

    public void MenuEventClosed()
    {
        inMenu = false;
        Time.timeScale = 1;
    }
}
