using UnityEngine;
using TMPro;
using UnityEngine.UI; // For Image component

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; } // Singleton Instance

    public GameObject tutorialPanel;        // Panel for tutorial UI
    public TextMeshProUGUI tutorialText;    // Text for displaying instructions
    public TextMeshProUGUI nextHintText;    // Text to show "Press Space to Next"
    public Image tutorialImage;            // Image component for tutorial illustrations
    public GameObject[] highlights;        // Objects to highlight (e.g., buttons or mechanics)

    private int currentStep = 0;

    private string[] tutorialSteps =
    {
        "Welcome! Use the arrow keys or WASD to move.",
        "Drag and click to shoot.",
        "Attack enemies to increase your score.",
        "Solve the maze to win the game!"
    };

    public Sprite[] tutorialImages; // Array of images for each step

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the static instance
        }
        else
        {
            DestroyImmediate(gameObject); // Prevent duplicate instances
        }
    }

    private void Start()
    {
        ShowStep(0); // Start with the first step
    }

    private void Update()
    {
        if (tutorialPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }
    }

    public void ShowStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= tutorialSteps.Length) return;

        currentStep = stepIndex;
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialSteps[stepIndex];
        nextHintText.text = "Press SPACE to the Next"; // Show hint text

        // Set the corresponding image for the step
        if (tutorialImages != null && stepIndex < tutorialImages.Length && tutorialImages[stepIndex] != null)
        {
            tutorialImage.sprite = tutorialImages[stepIndex];
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false); // Hide image if none is assigned
        }

        // Highlight only the current step
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].SetActive(i == stepIndex);
        }

        Time.timeScale = 0f; // Pause the game during the tutorial
    }

    public void NextStep()
    {
        if (currentStep + 1 < tutorialSteps.Length)
        {
            ShowStep(currentStep + 1);
        }
        else
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        tutorialPanel.SetActive(false);
        foreach (var highlight in highlights)
        {
            highlight.SetActive(false);
        }

        Time.timeScale = 0f; // Resume the game after the tutorial ends

    }
}
