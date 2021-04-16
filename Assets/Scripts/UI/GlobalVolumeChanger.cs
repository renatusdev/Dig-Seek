using UnityEngine;
using UnityEngine.UI;

public class GlobalVolumeChanger : MonoBehaviour
{
    public Slider slider;

    private void Update() {
        AudioListener.volume = slider.value;
    }
}
