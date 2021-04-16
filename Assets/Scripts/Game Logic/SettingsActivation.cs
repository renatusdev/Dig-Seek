using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsActivation : MonoBehaviour
{
    public GameObject settings;
    private InputController input;
    public Timer timer;
    public Controller hider;
    public Controller seeker;
    public Scoreboard scoreboard;

    bool frozen;

    private void Start() {
        input = InputController.i;
    }


    void Update()
    {
        if(scoreboard.pointGiven)
        {
            if(frozen)
            {
                FreezeGame(false);
            }
            return;
        }

        if(input.Escape)
        {
            if(settings.activeSelf)
            {
                FreezeGame(false);
            }
            else
            {
                FreezeGame(true);
            }
        }
    }

    public void FreezeGame(bool freeze)
    {
        frozen = freeze;
        settings.SetActive(freeze);
        timer.gameObject.SetActive(!freeze);
        
        if(freeze)
        {
            // Find hider and seeker in game
            if(hider == null || seeker == null)
            {
                hider = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
                seeker = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Controller>();
            }

            hider.Freeze();
            seeker.Freeze();
        }
        else
        {
            hider.Unfreeze();
            seeker.Unfreeze();
        }
    }
}
