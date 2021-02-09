using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(int index)
    {
        Destroy(GameObject.FindGameObjectWithTag("Scoreboard"));
        LevelLoader.i.LoadLevel(index, 0);
    }
}