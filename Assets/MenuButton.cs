using UnityEngine;
using UnityEngine.UI;
using Event = AK.Wwise.Event;

public class MenuButton : MonoBehaviour
{
    public Event hoverEvent;
    public Event onClickEvent;
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
    }

    public void Play_HoverSound()
    {
        if (_button == null || _button.IsInteractable()) hoverEvent?.Post(gameObject);
    }

    public void Play_OnClickSound()
    {
        onClickEvent?.Post(gameObject);
    }
}