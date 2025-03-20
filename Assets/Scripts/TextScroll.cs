using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 180f;
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] private bool scrollUpwards = false;   

    private TextMeshProUGUI text;
    private RectTransform rectTransform;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = text.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Move the transform based on the scroll direction
        if (scrollUpwards)
        {
            rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
        else
        {
            rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;
        }

        // Check if the text has scrolled out of view and reset the position
        if (scrollUpwards)
        {
            if (rectTransform.anchoredPosition.y > rectTransform.rect.height)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            if (rectTransform.anchoredPosition.y < -rectTransform.rect.height)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        // Check if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}