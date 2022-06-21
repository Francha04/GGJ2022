using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class musicManager : MonoBehaviour
{
    [SerializeField] StudioEventEmitter mainThemeEmitter;
    [SerializeField] StudioEventEmitter loseThemeEmitter;
    [SerializeField] GameState gState;
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (!mainThemeEmitter.IsPlaying() && gState.state == State.Play) 
        {
            mainThemeEmitter.Play();
        }
    }
    public void loseGameMusic() 
    {
        mainThemeEmitter.Stop();
        loseThemeEmitter.Play();
    }
}
