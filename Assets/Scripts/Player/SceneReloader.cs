using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

