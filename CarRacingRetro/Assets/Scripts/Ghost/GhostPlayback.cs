using UnityEngine;
using System.Collections.Generic;

public class GhostPlayback : MonoBehaviour
{
    private List<GhostFrame> frames;
    private float timer;
    private int idx;
    private bool playing = false;

    void Start() => Load();

    public void Load(string key = "best_ghost")
    {
        if (!PlayerPrefs.HasKey(key))
        {
            gameObject.SetActive(false); // primera vez: no hay fantasma
            return;
        }
        var data = JsonUtility.FromJson<GhostRecorder.GhostData>(PlayerPrefs.GetString(key));
        frames = data.frames;
        playing = frames != null && frames.Count >= 2;
    }

    void Update()
    {
        if (!playing) return;
        timer += Time.deltaTime;

        while (idx < frames.Count - 1 && frames[idx + 1].time < timer) idx++;
        if (idx >= frames.Count - 1) { playing = false; return; }

        GhostFrame a = frames[idx], b = frames[idx + 1];
        float t = Mathf.InverseLerp(a.time, b.time, timer);
        transform.position = Vector2.Lerp(a.position, b.position, t);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(a.rotation, b.rotation, t));
    }
}