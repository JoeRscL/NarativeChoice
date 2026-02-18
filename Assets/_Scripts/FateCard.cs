using UnityEngine;

public class FateCard : MonoBehaviour
{
    public int choiceIndex; 
    public DialogueManager manager; 

    
    private Color startColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    
    void OnMouseEnter()
    {
        rend.material.color = Color.yellow; 
    }

    
    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    
    void OnMouseDown()
    {
        
        manager.SelectChoice(choiceIndex);
    }
}