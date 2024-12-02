using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winGameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI timerText;
    public Button retryButton;
    public Button mainMenuButton;

    public GameObject menuPanel;
    public Button startButton;
    public Button settingsButton;
    public Button tutorialButton;
    public Button exitButton;

    public GameObject tutorialPanel;

    public GameObject player;
    private Vector3 playerStartPosition = new Vector3(-8f, 4.6f, 0f);

    private float score;
    private float timer;
    private float elapsedTime = 0f;
    private bool isGameActive = false;

    public int gameSessionId = 0;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        if (player != null)
        {
            playerStartPosition = player.transform.position;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        retryButton.onClick.AddListener(NewGame);
        mainMenuButton.onClick.AddListener(() =>
        {
            ShowMenu(); 
            ResetGame(); 
        });

        startButton.onClick.AddListener(() =>
        {
            HideMenu();
            NewGame();
        });

        settingsButton.onClick.AddListener(() => Debug.Log("Settings button is not active."));

        exitButton.onClick.AddListener(() => ExitGame());

        Button tutorialButton = tutorialPanel.transform.Find("BackButton").GetComponent<Button>();
        tutorialButton.onClick.AddListener(ShowMenu);

        ShowMenu();
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        menuPanel.SetActive(false);
        SetGameUIActive(false);
        Time.timeScale = 0f;
    }

    private void ShowMenu()
    {
        menuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        winGameText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        SetGameUIActive(false);
        Time.timeScale = 0f;
        isGameActive = false;
    }

    private void HideMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameActive = true;
    }

    public void NewGame()
    {
        if (!isGameActive)
        {
            gameSessionId++;
        }

        score = 0f;
        timer = 0f;
        scoreText.text = "S: " + Mathf.FloorToInt(score).ToString(); 

        Time.timeScale = 1f;
        enabled = true;
        isGameActive = true; 
        winGameText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        SetGameUIActive(true);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        if (player != null)
        {
            player.transform.position = playerStartPosition;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        UpdateHiscore();
    }


    public void GameOver()
    {
        enabled = false;
        Time.timeScale = 0f;
        isGameActive = false; 

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        UpdateHiscore();

        Debug.Log("Lose");
    }

    private void Update()
    {
        if (isGameActive) 
        {
            timer += Time.deltaTime;
            timerText.text = "TIME: " + timer.ToString("F2");

            scoreText.text = "S: " + Mathf.FloorToInt(score).ToString();
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 5f)
        {
            Debug.Log("5 seconds");
            elapsedTime = 0f;
        }
    }

    public void IncreaseScore(int enemySessionId)
    {
        if (isGameActive && enemySessionId == gameSessionId)
        {
            score += 1;
            scoreText.text = "S: " + Mathf.FloorToInt(score).ToString();
        }
    }


    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = "HS: " + Mathf.FloorToInt(hiscore).ToString();
    }
    public void WinGame()
    {
        enabled = false;
        Time.timeScale = 0f;
        isGameActive = false;

        winGameText.text = "WIN!!!";
        winGameText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        Debug.Log("Win");

        UpdateHiscore();
    }

    private void ResetGame()
    {
        score = 0f;
        timer = 0f;
        elapsedTime = 0f;
        isGameActive = false;

        if (player != null)
        {
            player.transform.position = playerStartPosition;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        retryButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        winGameText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }
    private void SetGameUIActive(bool isActive)
    {
        if (timerText != null) timerText.gameObject.SetActive(isActive);
        if (scoreText != null) scoreText.gameObject.SetActive(isActive);
        if (hiscoreText != null) hiscoreText.gameObject.SetActive(isActive);
    }
    private void ExitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

}
