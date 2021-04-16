using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string url;

    public void PlayURL()
    {
        if(url.Length > 0)
            Application.OpenURL(url);
    }
}
