using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// NOTE: Requires child objects with TextMeshProUGUI component.
public class ButtonSelector : MonoBehaviour
{
    GameObject currentButton;

    private void Start()
    {
        // Sets the first selected button.
        if (EventSystem.current.currentSelectedGameObject == null || currentButton == null)
        {
            currentButton = GetComponentInChildren<Button>().gameObject;
            EventSystem.current.SetSelectedGameObject(currentButton);
            AddSelectionUI(currentButton.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void OnEnable()
    {
        // If a currentButton is not null, then it was previously given a value
        // meaning we are returning to a previously disabled menu, in which
        // case we update the event system accordingly.
        if(currentButton != null)
            EventSystem.current.SetSelectedGameObject(currentButton);
    }

    void Update()
    {
        // If player clicks, this will return to null; so we immediately fix by reselecting curr button.
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(currentButton);
    
        // What is this?
        if(currentButton == null & EventSystem.current.currentSelectedGameObject != null)
        {
            currentButton = EventSystem.current.currentSelectedGameObject;
            AddSelectionUI(currentButton.GetComponentInChildren<TextMeshProUGUI>());
        }
        
        // If the event has a new selected button than the one we have
        // this means that the player has selected another button
        if(!currentButton.Equals(EventSystem.current.currentSelectedGameObject))
        {
            TextMeshProUGUI ui = currentButton.GetComponentInChildren<TextMeshProUGUI>();
            ui.text = ui.text.Trim('<', '>');
            currentButton = EventSystem.current.currentSelectedGameObject;
            
            AddSelectionUI(currentButton.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    void AddSelectionUI(TextMeshProUGUI ui) { ui.text = "<" + ui.text + ">"; }
}
