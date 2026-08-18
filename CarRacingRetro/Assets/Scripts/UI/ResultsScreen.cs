using UnityEngine;
using UnityEngine.UI;
using System.Collections;   // ← añade esto
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public RaceManager race;
    public SceneNavigator nav;

    [Header("Paneles")]
    public GameObject resultsPanel;
    public GameObject initialsEntry;

    [Header("Textos")]
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI messageText;
    public TMP_InputField initialsInput;
    public Button submitButton;
    public Button retryButton;
    public Button menuButton;

    [Header("Timing")]
    public float delayBeforePanel = 2f;   // ← segundos que se ve "FINISH" antes del panel

    private float finalTime;
    private int qualifyPos = -1;

    void Start()
    {
        resultsPanel.SetActive(false);
        race.OnFinish += OnRaceFinished;

        submitButton.onClick.AddListener(SubmitInitials);
        retryButton.onClick.AddListener(() => nav.LoadGame());
        menuButton.onClick.AddListener(() => nav.LoadMenu());
    }

    // se llama al terminar: NO abre el panel aún, lanza la espera
    void OnRaceFinished(float time, float previousBest, bool isRecord)
    {
        finalTime = time;
        StartCoroutine(ShowResultsDelayed());
    }

    IEnumerator ShowResultsDelayed()
    {
        yield return new WaitForSeconds(delayBeforePanel);
        ShowResults();
    }

    void ShowResults()
    {
        resultsPanel.SetActive(true);
        finalTimeText.text = FormatTime(finalTime);

        qualifyPos = Scoreboard.QualifiesAt(finalTime);

        if (qualifyPos >= 0)
        {
            messageText.text = "NEW HIGH SCORE!";
            initialsEntry.SetActive(true);
            retryButton.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(false);
            initialsInput.characterLimit = 3;
            initialsInput.text = "";
            initialsInput.Select();
        }
        else
        {
            messageText.text = "TIME";
            initialsEntry.SetActive(false);
        }
    }

    void SubmitInitials()
    {
        string ini = initialsInput.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(ini)) ini = "AAA";
        Scoreboard.AddScore(ini, finalTime);

        initialsEntry.SetActive(false);
        retryButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        messageText.text = "SAVED!";
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60);
        float s = t % 60;
        return $"{m:00}:{s:00.00}";
    }
}