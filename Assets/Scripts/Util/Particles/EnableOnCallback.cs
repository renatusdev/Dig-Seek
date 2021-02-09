using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EnableOnCallback : MonoBehaviour
{
    public GameObject[] toEnable;
    public bool destroyOnCallback;

    void OnParticleSystemStopped()
    {
        foreach(GameObject g in toEnable)
            g.SetActive(true);
        
        if(destroyOnCallback)
            Destroy(this.gameObject);
    }

    // Make sure particle system has callback stop action.
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
}
