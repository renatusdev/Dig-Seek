using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Make Monostate
[RequireComponent(typeof(Image))]
public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash i;

    [Header("When the alpha color reaches this value, the color reset occurs.")]
    [Range(0, 1)] public float maxAlpha;

    [Range(1,30)] public int flashSpeed;

    private Image flash;

    private void Awake()
    {
        if(i == null)
            i = this;
        else
            Destroy(this);
    }


    void Start()
    {
        // Get Image
        flash = GetComponent<Image>();
    }

    public void Flash(Color color)
    {
        // Make sure color is transparent
        flash.color = new Color(color.r, color.b, color.g, 0);
        StartCoroutine(Flashing(color, flashSpeed));
    }

    public void Flash(Color color, int flashSpeed)
    {
        // Make sure color is transparent
        flash.color = new Color(color.r, color.b, color.g, 0);
        StartCoroutine(Flashing(color, flashSpeed));
    }

    private IEnumerator Flashing(Color color, int flashSpeed)
    {
        while(flash.color.a < maxAlpha)
        {
            flash.color = Color.Lerp(flash.color, color, Time.deltaTime * flashSpeed);
            yield return null;
        }

        // Return back to transparent
        flash.color = new Color(color.r, color.b, color.g, 0);
    }
}