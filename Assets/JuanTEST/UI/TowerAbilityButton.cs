using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAbilityButton : MonoBehaviour
{
    public Button towerAbilityButton;
    public List<Button> otherAbilityButtons;

    private bool towerAbilityPurchased;

    private void Start()
    {
        towerAbilityButton.onClick.AddListener(OnTowerAbilityButtonClicked);
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

    private void OnTowerAbilityButtonClicked()
    {
        if (!towerAbilityPurchased)
        {
            towerAbilityPurchased = true;
            UpdateOtherAbilityButtons();
        }
    }

    private void UpdateOtherAbilityButtons()
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

