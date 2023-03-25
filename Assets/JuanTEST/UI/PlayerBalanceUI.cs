using Motherbase;
using TMPro;
using UnityEngine;

public class PlayerBalanceUI : MonoBehaviour
{
    public GameObject player;
    private TextMeshProUGUI _balanceText;
    private PlayerCurrency _playerCurrency;

    private void Start()
    {
        _playerCurrency = player.GetComponent<PlayerCurrency>();
        _balanceText = GetComponent<TextMeshProUGUI>();

        _playerCurrency.OnBalanceChanged += UpdateBalanceText;
        UpdateBalanceText(_playerCurrency.Balance);
    }

    private void OnDestroy()
    {
        _playerCurrency.OnBalanceChanged -= UpdateBalanceText;
    }

    private void UpdateBalanceText(int newBalance)
    {
        _balanceText.text = $"Balance: {newBalance}";
    }
}