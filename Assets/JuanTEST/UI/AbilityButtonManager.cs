using Motherbase;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonManager : MonoBehaviour
{
    public Button abilityButton;
    private ButtonCostDisplay _buttonCostDisplay;
    private PlayerCurrency _playerCurrency;

    private void Start()
    {
        _playerCurrency = PlayerCurrency.Instance;
        _buttonCostDisplay = GetComponent<ButtonCostDisplay>();

        if (_playerCurrency != null) _playerCurrency.OnBalanceChanged += UpdateAbilityButton;

        UpdateAbilityButton(_playerCurrency.Balance);
    }

    private void Update()
    {
        if (!Controller.Instance.gameStarted) return;
        UpdateAbilityButton(_playerCurrency.Balance);
    }

    private void UpdateAbilityButton(int currentBalance)
    {
        if (_playerCurrency != null && _buttonCostDisplay != null)
            abilityButton.interactable = currentBalance >= _buttonCostDisplay.cost;
    }
}