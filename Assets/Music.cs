using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AK.Wwise.Event playMenuMusic;
    public AK.Wwise.Event stopLevelMusic;

    // Start is called before the first frame update
    void Start()
    {
        stopLevelMusic?.Post(gameObject);
        playMenuMusic?.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
