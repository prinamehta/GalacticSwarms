using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this script to any GameObject in your scene
public class SceneLoader : MonoBehaviour
{
    // This will load the scene when you press the L key
    void Update()
    {
        // Check if L key was pressed this frame
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L key pressed! Loading Level 1...");
            LoadLevel1();
        }
    }

    // Public method to load Level 1 
    public void LoadLevel1()
    {
        Debug.Log("Loading Level 1 scene...");
        
        // First method: Load by name (ensure exact spelling and capitalization match)
        try
        {
            SceneManager.LoadScene("Level 1");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load by name: " + e.Message);
            
            // Fallback method: Try to load by index
            try
            {
                // Assuming Level 1 is the second scene in build settings (index 1)
                SceneManager.LoadScene(1);
                Debug.Log("Loaded Level 1 by index");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to load by index: " + ex.Message);
            }
        }
    }
}