using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForButton : MonoBehaviour 
{
    public string Button_To_Check;
    public Button.ButtonClickedEvent Actions;

    void Update()
    {
        if (Input.GetButtonDown(Button_To_Check))
        {
            Actions.Invoke();
        }
    }
}
