using TMPro;
using UnityEngine;

public class CostUI : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public static CostUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        costText.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        costText.text = text;
    }

    public void Show()
    {
        /*
        costText.gameObject.SetActive(true);
    */
    }

    public void Hide()
    {
        costText.gameObject.SetActive(false);
    }
}