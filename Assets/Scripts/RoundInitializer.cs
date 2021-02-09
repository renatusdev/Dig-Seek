using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RoundInitializer : MonoBehaviour
{
    public Timer timerToEnable;
    public GameObject enemy;
    public ZoomTool camZoom;

    public AudioClip hookSFX;
    public AudioClip levelMusic;
    
    public TextMeshProUGUI ui;
    
    private InputController controllerToEnable;

    void Start()
    {
        controllerToEnable = InputController.i;

        timerToEnable.enabled = false;
        controllerToEnable.enabled = false;

        enemy.GetComponent<Controller>().Freeze();

        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        GameObject musicManager= GameObject.FindGameObjectWithTag("Music Manager"); 

        AudioSource source;

        if(musicManager != null)
        {
            source = musicManager.GetComponent<AudioSource>();

            source.clip = hookSFX;
            source.loop = false;
            source.Play();
        }
        else
        {
            source = null;
        }
        

        ui.SetText("");
        camZoom.ZoomPunch(new Vector3(-12, 1, -14), 5, 2, 1, 1);
        
        yield return new WaitForSeconds(0.4f);
        
        ui.SetText("Ready?");
        ui.transform.DOPunchScale(Vector2.one, 0.1f, 6, 0.4f);
        ui.transform.DOPunchRotation(Vector3.forward * 25, 0.1f, 10, 0);

        yield return new WaitForSeconds(3.05f);
        ui.SetText("Hide!");
        ui.transform.DOPunchScale(Vector2.one, 0.2f, 3, 0.1f).OnComplete(() 
            => ui.transform.DOScale(Vector2.zero, 0.4f).OnComplete(()
                => { ui.SetText(""); ui.transform.localScale = Vector2.one; }));
        ui.transform.DOPunchRotation(Vector3.forward * 90, 0.4f, 3, 0.2f);

        timerToEnable.enabled = true;
        controllerToEnable.enabled = true;
        enemy.GetComponent<Controller>().Unfreeze();

        yield return new WaitForSeconds(hookSFX.length - 3.45f);

        if(musicManager != null)
        {
            source.clip = levelMusic;
            source.loop = true;
            source.Play();
        }
    }
}
