using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct GhostFrame
{
    public Vector2 position;
    public float rotation;
    public float time;
}

public class GhostRecorder : MonoBehaviour
{
    public float sampleRate = 0.05f;
    private List<GhostFrame> frames = new List<GhostFrame>();
    private float timer, elapsed;
    public bool recording = true;

    void Update()
    {
        if (!recording) return;
        elapsed += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= sampleRate)
        {
            timer = 0f;
            frames.Add(new GhostFrame {
                position = transform.position,
                rotation = transform.eulerAngles.z,
                time = elapsed
            });
        }
    }

    public List<GhostFrame> GetFrames() => frames;

    // Guardar como JSON (PlayerPrefs simple para el prototipo)
    public void SaveGhost(string key = "best_ghost")
    {
        GhostData data = new GhostData { frames = frames };
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    }

    [System.Serializable] public class GhostData { public List<GhostFrame> frames; }
}