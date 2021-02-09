using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Make Monostate
[RequireComponent(typeof(Image))]
public class ImageEffects : MonoBehaviour
{
    public static ImageEffects i;

    [Tooltip("When the alpha color reaches this value, the color reset occurs.")]
    [Range(0, 1)] public float maxAlpha;
    
    private Image image;

    private void Awake()
    {
        if(i == null)
            i = this;
        else
            Destroy(this);
    }

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void Flash(Color color, float speed)
    {
        image.color = new Color(color.r, color.b, color.g, 0);
        StartCoroutine(Interpolate(color, speed, false, 0));
    }

    public void Dim(Color color, float speed, float timeToHoldBeforeReset)
    {
        image.color = new Color(color.r, color.b, color.g, 0);
        StartCoroutine(Interpolate(color, speed, true, timeToHoldBeforeReset));
    }

    private IEnumerator Interpolate(Color color, float speed, bool holdBeforeReset, float timeToHoldBeforeReset)
    {
        while(image.color.a < maxAlpha)
        {
            image.color = Color.Lerp(image.color, color, Time.deltaTime * speed);
            yield return null;
        }

        if(holdBeforeReset)
            yield return new WaitForSeconds(timeToHoldBeforeReset);

        image.color = new Color(color.r, color.b, color.g, 0);
    }
}