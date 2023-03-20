using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TableScore : MonoBehaviour
{
    [SerializeField] private TMP_Text name1;
    [SerializeField] private TMP_Text score;

    public TMP_Text Name
    {
        get => name1;
        set => name1 = value;
    }

    public TMP_Text Score
    {
        get => score;
        set => score = value;
    }
}
