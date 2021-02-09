using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TweenTool : MonoBehaviour
{
    public float delay;
    public bool onAwake;
    public bool resetOnDisable;
    public Ease ease;
    public float duration;
    public Vector3 to;

    public UnityEvent m_eventOnComplete;

    Vector3 from;

    private void Start()
    {
        from = transform.localScale;
    }

    private void OnEnable()
    {
        if(resetOnDisable)
            transform.localScale = from;

        if(onAwake)
            transform.DOScale(to, duration).SetEase(ease).SetDelay(delay).OnComplete(() => m_eventOnComplete.Invoke());
    }
}
