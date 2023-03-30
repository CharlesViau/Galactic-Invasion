using System.Collections;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 3f;
    private Coroutine _coroutine;

    public static MessageUI Instance { get; private set; }

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
        messageText.gameObject.SetActive(false);
    }

    public void Show(string message, int duration = 3)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        messageText.text = message;
        messageText.gameObject.SetActive(true);
        _coroutine = StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(messageDuration);
        messageText.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        messageText.text = text;
    }
}