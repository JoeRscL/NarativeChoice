using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime; 
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText; 
    public Transform choiceButtonContainer; 
    public GameObject choiceButtonPrefab; 

    [Header("Story")]
    public TextAsset inkJSONAsset; 
    private Story story;

    void Start()
    {
        StartStory();
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        RefreshView();
    }

    void RefreshView()
    {
        
        foreach (Transform child in choiceButtonContainer)
            Destroy(child.gameObject);

        
        if (story.canContinue)
        {
            string text = story.Continue();
            dialogueText.text = text;
        }
        else
        {
            dialogueText.text = "Sesi berakhir."; 
        }

        
        if (story.currentChoices.Count > 0)
        {
            foreach (Choice choice in story.currentChoices)
            {
                GameObject button = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                
                button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                
                button.GetComponent<Button>().onClick.AddListener(() => {
                    OnClickChoice(choice.index);
                });
            }
        }
    }

    void OnClickChoice(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex); 
        RefreshView(); 
    }
}