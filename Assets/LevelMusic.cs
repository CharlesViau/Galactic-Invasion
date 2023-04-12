using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AK.Wwise.Event stopMenuMusic;
    public AK.Wwise.Event startLevelMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        stopMenuMusic?.Post(gameObject);
        startLevelMusic?.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
