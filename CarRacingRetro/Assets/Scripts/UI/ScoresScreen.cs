using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ScoresScreen : MonoBehaviour
{
    public SceneNavigator nav;
    public Transform rowContainer;  // un contenedor vertical (Vertical Layout Group)
    public GameObject rowPrefab;    // prefab con dos Text: rank+initials, time

    void Start()
    {
        var scores = Scoreboard.GetScores();
        string[] ranks = { "1ST","2ND","3RD","4TH","5TH","6TH","7TH","8TH","9TH","10TH" };

        for (int i = 0; i < 10; i++)
        {
            GameObject row = Instantiate(rowPrefab, rowContainer);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            if (i < scores.Count)
            {
                texts[0].text = $"{ranks[i]}  {scores[i].initials}";
                texts[1].text = FormatTime(scores[i].time);
            }
            else
            {
                texts[0].text = $"{ranks[i]}  ---";
                texts[1].text = "--:--.--";
            }
        }
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60);
        float s = t % 60;
        return $"{m:00}:{s:00.00}";
    }

    public void OnBack() => nav.LoadMenu();
}