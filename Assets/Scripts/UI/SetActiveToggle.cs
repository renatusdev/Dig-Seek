using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveToggle : MonoBehaviour
{
    public GameObject toToggle;
    public string tagToggle;


    public void Toggle()
    {
        if(toToggle == null)
        {
            toToggle = GameObject.FindGameObjectWithTag(tagToggle);
        }
        toToggle.SetActive(!toToggle.activeInHierarchy);
    }
}
