using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public struct ScoreEntry
{
    public string initials;
    public float time;
}

public static class Scoreboard
{
    const int MAX_ENTRIES = 10;
    const string KEY = "scoreboard";

    [System.Serializable]
    class ScoreList { public List<ScoreEntry> entries = new List<ScoreEntry>(); }

    public static List<ScoreEntry> GetScores()
    {
        if (!PlayerPrefs.HasKey(KEY)) return new List<ScoreEntry>();
        return JsonUtility.FromJson<ScoreList>(PlayerPrefs.GetString(KEY)).entries;
    }

    // devuelve la posición (0-9) si entra al top, o -1 si no califica
    public static int QualifiesAt(float time)
    {
        var list = GetScores();
        for (int i = 0; i < list.Count; i++)
            if (time < list[i].time) return i;
        return list.Count < MAX_ENTRIES ? list.Count : -1;
    }

    public static void AddScore(string initials, float time)
    {
        var list = GetScores();
        list.Add(new ScoreEntry { initials = initials, time = time });
        list.Sort((a, b) => a.time.CompareTo(b.time));
        if (list.Count > MAX_ENTRIES) list.RemoveRange(MAX_ENTRIES, list.Count - MAX_ENTRIES);

        var wrapper = new ScoreList { entries = list };
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(wrapper));
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(KEY);
        PlayerPrefs.Save();
    }
}