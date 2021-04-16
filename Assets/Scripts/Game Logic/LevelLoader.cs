using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public readonly static int transitionInAnimationTime = 1;
    
    public static LevelLoader i;

    public Animator transition;
    public bool reload;

    private void Awake()
    {
        if(i == null)
            i = this;
        else
            Destroy(this);    
    }

    public void ReturnToMenu()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("[LevelLoader]: Tried returning to menu when you are in the menu.");
        }

        Destroy(GameObject.FindGameObjectWithTag("Scoreboard"));
        StartCoroutine(Load(0, 0));
    }


    public void LoadLevel(float duration)
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if(reload)
            index--;

        StartCoroutine(Load(index, duration));
    }

    public void LoadLevel(int index, float duration)
    {
        StartCoroutine(Load(index, duration));
    }

    public void LoadLevel(float duration, Action callback)
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if(reload)
            index--;

        StartCoroutine(Load(index, duration, callback));
    }

    IEnumerator Load(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        transition.SetTrigger("Play");
        yield return new WaitForSeconds(transitionInAnimationTime);
        SceneManager.LoadScene(index);
    }

    IEnumerator Load(int index, float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        transition.SetTrigger("Play");
        yield return new WaitForSeconds(transitionInAnimationTime);
        callback();

        SceneManager.LoadScene(index);
    }
}
