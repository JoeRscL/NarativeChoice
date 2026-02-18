using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText;

    [Header("3D Card Settings")] 
    public GameObject cardPrefab; 
    public Transform[] cardSpawnPoints; 

    
    private List<GameObject> activeCards = new List<GameObject>();

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
        
        foreach (GameObject card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
        }
        else
        {
            dialogueText.text = ""; 
        }

        
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];

                
                if (i < cardSpawnPoints.Length)
                {
                    
                    GameObject newCard = Instantiate(cardPrefab, cardSpawnPoints[i].position, cardSpawnPoints[i].rotation);

                    FateCard cardScript = newCard.GetComponent<FateCard>();
                    cardScript.choiceIndex = i;
                    cardScript.manager = this; 
                    activeCards.Add(newCard);
                }
            }
        }
    }

    public void SelectChoice(int index)
    {
        story.ChooseChoiceIndex(index);
        RefreshView();
    }
}