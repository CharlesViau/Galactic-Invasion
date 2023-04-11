using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public AK.Wwise.Event hoverEvent;
    public AK.Wwise.Event onClickEvent;
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
    }

    public void Play_HoverSound()
    {
        if (button == null || button.IsInteractable()) hoverEvent?.Post(gameObject);
    }

    public void Play_OnClickSound()
    {
        onClickEvent?.Post(gameObject);
    }

}
