using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public bool reload;

    public void LoadLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if(reload)
            index--;

        StartCoroutine(Load(index));
    }

    IEnumerator Load(int index)
    {
        transition.SetTrigger("Play");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }
}
