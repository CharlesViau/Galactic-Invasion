using Motherbase;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonCostDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI costTextMeshPro; 
    public int cost; 
    private bool isPointerOver = false; 

    // Start is called before the first frame update
    void Start()
    {
        
        if (costTextMeshPro != null)
        {
            costTextMeshPro.gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        
        if (isPointerOver && costTextMeshPro != null)
        {
            costTextMeshPro.gameObject.SetActive(true);
            costTextMeshPro.text = "Cost: " + cost;
        }
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true; 
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false; // Set the flag to false
        // Hide the cost text if costTextMeshPro is not null
        if (costTextMeshPro != null)
        {
            costTextMeshPro.gameObject.SetActive(false);
        }
    }
}