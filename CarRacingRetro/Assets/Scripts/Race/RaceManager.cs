using UnityEngine;
using System;
using System.Collections;

public class RaceManager : MonoBehaviour
{
    public CarInput playerInput;
    public LapTracker lapTracker;
    public GhostRecorder ghostRecorder;

    public float raceTime { get; private set; }
    public bool racing { get; private set; }

    public event Action<int> OnCountdown;
    // pasa: tiempo final, mejor tiempo previo, si fue récord
    public event Action<float, float, bool> OnFinish;

    const string BEST_TIME_KEY = "best_time";

    void Start() => StartCoroutine(Countdown());

    IEnumerator Countdown()
    {
        playerInput.enabled = false;
        for (int i = 3; i > 0; i--)
        {
            OnCountdown?.Invoke(i);
            yield return new WaitForSeconds(1f);
        }
        OnCountdown?.Invoke(0); // GO
        playerInput.enabled = true;
        racing = true;
        lapTracker.OnRaceFinished += Finish;
    }

    void Update() { if (racing) raceTime += Time.deltaTime; }

    void Finish()
    {
        racing = false;
        playerInput.enabled = false;

        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, Mathf.Infinity);
        bool isRecord = raceTime < bestTime;

        if (isRecord)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, raceTime);
            if (ghostRecorder != null)
            {
                ghostRecorder.recording = false;
                ghostRecorder.SaveGhost(); // guarda solo si mejoraste
            }
            PlayerPrefs.Save();
        }

        OnFinish?.Invoke(raceTime, bestTime, isRecord);
    }
}