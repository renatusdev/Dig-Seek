using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static bool singleton = false; 

    [System.Serializable]
    public class SceneMusicEvent
    {
        public string sceneName;
        public UnityEvent eventAction;
    }

    public SceneMusicEvent[] m_SceneMusicEvents;

    void Awake()
    {
        if(singleton)
        {
            Destroy(this.gameObject);
        }
        else
        {
            singleton = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {   
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach(SceneMusicEvent e in m_SceneMusicEvents)
        {
            if(scene.name.Equals(e.sceneName))
            {
                e.eventAction.Invoke();
            }
        }
        return;
    }
}