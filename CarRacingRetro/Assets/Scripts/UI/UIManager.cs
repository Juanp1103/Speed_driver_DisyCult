using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RaceManager race;
    public LapTracker lap;
    public TextMeshProUGUI lapText, timeText, countdownText;

    void Start()
    {
        race.OnCountdown += n => countdownText.text = n == 0 ? "GO!" : n.ToString();
        race.OnCountdown += n => { if (n == 0) Invoke(nameof(HideCountdown), 1f); };
        race.OnFinish += (_,_,_) => countdownText.text = "FINISH";
        lap.OnLapCompleted += n => { };
    }

    void HideCountdown() => countdownText.text = "";

    void Update()
    {
        lapText.text = $"LAP {Mathf.Min(lap.CurrentLap + 1, lap.totalLaps)}/{lap.totalLaps}";
        int m = (int)(race.raceTime / 60);
        float s = race.raceTime % 60;
        timeText.text = $"{m:00}:{s:00.00}";
    }
}