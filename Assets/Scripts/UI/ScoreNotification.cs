using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreNotification : MonoBehaviour
{
    private readonly static int ySpacing = 10;

    [Range(0.01f, 0.05f)] public float charWaitTime;
    public GameObject textDisplay;
    public ScoreItem[] scoreList;

    Dictionary<ScoreType, ScoreItem> scoreData;

    int yAnchor = 0;
    int waitingLine = 0;

    void Start()
    {
        scoreData = new Dictionary<ScoreType, ScoreItem>();

        foreach(ScoreItem sI in scoreList)
            scoreData.Add(sI.scoreType, sI);
    }    

    public void Play(ScoreType scoreType, int points) { StartCoroutine(Play(scoreData[scoreType], points)); }

    // OVERWATCH 6k HIGH OCTANE UI 
    IEnumerator Play(ScoreItem data, int points)
    {
        string text = "[" + data.text;
        GameObject display = Instantiate(textDisplay, Vector3.zero, Quaternion.identity, this.transform);
        AudioSource source = display.gameObject.GetComponent<AudioSource>();
        TextMeshProUGUI ui = display.GetComponent<TextMeshProUGUI>();
        
        ui.rectTransform.localPosition = new Vector3(0, yAnchor, 0);
        yAnchor -= ySpacing;
        waitingLine++;

        ui.color = Color.clear;
        ui.SetText("");

        if(points > 0)  { text += " +" + points + "]"; }
        else            { text += " " + points + "]"; }
        
        DOTween.To(() => ui.color, x => ui.color = x, data.color, 0.5f);
        ui.rectTransform.DOLocalMoveY(ui.rectTransform.localPosition.y + 15, 0.5f);

        foreach(char c in text)
        {
            ui.SetText(ui.text + c);
            yield return new WaitForSeconds(charWaitTime);
        }

        yield return new WaitForSeconds(1);

        DOTween.To(() => ui.color, x => ui.color = x, Color.clear, 0.5f).OnComplete(() => 
        {
            Destroy(display);
            waitingLine--;
            if(waitingLine == 0)
                yAnchor = 0;
        });
    }
}

[System.Serializable]
public class ScoreItem
{
    public ScoreType scoreType;
    public string text;
    public Color color;
}

public enum ScoreType { ELIMINATION, GEM, HID, KILLED }