using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public RectTransform buttonRect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            LoadScene(sceneName);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (buttonRect != null && RectTransformUtility.RectangleContainsScreenPoint(
                buttonRect, Mouse.current.position.ReadValue()))
            {
                LoadScene(sceneName);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
