using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public float blinkInterval = 0.6f;

    [Tooltip("Si esta activo, el parpadeo funciona incluso cuando el juego esta pausado.")]
    public bool ignorePause = true;

    private TMP_Text tmp;
    private float timer;

    void Start()
    {
        tmp = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // Si ignorePause = true, usa unscaledDeltaTime (siempre avanza)
        // Si ignorePause = false, usa deltaTime (se detiene con Time.timeScale = 0)
        timer += ignorePause ? Time.unscaledDeltaTime : Time.deltaTime;

        if (timer >= blinkInterval)
        {
            tmp.enabled = !tmp.enabled;
            timer = 0;
        }
    }
}