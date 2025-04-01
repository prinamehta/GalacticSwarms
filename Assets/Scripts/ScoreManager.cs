using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text highscoreText;

    int score = 0;
    int highscore = 0;

    public void Awake()
    {
        // Proper singleton pattern implementation
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Check if text references are assigned
        if (scoreText == null || highscoreText == null)
        {
            Debug.LogError("Score or highscore Text references missing in ScoreManager!");
            return;
        }

        highscore = PlayerPrefs.GetInt("highscore", 0);
        UpdateScoreDisplay();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
        
        Debug.Log("ScoreManager initialized with score: " + score);
    }

    public void AddPoint()
    {
        score += 10;
        UpdateScoreDisplay();
        
        if (highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", score);
            highscoreText.text = "HIGHSCORE: " + highscore.ToString();
        }
        
        Debug.Log("Score increased to: " + score);
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString() + " POINTS";
        }
    }
}