using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverUI : MonoBehaviour
{
    public string mainMenuScene = "主场景";
    public float delayBeforeShow = 1.5f;
    public int scorePerObstacle = 5;
    private bool isGameOver = false;
    private bool gameOverShown = false;
    private float gameOverTime;
    private GameObject panel;
    private GameObject scoreTextGo;
    private Text scoreText;
    private Text finalScoreText;
    private RectTransform buttonRect;
    private int score = 0;
    private HashSet<int> scoredObstacles = new HashSet<int>();

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

        // Score display
        scoreTextGo = new GameObject("ScoreText");
        scoreTextGo.transform.SetParent(canvasGo.transform, false);
        RectTransform scoreRect = scoreTextGo.AddComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0.5f, 1f);
        scoreRect.anchorMax = new Vector2(0.5f, 1f);
        scoreRect.pivot = new Vector2(0.5f, 1f);
        scoreRect.sizeDelta = new Vector2(300, 60);
        scoreRect.anchoredPosition = new Vector2(0, -20);
        scoreText = scoreTextGo.AddComponent<Text>();
        scoreText.text = "Score: 0";
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        scoreText.fontSize = 36;
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = Color.white;

        // Game Over panel
        panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(canvasGo.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(600, 420);
        panelRect.anchoredPosition = Vector2.zero;
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);

        GameObject textGo = new GameObject("GameOverText");
        textGo.transform.SetParent(panel.transform, false);
        RectTransform textRect = textGo.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.7f);
        textRect.anchorMax = new Vector2(0.5f, 0.7f);
        textRect.sizeDelta = new Vector2(500, 100);
        textRect.anchoredPosition = Vector2.zero;
        Text text = textGo.AddComponent<Text>();
        text.text = "Game Over!";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 80;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.red;

        // Final score
        GameObject finalScoreGo = new GameObject("FinalScoreText");
        finalScoreGo.transform.SetParent(panel.transform, false);
        RectTransform finalScoreRect = finalScoreGo.AddComponent<RectTransform>();
        finalScoreRect.anchorMin = new Vector2(0.5f, 0.5f);
        finalScoreRect.anchorMax = new Vector2(0.5f, 0.5f);
        finalScoreRect.sizeDelta = new Vector2(400, 60);
        finalScoreRect.anchoredPosition = Vector2.zero;
        finalScoreText = finalScoreGo.AddComponent<Text>();
        finalScoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        finalScoreText.fontSize = 40;
        finalScoreText.alignment = TextAnchor.MiddleCenter;
        finalScoreText.color = Color.white;

        // Return button
        GameObject btnGo = new GameObject("ReturnButton");
        btnGo.transform.SetParent(panel.transform, false);
        RectTransform btnRect = btnGo.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.2f);
        btnRect.anchorMax = new Vector2(0.5f, 0.2f);
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
                    scoreTextGo.SetActive(false);
                }
            }
        }

        if (!gameOverShown)
        {
            CheckObstacleScoring();
        }

        if (isGameOver && !gameOverShown && Time.time - gameOverTime >= delayBeforeShow)
        {
            gameOverShown = true;
            finalScoreText.text = "Score: " + score;
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

    void CheckObstacleScoring()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obs in obstacles)
        {
            int id = obs.GetInstanceID();
            if (!scoredObstacles.Contains(id) && obs.transform.position.x < -3f)
            {
                scoredObstacles.Add(id);
                score += scorePerObstacle;
                scoreText.text = "Score: " + score;
            }
        }
    }
}
