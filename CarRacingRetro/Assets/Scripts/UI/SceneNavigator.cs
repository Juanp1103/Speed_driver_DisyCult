using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void LoadMenu()   => SceneManager.LoadScene("MainMenu");
    public void LoadGame()   => SceneManager.LoadScene("Game");
    public void LoadScores() => SceneManager.LoadScene("Scores");
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}