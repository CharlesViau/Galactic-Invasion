using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAbilityButton : MonoBehaviour
{
    public List<Button> otherAbilityButtons;

    public bool towerAbilityPurchased;

    private void Start()
    {
        towerAbilityPurchased = false;
        SetOtherAbilityButtonsInteractable(false);
        StartCoroutine(InitializeButtons());
    }

    private IEnumerator InitializeButtons()
    {
        // Wait until the end of the frame to update the buttons
        yield return new WaitForEndOfFrame();
        UpdateOtherAbilityButtons();
    }

    /*private void OnTowerAbilityButtonClicked()
    {
        if (!towerAbilityPurchased)
        {
            towerAbilityPurchased = true;
            UpdateOtherAbilityButtons();
        }
    }*/

    public void UpdateOtherAbilityButtons()
    {
        SetOtherAbilityButtonsInteractable(towerAbilityPurchased);
        
    }

    private void SetOtherAbilityButtonsInteractable(bool interactable)
    {
        foreach (Button button in otherAbilityButtons)
        {
            button.interactable = interactable;
        }
    }
}

