using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class MenuButton : MonoBehaviour
{
    public AK.Wwise.Event hoverEvent;
    public AK.Wwise.Event onClickEvent;

    public void Play_HoverSound()
    {
        hoverEvent?.Post(gameObject);
    }

    public void Play_OnClickSound()
    {
        onClickEvent?.Post(gameObject);
    }

}
