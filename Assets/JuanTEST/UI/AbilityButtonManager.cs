using Motherbase;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonManager : MonoBehaviour
{
    public Button abilityButton;
    private ButtonCostDisplay buttonCostDisplay;
    private PlayerCurrency playerCurrency;

    private void Start()
    {
        playerCurrency = PlayerCurrency.Instance;
        buttonCostDisplay = GetComponent<ButtonCostDisplay>();

        if (playerCurrency != null)
        {
            playerCurrency.OnBalanceChanged += UpdateAbilityButton;
        }
        
        UpdateAbilityButton(playerCurrency.Balance);
    }

    public void UpdateAbilityButton(int currentBalance)
    {
        if (playerCurrency != null && buttonCostDisplay != null)
        {
            abilityButton.interactable = currentBalance >= buttonCostDisplay.cost;
        }
    }
}


