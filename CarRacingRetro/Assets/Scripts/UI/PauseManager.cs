using UnityEngine;
using UnityEngine.InputSystem;   

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public SceneNavigator nav;
    public RaceManager race;

    private bool paused = false;
    private CarControls controls;

    void Awake()
    {
        controls = new CarControls();
        pausePanel.SetActive(false);
    }
    void OnEnable()  => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // usa el botón Start/Escape para pausar — añade un action "Pause" a tu asset,
        // o de momento usa el teclado directo:
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        if (race != null && !race.racing) return; // no pausar si ya terminó
        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    public void Resume()  { paused = false; pausePanel.SetActive(false); Time.timeScale = 1f; }
    public void Restart() { Time.timeScale = 1f; nav.LoadGame(); }
    public void QuitToMenu() { Time.timeScale = 1f; nav.LoadMenu(); }
}