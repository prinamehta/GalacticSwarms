using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Scene names
    private const string MAIN_MENU = "MainMenu";
    private const string LEVEL_1 = "Level 1";
    private const string LEVEL_2 = "Level 2";
    private const string LEVEL_3 = "Level 3";
    private const string WIN_SCENE = "Win Scene";
    
    // Level progression thresholds
    private const int LEVEL_1_SCORE_THRESHOLD = 160;
    private const int LEVEL_2_SCORE_THRESHOLD = 400;
    private const int BOSS_DEFEAT_THRESHOLD = 2;
    
    // Current game state
    private int currentScore = 0;
    private int bossesDefeated = 0;

    // Singleton instance
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one LevelManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ResetGameState();
    }

    // Call this method to add points to the player's score
    public void AddScore(int points)
    {
        currentScore += points;
        CheckLevelProgression();
    }

    // Call this method when a boss is defeated
    public void BossDefeated()
    {
        bossesDefeated++;
        CheckLevelProgression();
    }

    // Reset the game state when starting a new game
    public void ResetGameState()
    {
        currentScore = 0;
        bossesDefeated = 0;
    }

    // Check if player meets criteria to progress to the next level
    private void CheckLevelProgression()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case LEVEL_1:
                if (currentScore >= LEVEL_1_SCORE_THRESHOLD)
                {
                    LoadLevel(LEVEL_2);
                }
                break;

            case LEVEL_2:
                if (currentScore >= LEVEL_2_SCORE_THRESHOLD)
                {
                    LoadLevel(LEVEL_3);
                }
                break;

            case LEVEL_3:
                if (bossesDefeated >= BOSS_DEFEAT_THRESHOLD)
                {
                    WinGame();
                }
                break;
        }
    }

    // Load a specific level
    public void LoadLevel(string levelName)
    {
        // Reset score when transitioning to a new level (except when going from level 1 to 2)
        if (SceneManager.GetActiveScene().name == LEVEL_2 && levelName == LEVEL_3)
        {
            currentScore = 0;
        }
        
        SceneManager.LoadScene(levelName);
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        ResetGameState();
        SceneManager.LoadScene(MAIN_MENU);
    }

    // Start a new game
    public void StartNewGame()
    {
        ResetGameState();
        LoadLevel(LEVEL_1);
    }

    // Win the game
    private void WinGame()
    {
        // If you have a win scene, load it
        if (SceneExistsInBuildSettings(WIN_SCENE))
        {
            SceneManager.LoadScene(WIN_SCENE);
        }
        else
        {
            Debug.Log("Game Won! Player defeated " + bossesDefeated + " bosses with a score of " + currentScore);
            // Return to main menu after winning
            LoadMainMenu();
        }
    }

    // Check if a scene exists in the build settings
    private bool SceneExistsInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string nameFromPath = path.Substring(path.LastIndexOf('/') + 1).Replace(".unity", "");
            
            if (nameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}