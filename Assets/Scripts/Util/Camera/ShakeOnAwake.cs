using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ShakeOnAwake : MonoBehaviour
{
    [Range(0,10)]   public float magnitude;
    [Range(0,30)]   public float roughness;
    [Range(0,1)]    public float fadeInTime;
    [Range(0,1)]    public float fadeOutTime;

    void Start() {  CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime); }
}
