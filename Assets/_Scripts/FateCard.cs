using UnityEngine;
using TMPro;

public class FateCard : MonoBehaviour
{
    public int choiceIndex;
    public DialogueManager manager;
    public TextMeshPro cardNameText;

    private Color originalColor;
    private Renderer cardRenderer;

    
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    [Header("Animation Settings")]
    public float liftAmount = 0.05f; 
    public float liftSpeed = 12f; 

    void Start() 
    {
        cardRenderer = GetComponent<Renderer>();
        if (cardRenderer != null)
        {
            originalColor = cardRenderer.material.color;
        }

        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * liftSpeed);
    }

    void OnMouseDown()
    {
        if (manager != null)
        {
            manager.SelectChoice(choiceIndex);
        }
    }

    void OnMouseEnter()
    {
       
        targetPosition = originalPosition + new Vector3(0, liftAmount, 0);

        if (cardRenderer != null)
        {
            cardRenderer.material.color = Color.yellow;
        }
    }

    void OnMouseExit()
    {
        
        targetPosition = originalPosition;

        if (cardRenderer != null)
        {
            cardRenderer.material.color = originalColor;
        }
    }
}