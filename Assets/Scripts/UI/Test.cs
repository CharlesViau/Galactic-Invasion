using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    private Button bouton;
    private void Awake()
    {
        bouton = GetComponent<Button>();
    }
}