using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCursor : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update() {
        // Debug.Log(Cursor.visible);
    }
}
