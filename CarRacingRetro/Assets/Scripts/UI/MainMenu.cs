using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneNavigator nav; // arrastra un GameObject con SceneNavigator

    public GameObject controlsPanel; // arrastra el panel de controles desde el inspector

    public GameObject MenuPanel; // arrastra el panel del menú principal desde el inspector

    // conecta estos a los botones desde el inspector (OnClick)
    public void OnStart()  => nav.LoadGame();
    public void OnScores() => nav.LoadScores();
    public void OnQuit()   => nav.QuitGame();

    public void OnControls() 
    {
        MenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void OnCloseControls() 
    {
        controlsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
}