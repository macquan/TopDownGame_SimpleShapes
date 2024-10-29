using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI timerText;
    public Button retryButton;

    public GameObject player;
    private Vector3 playerStartPosition = Vector3.zero;

    private float score;
    private float timer;
    private bool isGameActive = false; 


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
        NewGame();
    }

    public void NewGame()
    {
        score = 0f;
        timer = 0f;
        scoreText.text = "S: " + Mathf.FloorToInt(score).ToString(); 

        Time.timeScale = 1f;
        enabled = true;
        isGameActive = true; 
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

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

        UpdateHiscore();
    }

    private void Update()
    {
        if (isGameActive) 
        {
            timer += Time.deltaTime;
            timerText.text = "TIME: " + timer.ToString("F2");

            scoreText.text = "S: " + Mathf.FloorToInt(score).ToString();
        }
    }

    public void IncreaseScore()
    {
        if (isGameActive)
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
}
