using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public string mainMenuScene = "主场景";
    public float delayBeforeShow = 1.5f;
    private bool isGameOver = false;
    private bool gameOverShown = false;
    private float gameOverTime;
    private GameObject panel;
    private RectTransform buttonRect;

    void Start()
    {
        CreateUI();
    }

    void CreateUI()
    {
        GameObject canvasGo = new GameObject("GameOverCanvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGo.AddComponent<GraphicRaycaster>();

        panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(canvasGo.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(600, 400);
        panelRect.anchoredPosition = Vector2.zero;
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);

        GameObject textGo = new GameObject("GameOverText");
        textGo.transform.SetParent(panel.transform, false);
        RectTransform textRect = textGo.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.65f);
        textRect.anchorMax = new Vector2(0.5f, 0.65f);
        textRect.sizeDelta = new Vector2(500, 100);
        textRect.anchoredPosition = Vector2.zero;
        Text text = textGo.AddComponent<Text>();
        text.text = "Game Over!";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 80;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.red;

        GameObject btnGo = new GameObject("ReturnButton");
        btnGo.transform.SetParent(panel.transform, false);
        RectTransform btnRect = btnGo.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.25f);
        btnRect.anchorMax = new Vector2(0.5f, 0.25f);
        btnRect.sizeDelta = new Vector2(260, 80);
        btnRect.anchoredPosition = Vector2.zero;
        Image btnImage = btnGo.AddComponent<Image>();
        btnImage.color = new Color(0.3f, 0.55f, 0.25f);
        btnGo.AddComponent<Button>();
        buttonRect = btnRect;

        GameObject btnTextGo = new GameObject("ReturnBtnText");
        btnTextGo.transform.SetParent(btnGo.transform, false);
        RectTransform btnTextRect = btnTextGo.AddComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.sizeDelta = Vector2.zero;
        Text btnText = btnTextGo.AddComponent<Text>();
        btnText.text = "Return to Menu";
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 30;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;

        panel.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null && pc.gameOver)
                {
                    isGameOver = true;
                    gameOverTime = Time.time;
                }
            }
        }

        if (isGameOver && !gameOverShown && Time.time - gameOverTime >= delayBeforeShow)
        {
            gameOverShown = true;
            panel.SetActive(true);
            Time.timeScale = 0;
        }

        if (isGameOver && gameOverShown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    buttonRect, Input.mousePosition))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(mainMenuScene);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
