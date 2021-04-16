using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

// Some logic of this is wrong
[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    public int startTime;
    public Color colorOnSubstraction;

    private TextMeshProUGUI ui;
    private bool stop;
    private bool start;
    private float counter;
    private float originalFontSize;
    private Vector3 originalPosition;
    public Action actionOnStop { private get; set; }

    void Start()
    {
        ui = GetComponent<TextMeshProUGUI>();
        originalFontSize = ui.fontSize;
        originalPosition = ui.rectTransform.localPosition;
        Reset();
    }

    public void Reset()
    {
        stop = false;
        counter = startTime;
        ui.SetText(counter.ToString("F2"));
    }

    public void Substract(int amount)
    {
        counter -= Mathf.Abs(amount);
        
        if(counter <= 0)
            counter = 0;
        
        ui.SetText(counter.ToString("F2"));

        ui.transform.DOPunchPosition(new Vector3(25, -30), 0.4f, 7, 0.7f).OnComplete(() => ui.rectTransform.localPosition = originalPosition);
        DOTween.To(x => ui.fontSize = x, ui.fontSize, ui.fontSize + 12, 0.3f).SetEase(Ease.InOutElastic).OnComplete(() => ui.fontSize = originalFontSize);
        DOTween.To(() => ui.color, x => ui.color = x, colorOnSubstraction, 0.3f).SetEase(Ease.InOutElastic).OnComplete(() => ui.color = Color.white);
    }

    void Update()
    {
        if(stop)
            return;

        if(counter > 0)
        {
            counter -= Time.deltaTime;
            ui.SetText(counter.ToString("F2"));
        }
        else
        {
            Stop(actionOnStop);
        }
    }

    public void Stop(Action action) { stop = true; counter = 0; action(); }

    public void Stop() { stop = true; counter = 0;}
 
    public float GetTime() { return counter; }
}
