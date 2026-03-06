using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText;
    public GameObject nextButton;
    public TextMeshProUGUI dayText;

    [Header("Status Bars")]
    public Slider faithSlider;
    public Slider wealthSlider;

    private int currentDay = 1;

    [Header("3D Card Settings")]
    public GameObject cardPrefab;
    public Transform[] cardSpawnPoints;
    private List<GameObject> activeCards = new List<GameObject>();

    [Header("Day Transition")]
    public GameObject eveningPanel;
    public TextMeshProUGUI sisterReportText;
    public CanvasGroup faderCanvasGroup;

    [Header("Story")]
    public TextAsset inkJSONAsset;
    private Story story;

    private Coroutine typingCoroutine;

    void Start()
    {
        currentDay = PlayerPrefs.GetInt("Save_Day", 1);
        UpdateDayUI();
        StartStory();
    }

    void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
        }
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);

        if (story.variablesState.Contains("hari_ini"))
        {
            story.variablesState["hari_ini"] = currentDay;
        }

        if (PlayerPrefs.HasKey("Save_Faith"))
        {
            story.variablesState["faith"] = PlayerPrefs.GetInt("Save_Faith");
        }
        if (PlayerPrefs.HasKey("Save_Wealth"))
        {
            story.variablesState["wealth"] = PlayerPrefs.GetInt("Save_Wealth");
        }

        RefreshView();
    }

    public void RefreshView()
    {
        foreach (GameObject card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        if (story.canContinue)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(story.Continue()));

            UpdateStatusUI();
        }
        else
        {
            EndConfession();
            return;
        }

        if (story.currentChoices.Count > 0)
        {
            if (nextButton != null) nextButton.SetActive(false);

            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                if (i < cardSpawnPoints.Length)
                {
                    GameObject newCard = Instantiate(cardPrefab, cardSpawnPoints[i].position, cardSpawnPoints[i].rotation);
                    FateCard cardScript = newCard.GetComponent<FateCard>();
                    cardScript.choiceIndex = i;
                    cardScript.manager = this;

                    if (cardScript.cardNameText != null)
                    {
                        cardScript.cardNameText.text = choice.text;
                    }

                    activeCards.Add(newCard);
                }
            }
        }
        else
        {
            if (nextButton != null) nextButton.SetActive(true);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    void UpdateStatusUI()
    {
        if (story.variablesState.Contains("faith") && faithSlider != null)
        {
            faithSlider.value = (int)story.variablesState["faith"];
        }
        if (story.variablesState.Contains("wealth") && wealthSlider != null)
        {
            wealthSlider.value = (int)story.variablesState["wealth"];
        }
    }

    public void SelectChoice(int index)
    {
        story.ChooseChoiceIndex(index);
        RefreshView();
    }

    void EndConfession()
    {
        string hasilEnding = "";

        if (story.variablesState.Contains("ending_didapat"))
        {
            hasilEnding = story.variablesState["ending_didapat"].ToString();
        }

        if (hasilEnding == "game_over")
        {
            PlayerPrefs.DeleteKey("Save_Day");
            PlayerPrefs.DeleteKey("Save_Faith");
            PlayerPrefs.DeleteKey("Save_Wealth");
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        string laporan = "";
        if (hasilEnding == "good_ending")
        {
            laporan = "Suster Amara: 'Mother, orang tadi terlihat jauh lebih damai setelah keluar dari bilik Anda.'";
        }
        else if (hasilEnding == "bad_ending")
        {
            laporan = "Suster Kael: 'Mother... orang tadi baru saja ditangkap penjaga di gerbang desa. Apa yang terjadi?'";
        }
        else
        {
            laporan = "Malam ini sunyi, tidak ada kabar dari para Suster.";
        }

        if (eveningPanel != null && sisterReportText != null)
        {
            sisterReportText.text = laporan;
            eveningPanel.SetActive(true);
        }

        if (nextButton != null) nextButton.SetActive(false);
    }

    public void SleepNextDay()
    {
        if (faderCanvasGroup != null)
        {
            StartCoroutine(DoFade(1f, () => {
                ProcessNextDay();
                StartCoroutine(DoFade(0f));
            }));
        }
        else
        {
            ProcessNextDay();
        }
    }

    private void ProcessNextDay()
    {
        currentDay++;
        PlayerPrefs.SetInt("Save_Day", currentDay);

        if (story.variablesState.Contains("faith"))
        {
            PlayerPrefs.SetInt("Save_Faith", (int)story.variablesState["faith"]);
        }
        if (story.variablesState.Contains("wealth"))
        {
            PlayerPrefs.SetInt("Save_Wealth", (int)story.variablesState["wealth"]);
        }

        PlayerPrefs.Save();
        UpdateDayUI();

        if (eveningPanel != null)
        {
            eveningPanel.SetActive(false);
        }

        StartStory();
    }

    IEnumerator DoFade(float targetAlpha, System.Action onComplete = null)
    {
        float speed = 2f;
        while (!Mathf.Approximately(faderCanvasGroup.alpha, targetAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        faderCanvasGroup.alpha = targetAlpha;
        onComplete?.Invoke();
    }
}